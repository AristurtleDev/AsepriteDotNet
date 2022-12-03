/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */
using System.Diagnostics;
using System.Drawing;

using AsepriteDotNet.Document;
using AsepriteDotNet.Compression;

namespace AsepriteDotNet.IO;

/// <summary>
///     Utility class for reading an Aseprite file from disk.
/// </summary>
public static class AsepriteFileReader
{
    private const ushort ASE_HEADER_MAGIC = 0xA5E0;                 //  File Header Magic Number
    private const int ASE_HEADER_SIZE = 128;                        //  File Header Length, In Bytes
    private const uint ASE_HEADER_FLAG_LAYER_OPACITY_VALID = 1;     //  Header Flag (Is Layer Opacity Valid)

    private const ushort ASE_FRAME_MAGIC = 0xF1FA;                  //  Frame Magic Number

    private const ushort ASE_CHUNK_OLD_PALETTE1 = 0x0004;           //  Old Palette Chunk
    private const ushort ASE_CHUNK_OLD_PALETTE2 = 0x0011;           //  Old Palette Chunk
    private const ushort ASE_CHUNK_LAYER = 0x2004;                  //  Layer Chunk
    private const ushort ASE_CHUNK_CEL = 0x2005;                    //  Cel Chunk
    private const ushort ASE_CHUNK_CEL_EXTRA = 0x2006;              //  Cel Extra Chunk
    private const ushort ASE_CHUNK_COLOR_PROFILE = 0x2007;          //  Color Profile Chunk
    private const ushort ASE_CHUNK_EXTERNAL_FILES = 0x2008;         //  External Files Chunk
    private const ushort ASE_CHUNK_MASK = 0x2016;                   //  Mask Chunk (deprecated)
    private const ushort ASE_CHUNK_PATH = 0x2017;                   //  Path Chunk (never used)
    private const ushort ASE_CHUNK_TAGS = 0x2018;                   //  Tags Chunk
    private const ushort ASE_CHUNK_PALETTE = 0x2019;                //  Palette Chunk
    private const ushort ASE_CHUNK_USER_DATA = 0x2020;              //  User Data Cunk
    private const ushort ASE_CHUNK_SLICE = 0x2021;                  //  Slice Chunk
    private const ushort ASE_CHUNK_TILESET = 0x2023;                //  Tileset Chunk

    private const ushort ASE_LAYER_TYPE_NORMAL = 0;                 //  Layer Type Normal (Image) Layer
    private const ushort ASE_LAYER_TYPE_GROUP = 1;                  //  Layer Type Group
    private const ushort ASE_LAYER_TYPE_TILEMAP = 2;                //  Layer Type Tilemap

    private const ushort ASE_LAYER_FLAG_VISIBLE = 1;                //  Layer Flag (Is Visible)
    private const ushort ASE_LAYER_FLAG_EDITABLE = 2;               //  Layer Flag (Is Editable)
    private const ushort ASE_LAYER_FLAG_LOCKED = 4;                 //  Layer Flag  (Movement Locked)
    private const ushort ASE_LAYER_FLAG_BACKGROUND = 8;             //  Layer Flag (Is Background Layer)
    private const ushort ASE_LAYER_FLAG_PREFERS_LINKED = 16;        //  Layer Flag (Prefers Linked Cels)
    private const ushort ASE_LAYER_FLAG_COLLAPSED = 32;             //  Layer Flag (Displayed Collapsed)
    private const ushort ASE_LAYER_FLAG_REFERENCE = 64;             //  Layer Flag (Is Reference Layer)

    private const ushort ASE_CEL_TYPE_RAW_IMAGE = 0;                //  Cel Type (Raw Image)
    private const ushort ASE_CEL_TYPE_LINKED = 1;                   //  Cel Type (Linked)
    private const ushort ASE_CEL_TYPE_COMPRESSED_IMAGE = 2;         //  Cel Type (Compressed Image)
    private const ushort ASE_CEL_TYPE_COMPRESSED_TILEMAP = 3;       //  Cel Type (Compressed Tilemap)

    private const uint ASE_CEL_EXTRA_FLAG_PRECISE_BOUNDS_SET = 1;   //  Cel Extra Flag (Precise Bounds Set)

    private const ushort ASE_PALETTE_FLAG_HAS_NAME = 1;             //  Palette Flag (Color Has Name)

    private const uint ASE_USER_DATA_FLAG_HAS_TEXT = 1;             //  User Data Flag (Has Text)
    private const uint ASE_USER_DATA_FLAG_HAS_COLOR = 2;            //  User Data Flag (Has Color)

    private const uint ASE_SLICE_FLAGS_IS_NINE_PATCH = 1;           //  Slice Flag (Is 9-Patch Slice)
    private const uint ASE_SLICE_FLAGS_HAS_PIVOT = 2;               //  Slice Flag (Has Pivot Information)

    private const uint ASE_TILESET_FLAG_EXTERNAL_FILE = 1;          //  Tileset Flag (Includes Link To External File)
    private const uint ASE_TILESET_FLAG_EMBEDDED = 2;               //  Tileset Flag (Includes Tiles Inside File)

    private const byte TILE_ID_SHIFT = 0;                           //  Tile ID Bitmask Shift
    private const uint TILE_ID_MASK = 0x1fffffff;                   //  Tile ID Bitmask
    private const uint TILE_FLIP_X_MASK = 0x20000000;               //  Tile Flip X Bitmask
    private const uint TILE_FLIP_Y_MASK = 0x40000000;               //  Tile Flip Y Bitmask
    private const uint TILE_90CW_ROTATION_MASK = 0x80000000;        //  Tile 90CW Rotation Bitmask

    /// <summary>
    ///     Reads the Asperite file from the specified <paramref name="path"/>.
    /// </summary>
    /// <param name="path">
    ///     The absolute file path to the Aseprite file to read.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if invalid data is found within the Asperite file while it
    ///     is being read. The exception message contains the details on what
    ///     value was invalid.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified <paramref name="path"/> is a zero-length
    ///     string, contains only white space, or contains one ore more
    ///     invalid characters. Use 
    ///     <see cref="System.IO.Path.GetInvalidPathChars"/> to query for 
    ///     invalid characters.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="path"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="PathTooLongException">
    ///     Thrown if the specified <paramref name="path"/>, file name, or
    ///     both exceed the system-defined maximum length.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    ///     Thrown if the specified <paramref name="path"/> is invalid (for
    ///     example, it is on an unmapped drive).
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if the specified <paramref name="path"/> is a directory or
    ///     the caller does not have the required permissions.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    ///     Thrown if the file specified in the <paramref name="path"/> is not
    ///     found.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown if the <paramref name="path"/> is in an inavlid format.
    /// </exception>
    /// <exception cref="IOException">
    ///     Thrown if an I/O error occurs while attempting to open the file.
    /// </exception>
    public static AsepriteFile ReadFile(string path)
    {
        using AsepriteBinaryReader reader = new(File.OpenRead(path));
        return ReadFile(reader);
    }

    internal static AsepriteFile ReadFile(AsepriteBinaryReader reader)
    {
        AsepriteFile doc = new();

        //  Reference to the last group layer that is read so that subsequent
        //  child layers can be added to it.
        GroupLayer? lastGroupLayer = default;

        //  Read the Aseprite file header
        _ = reader.ReadDword();             //  File size (ignored, don't need)
        ushort hmagic = reader.ReadWord();  //  Header magic number

        if (hmagic != ASE_HEADER_MAGIC)
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid header magic number (0x{hmagic:X4}). This does not appear to be a valid Aseprite file");
        }

        ushort nframes = reader.ReadWord(); //  Total number of frames
        ushort width = reader.ReadWord();   //  Width, in pixels
        ushort height = reader.ReadWord();  //  Height, in pixels

        if (width < 1 || height < 1)
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid canvas size {width}x{height}.");
        }

        doc.Size = new Size(width, height);

        ushort depth = reader.ReadWord();   //  Color depth (bits per pixel)

        if (!Enum.IsDefined<ColorDepth>((ColorDepth)depth))
        {
            reader.Dispose();
            throw new InvalidOperationException($"Invalid color depth: {depth}");
        }

        doc.ColorDepth = (ColorDepth)depth;

        uint hflags = reader.ReadDword();   //  Header flags

        bool isLayerOpacityValid = HasFlag(hflags, ASE_HEADER_FLAG_LAYER_OPACITY_VALID);

        if (!isLayerOpacityValid)
        {
            doc.AddWarning("Layer opacity valid flag is not set. All layer opacity will default to 255");
        }

        _ = reader.ReadWord();                      //  Speed (ms between frames) (deprecated)
        _ = reader.ReadDword();                     //  Set to zero (ignored)
        _ = reader.ReadDword();                     //  Set to zero (ignored)
        byte transparentIndex = reader.ReadByte();  //  Index of transparent color in palette

        if (doc.ColorDepth != ColorDepth.Indexed)
        {
            //  Transparent color index is only valid in indexed depth
            transparentIndex = 0;
            doc.AddWarning("Transparent index only valid for Indexed Color Depth. Defaulting to 0");
        }

        doc.Palette.TransparentIndex = transparentIndex;

        _ = reader.ReadBytes(3);            //  Ignore these bytes
        ushort ncolors = reader.ReadWord(); //  Number of colors

        //  Remainder of header is not needed, skipping to end of header
        reader.Seek(ASE_HEADER_SIZE);

        //  Read frame-by-frame until all frames are read.
        for (int fnum = 0; fnum < nframes; fnum++)
        {
            Frame frame = new();

            //  Reference to the last cel that was read so we can apply a 
            //  Cel Extra chunk to it if one is read after.
            Cel? lastCel = default;

            //  Reference to the last chunk that can have user data so we can
            //  apply a User Data chunk to it when one is read.
            IUserData? lastUserData = default;

            //  Tracks the iteration of the tags when reading user data for
            //  tags chunk.
            int tagIterator = 0;

            //  Read the frame header
            _ = reader.ReadDword();             //  Bytes in frame (don't need, ignored)
            ushort fmagic = reader.ReadWord();  //  Frmae magic number

            if (fmagic != ASE_FRAME_MAGIC)
            {
                reader.Dispose();
                throw new InvalidOperationException($"Invalid frame magic number (0x{fmagic:X4}) in frame {fnum}.");
            }

            int nchunks = reader.ReadWord();        //  Old field which specified chunk count
            frame.Duration = reader.ReadWord();     //  Frame duration, in milliseconds
            _ = reader.ReadBytes(2);                //  For future (set to zero)
            uint moreChunks = reader.ReadDword();   //  New field which specifies chunk count

            //  Determine which chunk count to use
            if (nchunks == 0xFFFF && nchunks < moreChunks)
            {
                nchunks = (int)moreChunks;
            }

            //  Read chunk-by-chunk until all chunks in this frame are read.
            for (int cnum = 0; cnum < nchunks; cnum++)
            {
                long cstart = reader.Position;
                uint clen = reader.ReadDword();     //  Size of chunk, in bytes
                ushort ctype = reader.ReadWord();   //  The type of chunk
                long cend = cstart + clen;

                if (ctype == ASE_CHUNK_LAYER)
                {
                    ushort lflags = reader.ReadWord();  //  Layer flags
                    ushort ltype = reader.ReadWord();   //  Layer type
                    ushort level = reader.ReadWord();   //  Layer child level
                    _ = reader.ReadWord();              //  Default layer width (ignored)
                    _ = reader.ReadWord();              //  Default layer height (ignored)
                    ushort blend = reader.ReadWord();   //  Blend mode
                    byte opacity = reader.ReadByte();   //  Layer opacity
                    _ = reader.ReadBytes(3);            //  For future (set to zero)
                    string name = reader.ReadString();  //  Layer name

                    if (!isLayerOpacityValid)
                    {
                        opacity = 255;
                    }

                    //  Validate blend mode
                    if (!Enum.IsDefined<BlendMode>((BlendMode)blend))
                    {
                        reader.Dispose();
                        throw new InvalidOperationException($"Unknown blend mode '{blend}' foudn in layer '{name}'");
                    }

                    Layer layer;

                    if (ltype == ASE_LAYER_TYPE_NORMAL)
                    {
                        layer = new ImageLayer()
                        {
                            ChildLevel = level,
                            BlendMode = (BlendMode)blend,
                            Opacity = opacity,
                            Name = name
                        };
                    }
                    else if (ltype == ASE_LAYER_TYPE_GROUP)
                    {
                        layer = new GroupLayer()
                        {
                            ChildLevel = level,
                            BlendMode = (BlendMode)blend,
                            Opacity = opacity,
                            Name = name
                        };
                    }
                    else if (ltype == ASE_LAYER_TYPE_TILEMAP)
                    {
                        uint index = reader.ReadDword();    //  Tilset index

                        layer = new TilemapLayer()
                        {
                            ChildLevel = level,
                            BlendMode = (BlendMode)blend,
                            Opacity = opacity,
                            Name = name,
                            TilesetIndex = (int)index
                        };
                    }
                    else
                    {
                        reader.Dispose();
                        throw new InvalidOperationException($"Unknown layer type '{ltype}'");
                    }

                    layer.IsVisible = HasFlag(lflags, ASE_LAYER_FLAG_VISIBLE);
                    layer.IsBackgroundLayer = HasFlag(lflags, ASE_LAYER_FLAG_BACKGROUND);
                    layer.IsReferenceLayer = HasFlag(lflags, ASE_LAYER_FLAG_REFERENCE);

                    if (level != 0 && lastGroupLayer is not null)
                    {
                        lastGroupLayer.AddChild(layer);
                    }

                    if (layer is GroupLayer gLayer)
                    {
                        lastGroupLayer = gLayer;
                    }

                    lastUserData = layer;
                    doc.Add(layer);

                }
                else if (ctype == ASE_CHUNK_CEL)
                {
                    ushort index = reader.ReadWord();   //  Layer index
                    short x = reader.ReadShort();       //  X position
                    short y = reader.ReadShort();       //  Y position
                    byte opacity = reader.ReadByte();   //  Opacity level
                    ushort type = reader.ReadWord();    //  Cel type
                    _ = reader.ReadBytes(7);            //  For future (set to zero)

                    Cel cel;

                    if (type == ASE_CEL_TYPE_RAW_IMAGE)
                    {
                        ushort w = reader.ReadWord();                   //  Width, in pixels
                        ushort h = reader.ReadWord();                   //  Height, in pixels
                        byte[] pixelData = reader.ReadToPosition(cend); //  Raw pixel data

                        Color[] pixels = PixelsToColor(pixelData, doc.ColorDepth, doc.Palette);

                        cel = new ImageCel()
                        {
                            Size = new Size(w, h),
                            Pixels = pixels
                        };
                    }
                    else if (type == ASE_CEL_TYPE_LINKED)
                    {
                        ushort findex = reader.ReadWord();  //  Frame position to link with

                        cel = new LinkedCel()
                        {
                            Frame = findex
                        };
                    }
                    else if (type == ASE_CEL_TYPE_COMPRESSED_IMAGE)
                    {
                        ushort w = reader.ReadWord();                   //  Width, in pixels
                        ushort h = reader.ReadWord();                   //  Height, in pixels
                        byte[] compressed = reader.ReadToPosition(cend); //  Raw pixel data compressed with Zlib
                        byte[] pixelData = Zlib.Deflate(compressed);
                        Color[] pixels = PixelsToColor(pixelData, doc.ColorDepth, doc.Palette);

                        cel = new ImageCel()
                        {
                            Size = new Size(w, h),
                            Pixels = pixels
                        };
                    }
                    else if (type == ASE_CEL_TYPE_COMPRESSED_TILEMAP)
                    {
                        ushort w = reader.ReadWord();                       //  Width, in number of tiles
                        ushort h = reader.ReadWord();                       //  Height, in number of tiles
                        ushort bpt = reader.ReadWord();                     //  Bits per tile
                        uint id = reader.ReadDword();                       //  Bitmask for Tile ID
                        uint xflip = reader.ReadDword();                    //  Bitmask for X Flip
                        uint yflip = reader.ReadDword();                    //  Bitmask for Y Flip
                        uint rotation = reader.ReadDword();                 //  Bitmask for 90CW rotation
                        _ = reader.ReadBytes(10);                           //  Reserved
                        byte[] compressed = reader.ReadToPosition(cend);    //  Raw tile data compressed with Zlib

                        // cel = new TilemapCel()
                        // {
                        //     Size = new Size(w, h),
                        //     BitsPerTile = bpt,
                        //     TileIdBitmask = id,
                        //     XFlipBitmask = xflip,
                        //     YFlipBitmask = yflip,
                        //     RotationBitmask = rotation,
                        //     Tiles = Zlib.Deflate(compressed)
                        // };

                        byte[] tileData = Zlib.Deflate(compressed);
                        //  Per Aseprite file spec, the "bits" per tile is, at
                        //  the moment, always 32-bits.  This means it's 4-bytes
                        //  per tile (32 / 8 = 4).  Meaning that each tile value
                        //  is a uint (DWORD)
                        int bytesPerTile = 4;
                        Tile[] tiles = new Tile[tileData.Length / bytesPerTile];

                        for (int i = 0, b = 0; i < tiles.Length; i++, b += bytesPerTile)
                        {
                            byte[] dword = tileData[b..(b + bytesPerTile)];
                            uint value = BitConverter.ToUInt32(dword);
                            Tile tile = new Tile
                            {
                                TileID = (value & TILE_ID_MASK) >> TILE_ID_SHIFT,
                                #pragma warning disable 0618
                                XFlip = (value & TILE_FLIP_X_MASK),
                                YFlip = (value & TILE_FLIP_Y_MASK),
                                Rotate90 = (value & TILE_90CW_ROTATION_MASK)
                                #pragma warning restore 0618
                            };
                            tiles[i] = tile;
                        }

                        cel = new TilemapCel()
                        {
                            Size = new Size(w, h),
                            BitsPerTile = bpt,
                            TileIdBitmask = id,
                            XFlipBitmask = xflip,
                            YFlipBitmask = yflip,
                            RotationBitmask = rotation,
                            Tiles = tiles
                        };


                    }
                    else
                    {
                        reader.Dispose();
                        throw new InvalidOperationException($"Unknown cel type '{type}'");
                    }

                    cel.LayerIndex = index;
                    cel.Position = new Point(x, y);
                    cel.Opacity = opacity;

                    lastCel = cel;
                    lastUserData = cel;
                    frame.AddCel(cel);
                }
                else if (ctype == ASE_CHUNK_CEL_EXTRA)
                {
                    uint flags = reader.ReadDword();    //  Flags
                    float x = reader.ReadFixed();       //  Precise X position
                    float y = reader.ReadFixed();       //  Precise Y position
                    float w = reader.ReadFixed();       //  Width of the cel in sprite (scaled in realtime)
                    float h = reader.ReadFixed();       //  Height of the cel in sprite (scaled in realtime)
                    _ = reader.ReadBytes(16);           //  For future (set to zero)

                    CelExtra extra = new()
                    {
                        PreciseBoundsSet = HasFlag(flags, ASE_CEL_EXTRA_FLAG_PRECISE_BOUNDS_SET),
                        PreciseX = x,
                        PreciseY = y,
                        WidthInSprite = w,
                        HeightInSprite = h
                    };

                    if (lastCel is not null)
                    {
                        lastCel.ExtraData = extra;
                        lastCel = null;
                    }
                }
                else if (ctype == ASE_CHUNK_TAGS)
                {
                    ushort ntags = reader.ReadWord();   //  Number of tags
                    _ = reader.ReadBytes(8);            //  For future (set to zero)

                    for (int i = 0; i < ntags; i++)
                    {
                        ushort from = reader.ReadWord();    //  From frame
                        ushort to = reader.ReadWord();      //  To frame
                        byte direction = reader.ReadByte(); //  Loop Direction

                        //  Validate direction value
                        if (!Enum.IsDefined<LoopDirection>((LoopDirection)direction))
                        {
                            reader.Dispose();
                            throw new InvalidOperationException($"Unknown loop direction '{direction}'");
                        }

                        _ = reader.ReadBytes(8);            //  For future (set to zero)
                        byte r = reader.ReadByte();         //  Red RGB value of tag color
                        byte g = reader.ReadByte();         //  Green RGB value of tag color
                        byte b = reader.ReadByte();         //  Blue RGB value of tag color
                        _ = reader.ReadByte();              //  Extra byte (zero)
                        string name = reader.ReadString();  //  Tag name

                        Tag tag = new()
                        {
                            From = from,
                            To = to,
                            LoopDirection = (LoopDirection)direction,
                            Color = Color.FromArgb(255, r, g, b),
                            Name = name
                        };

                        doc.Add(tag);
                    }

                    tagIterator = 0;
                    lastUserData = doc.Tags.FirstOrDefault();
                }
                else if (ctype == ASE_CHUNK_PALETTE)
                {
                    uint nsize = reader.ReadDword();    //  New palette size (total number of entries)
                    uint from = reader.ReadDword();     //  First color index to change
                    uint to = reader.ReadDword();       //  Last color index to change
                    _ = reader.ReadBytes(8);            //  For future (set to zero)

                    if (nsize > 0)
                    {
                        doc.Palette.Resize((int)nsize);
                    }

                    for (uint i = from; i <= to; i++)
                    {
                        ushort flags = reader.ReadWord();
                        byte r = reader.ReadByte();
                        byte g = reader.ReadByte();
                        byte b = reader.ReadByte();
                        byte a = reader.ReadByte();

                        if (HasFlag(flags, ASE_PALETTE_FLAG_HAS_NAME))
                        {
                            _ = reader.ReadString();    //  Color name (ignored)
                        }
                        doc.Palette[(int)i] = Color.FromArgb(a, r, g, b);
                    }
                }
                else if (ctype == ASE_CHUNK_USER_DATA)
                {
                    uint flags = reader.ReadDword();    //  Flags

                    string? text = default;
                    if (HasFlag(flags, ASE_USER_DATA_FLAG_HAS_TEXT))
                    {
                        text = reader.ReadString();     //  User Data text
                    }

                    Color? color = default;
                    if (HasFlag(flags, ASE_USER_DATA_FLAG_HAS_COLOR))
                    {
                        byte r = reader.ReadByte();     //  Color Red (0 - 255)
                        byte g = reader.ReadByte();     //  Color Green (0 - 255)
                        byte b = reader.ReadByte();     //  Color Blue (0 - 255)
                        byte a = reader.ReadByte();     //  Color Alpha (0 - 255)

                        color = Color.FromArgb(a, r, g, b);
                    }

                    Debug.Assert(lastUserData is not null);

                    if (lastUserData is not null)
                    {
                        lastUserData.UserData.Text = text;
                        lastUserData.UserData.Color = color;

                        if (lastUserData is Tag)
                        {

                            //  Tags are a special case, user data for tags 
                            //  comes all together (one next to the other) after 
                            //  the tags chunk, in the same order:
                            //
                            //  * TAGS CHUNK (TAG1, TAG2, ..., TAGn)
                            //  * USER DATA CHUNK FOR TAG1
                            //  * USER DATA CHUNK FOR TAG2
                            //  * ...
                            //  * USER DATA CHUNK FOR TAGn
                            //
                            //  So here we expect that the next user data chunk 
                            //  will correspond to the next tag in the tags 
                            //  collection
                            tagIterator++;

                            if (tagIterator < doc.Tags.Count)
                            {
                                lastUserData = doc.Tags[tagIterator];
                            }
                            else
                            {
                                lastUserData = null;
                            }
                        }

                    }
                }
                else if (ctype == ASE_CHUNK_SLICE)
                {
                    uint nkeys = reader.ReadDword();    //  Number of "slice keys"
                    uint flags = reader.ReadDword();    //  Flags
                    _ = reader.ReadDword();             //  Reserved
                    string name = reader.ReadString();  //  Name

                    Slice slice = new()
                    {
                        Name = name,
                        IsNinePatch = HasFlag(flags, ASE_SLICE_FLAGS_IS_NINE_PATCH),
                        HasPivot = HasFlag(flags, ASE_SLICE_FLAGS_HAS_PIVOT)
                    };

                    for (uint i = 0; i < nkeys; i++)
                    {
                        uint kframe = reader.ReadDword();   //  Frame number this slice is valid starting from
                        int x = reader.ReadLong();          //  Slice X origin coordinate in the sprite
                        int y = reader.ReadLong();          //  Slice Y origin coordinate in the sprite
                        uint w = reader.ReadDword();        //  Slice Width (can be 0 if slice is hidden)
                        uint h = reader.ReadDword();        //  Slice Height (can be 0 if slice is hidden)

                        SliceKey key = new(slice)
                        {
                            Frame = (int)kframe,
                            Bounds = new Rectangle(x, y, (int)w, (int)h)
                        };

                        if (slice.IsNinePatch)
                        {
                            int cx = reader.ReadLong();     //  Center X position (relative to slice bounds)
                            int cy = reader.ReadLong();     //  Center Y position (relative to slice bounds)
                            uint cw = reader.ReadDword();   //  Center width
                            uint ch = reader.ReadDword();   //  Center height

                            key.CenterBounds = new Rectangle(cx, cy, (int)cw, (int)ch);
                        }

                        if (slice.HasPivot)
                        {
                            int px = reader.ReadLong(); //  Pivot X position (relative to the slice origin)
                            int py = reader.ReadLong(); //  Pivot Y position (relative to the slice origin)

                            key.Pivot = new Point(px, py);
                        }
                    }

                    doc.Add(slice);
                    lastUserData = slice;
                }
                else if (ctype == ASE_CHUNK_TILESET)
                {
                    uint id = reader.ReadDword();       //  Tileset ID
                    uint flags = reader.ReadDword();    //  Tileset flags
                    uint count = reader.ReadDword();    //  Number of tiles
                    ushort w = reader.ReadWord();       //  Tile width
                    ushort h = reader.ReadWord();       //  Tile height
                    _ = reader.ReadShort();             //  Base index (ignoring, only used in Asperite UI)
                    _ = reader.ReadBytes(14);           //  Reserved
                    string name = reader.ReadString();  //  Name of tileset


                    if (HasFlag(flags, ASE_TILESET_FLAG_EXTERNAL_FILE))
                    {
                        reader.Dispose();
                        throw new InvalidOperationException($"Tileset '{name}' includes tileset in external file. This is not supported at this time");
                    }

                    if (HasFlag(flags, ASE_TILESET_FLAG_EMBEDDED))
                    {
                        uint len = reader.ReadDword();                  //  Compressed data length
                        byte[] compressed = reader.ReadBytes((int)len); //  Compressed tileset image

                        byte[] pixelData = Zlib.Deflate(compressed);
                        Color[] pixels = PixelsToColor(pixelData, doc.ColorDepth, doc.Palette);

                        Tileset tileset = new()
                        {
                            ID = (int)id,
                            TileCount = (int)count,
                            TileSize = new Size(w, h),
                            Name = name,
                            Pixels = pixels
                        };

                        doc.Add(tileset);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Tileset '{name}' does not include tileset image in file");
                    }
                }
                else if (ctype == ASE_CHUNK_OLD_PALETTE1)
                {
                    doc.AddWarning($"Old Palette Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }
                else if (ctype == ASE_CHUNK_OLD_PALETTE2)
                {
                    doc.AddWarning($"Old Palette Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }
                else if (ctype == ASE_CHUNK_COLOR_PROFILE)
                {
                    doc.AddWarning($"Color Profile Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }
                else if (ctype == ASE_CHUNK_EXTERNAL_FILES)
                {
                    doc.AddWarning($"External Files Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }
                else if (ctype == ASE_CHUNK_MASK)
                {
                    doc.AddWarning($"Mask Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }
                else if (ctype == ASE_CHUNK_PATH)
                {
                    doc.AddWarning($"Path Chunk (0x{ctype:X4}) ignored");
                    reader.Seek(cend);
                }

                Debug.Assert(reader.Position == cend);
            }

            doc.Add(frame);
        }

        if (doc.Palette.Count != ncolors)
        {
            doc.AddWarning($"Number of colors in header ({ncolors}) does not match final palette count ({doc.Palette.Count})");
        }

        return doc;
    }

    private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

    internal static Color[] PixelsToColor(byte[] pixels, ColorDepth depth, Palette palette)
    {
        return depth switch
        {
            ColorDepth.Indexed => IndexedPixelsToColor(pixels, palette),
            ColorDepth.Grayscale => GrayscalePixelsToColor(pixels),
            ColorDepth.RGBA => RGBAPixelsToColor(pixels),
            _ => throw new InvalidOperationException("Unknown Color Depth")
        };
    }

    internal static Color[] RGBAPixelsToColor(byte[] pixels)
    {
        int bytesPerPixel = (int)ColorDepth.RGBA / 8;
        Color[] results = new Color[pixels.Length / bytesPerPixel];

        for (int i = 0, b = 0; i < results.Length; i++, b += bytesPerPixel)
        {
            byte red = pixels[b];
            byte green = pixels[b + 1];
            byte blue = pixels[b + 2];
            byte alpha = pixels[b + 3];
            results[i] = Color.FromArgb(alpha, red, green, blue);
        }

        return results;
    }

    internal static Color[] GrayscalePixelsToColor(byte[] pixels)
    {
        int bytesPerPixel = (int)ColorDepth.Grayscale / 8;
        Color[] results = new Color[pixels.Length / bytesPerPixel];

        for (int i = 0, b = 0; i < results.Length; i++, b += bytesPerPixel)
        {
            byte red = pixels[b];
            byte green = pixels[b];
            byte blue = pixels[b];
            byte alpha = pixels[b + 1];
            results[i] = Color.FromArgb(alpha, red, green, blue);
        }

        return results;
    }

    internal static Color[] IndexedPixelsToColor(byte[] pixels, Palette palette)
    {
        int bytesPerPixel = (int)ColorDepth.Indexed / 8;
        Color[] results = new Color[pixels.Length / bytesPerPixel];

        for (int i = 0; i < pixels.Length; i++)
        {
            int index = pixels[i];

            if (index == palette.TransparentIndex)
            {
                results[i] = Color.FromArgb(0, 0, 0, 0);
            }
            else
            {
                results[i] = palette[index];
            }
        }

        return results;
    }
}