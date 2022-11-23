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
using System.Drawing;

using AsepriteDotNet.Document;
using AsepriteDotNet.Document.Native;
using AsepriteDotNet.IO.Compression;

namespace AsepriteDotNet.IO;

public static class AsepriteFileReader
{
    public static void ReadFile(string path)
    {
        using AsepriteBinaryReader reader = new(File.OpenRead(path));

        RawHeader rawHeader = ReadRawHeader(reader);
        Header header = new(rawHeader);


        ColorProfile? colorProfile;
        List<ExternalFile>? externalFiles = new List<ExternalFile>();
        List<Layer> layers = new List<Layer>();
        List<Tag> tags = new List<Tag>();
        Color[] palette = Array.Empty<Color>();


        for (int fnum = 0; fnum < rawHeader.Frames; fnum++)
        {
            IList<Cel> cels = new List<Cel>();
            Cel? lastCel = default;
            IUserData? lastWithUserData = default;
            int tagIterator = 0;

            RawFrameHeader fHeader = ReadRawFrameHeader(reader);

            //  If the old chunk count is 0xFFFF then there may be more chunks
            //  to read, so we would use the new chunk count, but only if it
            //  is greater than the old chunk count
            int nchunks = fHeader.OldCount;
            if (fHeader.OldCount == 0xFFFF && nchunks < fHeader.NewCount)
            {
                nchunks = (int)fHeader.NewCount;
            }

            for (int chunkNum = 0; chunkNum < nchunks; chunkNum++)
            {
                long chunkStart = reader.Position;

                RawChunkHeader cHeader = ReadRawChunkHeader(reader);

                long chunkEnd = chunkStart + cHeader.Length;

                ChunkType chunkType = (ChunkType)cHeader.Type;

                if (chunkType == ChunkType.LayerChunk)
                {
                    Layer layer = ReadLayerChunk(reader, header.IsLayerOpacityVailid);
                    layers.Add(layer);
                }
                else if (chunkType == ChunkType.CelChunk)
                {
                    Cel cel = ReadCelChunk(reader, chunkEnd);
                    cels.Add(cel);
                    lastCel = cel;
                }
                else if (chunkType == ChunkType.CelExtraChunk)
                {
                    RawCelExtraChunk celExtraChunk = ReadRawCelExtraChunk(reader);
                    lastCel?.SetPreciseBounds(celExtraChunk);
                }
                else if (chunkType == ChunkType.ColorProfileChunk)
                {
                    colorProfile = ReadColorProfile(reader);
                }
                else if (chunkType == ChunkType.ExternalFilesChunk)
                {
                    externalFiles = ReadExternalFiles(reader);
                }
                else if (chunkType == ChunkType.TagsChunk)
                {
                    tags = ReadTags(reader);
                    tagIterator = 0;
                    lastWithUserData = tags.First();
                }
                else if (chunkType == ChunkType.PaletteChunk)
                {
                    ReadPalette(reader, ref palette);
                }
                else if (chunkType == ChunkType.UserDataChunk)
                {
                    UserData userData = ReadUserData(reader);

                    if (lastWithUserData is not null)
                    {
                        lastWithUserData.UserData = userData;

                        //  Tags are a special case, user data for tags comes
                        //  all together (one next to the other) after the tags
                        //  chunk, in the same order:
                        //
                        //  * TAGS CHUNK (TAG1, TAG2, ..., TAGn)
                        //  * USER DATA CHUNK FOR TAG 1
                        //  * USER DATA CHUNK FOR TAG 2
                        //  * ...
                        //  * USER DATA CHUNK FOR TAGn
                        //
                        //  So here we expect that the next user data chunk
                        //  will correspond to the next tag in the tags
                        //  collection
                        tagIterator++;

                        if (tagIterator < tags.Count)
                        {
                            lastWithUserData = tags[tagIterator];
                        }
                        else
                        {
                            lastWithUserData = null;
                        }
                    }
                }
                else if (chunkType == ChunkType.OldPaletteChunkA)
                {
                    throw new NotSupportedException();
                }
                else if (chunkType == ChunkType.OldPaletteChunkB)
                {
                    throw new NotSupportedException();
                }
                else if (chunkType == ChunkType.MaskChunk)
                {
                    throw new NotSupportedException();
                }
                else if (chunkType == ChunkType.PathChunk)
                {
                    throw new NotSupportedException();
                }

            }
        }
    }


    // -------------------------------------------------------------------------
    //  Read methods
    //
    //  These are various methods that act as a bridge between reading the
    //  raw value struct parts of an Aseprite file and returning back a more
    //  strongly defined instance of those values.
    // -------------------------------------------------------------------------

    internal static Layer ReadLayerChunk(AsepriteBinaryReader reader, bool isLayerOpacityValid)
    {
        RawLayerChunk chunk = ReadRawLayerChunk(reader);

        //  Is layer opacity valid?
        if (!isLayerOpacityValid)
        {
            chunk.Opacity = 255;
        }

        LayerType ltype = (LayerType)chunk.Type;
        Layer layer;

        if (ltype == LayerType.Tilemap)
        {
            layer = new TilesetLayer(chunk);
        }
        else
        {
            layer = new Layer(chunk);
        }

        return layer;
    }

    internal static Cel ReadCelChunk(AsepriteBinaryReader reader, long end)
    {
        RawCelChunk rawCel = ReadRawCelChunk(reader, end);

        CelType celType = (CelType)rawCel.Type;

        return celType switch
        {
            //  Compressed pixel data is decompressed during the raw read
            CelType.RawImage or CelType.CompressedImage => new ImageCel(rawCel),
            CelType.Linked => new LinkedCel(rawCel),
            CelType.CompressedTilemap => new TilemapCel(rawCel),
            _ => throw new InvalidOperationException()
        };
    }

    internal static ColorProfile ReadColorProfile(AsepriteBinaryReader reader)
    {
        RawColorProfileChunk chunk = ReadRawColorProfileChunk(reader);
        return new ColorProfile(chunk);
    }

    internal static List<ExternalFile> ReadExternalFiles(AsepriteBinaryReader reader)
    {
        RawExternalFilesChunk chunk = ReadRawExternalFilesChunk(reader);

        List<ExternalFile> files = new(chunk.Entries.Length);

        for (int i = 0; i < chunk.Entries.Length; i++)
        {
            RawExternalFileChunkEntry entryChunk = chunk.Entries[i];

            ExternalFile file = new(entryChunk);
            files.Add(file);
        }

        return files;
    }

    internal static List<Tag> ReadTags(AsepriteBinaryReader reader)
    {
        RawTagsChunk chunk = ReadRawTagsChunk(reader);

        List<Tag> tags = new(chunk.Tags.Length);

        for (int i = 0; i < chunk.Tags.Length; i++)
        {
            RawTagsChunkTag tagChunk = chunk.Tags[i];

            Tag tag = new(tagChunk);
            tags.Add(tag);
        }

        return tags;
    }

    internal static void ReadPalette(AsepriteBinaryReader reader, ref Color[] palette)
    {
        RawPaletteChunk chunk = ReadRawPaletteChunk(reader);

        if (chunk.NewPaletteSize > 0)
        {
            //  Need to resize the palette array
            Color[] newPalette = new Color[chunk.NewPaletteSize];
            Array.Copy(palette, newPalette, palette.Length);
            palette = newPalette;
        }

        for (uint i = chunk.From; i <= chunk.To; i++)
        {
            RawPaletteChunkEntry entry = chunk.Entries[i];
            Color color = Color.FromArgb(entry.Alpha, entry.Red, entry.Blue, entry.Green);
            palette[i] = color;
        }
    }

    internal static UserData ReadUserData(AsepriteBinaryReader reader)
    {
        RawUserDataChunk chunk = ReadRawUserDataChunk(reader);
        return new UserData(chunk);
    }

    // -------------------------------------------------------------------------
    //  ReadRaw methods
    //
    //  These are various methods that read the raw value struct of a part of
    //  an aseprite file and returns it back.  Each method will perform a
    //  validation call after all values for it are read, which can thrown an
    //  exception if the values are found to not be as expected.
    // -------------------------------------------------------------------------

    private static RawHeader ReadRawHeader(AsepriteBinaryReader reader)
    {
        RawHeader header;

        //  Begin reading
        header.FileSize = reader.ReadDword();           //  Filesize, in bytes
        header.MagicNumber = reader.ReadWord();         //  Magic Number, should always be 0xA5E0
        header.Frames = reader.ReadWord();              //  Total number of frames
        header.Width = reader.ReadWord();               //  Width, in pixels, of canvas
        header.Height = reader.ReadWord();              //  Height, in pixels, of canvs
        header.ColorDepth = reader.ReadWord();          //  Color Depth (bits-per-pixel)
        header.Flags = reader.ReadDword();              //  Header flags
        header.Speed = reader.ReadWord();               //  Speed (ms betwene frames) (deprecated)
        header.Ignore1 = reader.ReadDword();            //  Set to 0
        header.Ignore2 = reader.ReadDword();            //  Set to 0
        header.TransparentIndex = reader.ReadByte();    //  Palette entry of transparent color (Indexed only)
        header.Ignore3 = reader.ReadBytes(3);           //  Ignore these bytes
        header.NumberOfColors = reader.ReadWord();      //  Number of colors (0 = 256 for old sprites)
        header.PixelWidth = reader.ReadByte();          //  Pixel Width
        header.PixelHeight = reader.ReadByte();         //  Pixel Height
        header.GridX = reader.ReadShort();              //  Grid top-left x-coordiante
        header.GridY = reader.ReadShort();              //  Grid top-left y-coordiante
        header.GridWidth = reader.ReadWord();           //  Grid width, in pixels
        header.GridHeight = reader.ReadWord();          //  Grid height, in pixels
        header.Ignore4 = reader.ReadBytes(84);          //  For future (set to zero)

        ThrowIfInvalid(header);

        return header;
    }

    internal static RawFrameHeader ReadRawFrameHeader(AsepriteBinaryReader reader)
    {
        RawFrameHeader header;

        //  Begin reading
        header.Length = reader.ReadDword();     //  Bytes in frame
        header.Magic = reader.ReadWord();       //  Magic number, should always be 0xF1FA
        header.OldCount = reader.ReadWord();    //  Old field which specifies number of "chunks"
        header.Duration = reader.ReadWord();    //  Frame duration, in milliseconds
        header.Ignore1 = reader.ReadBytes(2);   //  For future (set to zero)
        header.NewCount = reader.ReadDword();   //  New field which specifies number of "chunks"

        ThrowIfInvalid(header);

        return header;
    }

    internal static RawChunkHeader ReadRawChunkHeader(AsepriteBinaryReader reader)
    {
        RawChunkHeader header;

        //  Begin reading
        header.Length = reader.ReadDword();     //  Size of chunk, in bytes
        header.Type = reader.ReadWord();        //  The type of chunk

        ThrowIfInvalid(header);

        return header;
    }

    internal static RawLayerChunk ReadRawLayerChunk(AsepriteBinaryReader reader)
    {
        RawLayerChunk chunk;

        //  Default all optiona values
        chunk.TilsetIndex = default;

        //  Begin reading
        chunk.Flags = reader.ReadWord();                //  Layer Flags
        chunk.Type = reader.ReadWord();                 //  Layer Type
        chunk.ChildLevel = reader.ReadWord();           //  Layer child level
        chunk.DefaultWidth = reader.ReadWord();         //  Default width, in pixles (ignored)
        chunk.DefaultHeight = reader.ReadWord();        //  Default height, in pixels (ignore)
        chunk.BlendMode = reader.ReadWord();            //  Blend mode
        chunk.Opacity = reader.ReadByte();              //  Layer opacity (only valid if header flag set)
        chunk.Ignore = reader.ReadBytes(3);             //  For Furture (set to zero)
        chunk.Name = reader.ReadString();               //  Layer name

        //  Read optional values basd on layer type
        if (chunk.Type == 2)
        {
            chunk.TilsetIndex = reader.ReadDword();     //  Tileset index
        }

        ThrowIfInvalid(chunk);

        return chunk;
    }

    internal static RawCelChunk ReadRawCelChunk(AsepriteBinaryReader reader, long end)
    {
        RawCelChunk chunk;

        //  Default all optional values
        chunk.Width = default;
        chunk.Height = default;
        chunk.Pixels = default;
        chunk.FramePosition = default;
        chunk.Pixels = default;
        chunk.BitsPerTile = default;
        chunk.TileIdBitmask = default;
        chunk.XFlipBitmask = default;
        chunk.YFlipBitmask = default;
        chunk.RotationBitmask = default;
        chunk.Ignore2 = default;
        chunk.Tiles = default;

        //  Begin reading
        chunk.LayerIndex = reader.ReadWord();   //  Index of layer this cel is on
        chunk.X = reader.ReadShort();           //  top-left x-coordinate of cel relative to canvas bounds
        chunk.Y = reader.ReadShort();           //  top-left y-coordiante of cel relative to canvas bounds
        chunk.Opacity = reader.ReadByte();      //  Opacity of cel
        chunk.Type = reader.ReadWord();         //  Cel type
        chunk.Ignore = reader.ReadBytes(7);     //  For future (set to zero)


        //  Read optional values bsed on cel type
        if (chunk.Type == 0)
        {
            //  Raw image cel
            chunk.Width = reader.ReadWord();            //  Width, in pixels
            chunk.Height = reader.ReadWord();           //  Height, in pixels
            chunk.Pixels = reader.ReadToPosition(end);  //  Uncompressed raw pixel data
        }
        else if (chunk.Type == 1)
        {
            //  Linked cell
            chunk.FramePosition = reader.ReadWord();    //  Frame position to link with
        }
        else if (chunk.Type == 2)
        {
            //  Compressed image cel
            chunk.Width = reader.ReadWord();                //  Width, in pixels
            chunk.Height = reader.ReadWord();               //  Height, in pixels
            byte[] compressed = reader.ReadToPosition(end); //  Compressed pixel data

            chunk.Pixels = Zlib.Deflate(compressed);
        }
        else if (chunk.Type == 3)
        {
            chunk.Width = reader.ReadWord();                //  Width, in number of tiles
            chunk.Height = reader.ReadWord();               //  Height, in number of tiles
            chunk.BitsPerTile = reader.ReadWord();          //  Bits per tile
            chunk.TileIdBitmask = reader.ReadDword();       //  Bitmask for tile id
            chunk.XFlipBitmask = reader.ReadDword();        //  Bitmask for X flip
            chunk.YFlipBitmask = reader.ReadDword();        //  Bitmask for Y flip
            chunk.RotationBitmask = reader.ReadDword();     //  Bitmask for 90cw rotation
            chunk.Ignore2 = reader.ReadBytes(10);           //  Reserved
            byte[] compressed = reader.ReadToPosition(end); //  Compressed tile data

            chunk.Tiles = Zlib.Deflate(compressed);
        }

        ThrowIfInvalid(chunk);

        return chunk;
    }

    internal static RawCelExtraChunk ReadRawCelExtraChunk(AsepriteBinaryReader reader)
    {
        RawCelExtraChunk chunk;

        //  Begin reading
        chunk.Flags = reader.ReadDword();           //  Flags (1 = Precise Bounds are set) (set to zero????)
        chunk.PreciseX = reader.ReadFixed();        //  Precise X position
        chunk.PreciseY = reader.ReadFixed();        //  Precise Y position
        chunk.PreciseWidth = reader.ReadFixed();    //  Width of the cel in the sprite (scaled in real-time)
        chunk.PreciseHeight = reader.ReadFixed();   //  Height of the cel in the sprite
        chunk.Ignore = reader.ReadBytes(16);        //  For future use (set to zero)

        ThrowIfInvalid(chunk);

        return chunk;


    }

    internal static RawColorProfileChunk ReadRawColorProfileChunk(AsepriteBinaryReader reader)
    {
        RawColorProfileChunk chunk;

        //  Default all optional values
        chunk.ICCProfileDataLength = default;
        chunk.ICCProfileData = default;

        //  Begin reading
        chunk.Type = reader.ReadWord();         //  Color profile type
        chunk.Flags = reader.ReadWord();        //  Flags 
        chunk.FixedGamma = reader.ReadFixed();  //  Fixed gamma (1.0 = linear)
        chunk.Ignore = reader.ReadBytes(8);     //  Reserved (set to 0)

        //  Read optional values based on type
        if (chunk.Type == 2)
        {
            chunk.ICCProfileDataLength = reader.ReadDword();
            chunk.ICCProfileData = reader.ReadBytes((int)chunk.ICCProfileDataLength);
        }

        ThrowIfInvalid(chunk);

        return chunk;
    }

    internal static RawExternalFilesChunk ReadRawExternalFilesChunk(AsepriteBinaryReader reader)
    {
        RawExternalFilesChunk chunk;

        //  Begin reading
        chunk.NumberOfEntries = reader.ReadDword(); //  Number of Entries
        chunk.Ignore = reader.ReadBytes(8);         //  Reserved (set to zero)

        //  Initialize the entries array
        chunk.Entries = new RawExternalFileChunkEntry[chunk.NumberOfEntries];

        //  Read each entry
        for (uint i = 0; i < chunk.NumberOfEntries; i++)
        {
            RawExternalFileChunkEntry entryChunk;
            entryChunk.EntryId = reader.ReadDword();            //  Entry ID (this ID is referenced by tilesets or palettes)
            entryChunk.Ignore = reader.ReadBytes(8);            //  Reserved (set to zero)
            entryChunk.ExternalFileName = reader.ReadString();  //  External file name

            chunk.Entries[i] = entryChunk;
        }

        return chunk;
    }

    internal static RawTagsChunk ReadRawTagsChunk(AsepriteBinaryReader reader)
    {
        RawTagsChunk chunk;

        //  Begin reading
        chunk.NumberOfTags = reader.ReadWord();     //  Number of tags
        chunk.Ignore = reader.ReadBytes(8);         //  For future (set to zero)

        //  Initialize the tags array
        chunk.Tags = new RawTagsChunkTag[chunk.NumberOfTags];

        //  Read each tag
        for (ushort i = 0; i < chunk.NumberOfTags; i++)
        {
            RawTagsChunkTag tagChunk;
            tagChunk.From = reader.ReadWord();              //  From frame
            tagChunk.To = reader.ReadWord();                //  To frame
            tagChunk.LoopDirection = reader.ReadByte();     //  Loop animation direction
            tagChunk.Ignore1 = reader.ReadBytes(8);         //  For future (set to zero)
            tagChunk.Color = reader.ReadBytes(3);           //  RGB Values of the tag color
            tagChunk.Ignore2 = reader.ReadByte();           //  Extra byte (zero)
            tagChunk.Name = reader.ReadString();            //  Tag name

            ThrowIfInvalid(tagChunk);

            chunk.Tags[i] = tagChunk;
        }

        return chunk;
    }

    internal static RawPaletteChunk ReadRawPaletteChunk(AsepriteBinaryReader reader)
    {
        RawPaletteChunk chunk;

        //  Begin reading
        chunk.NewPaletteSize = reader.ReadDword();
        chunk.From = reader.ReadDword();
        chunk.To = reader.ReadDword();
        chunk.Ignore = reader.ReadBytes(8);

        //  Number of entries to read
        int nEntries = (int)chunk.To - (int)chunk.From + 1;

        //  Initialize the entry array
        chunk.Entries = new RawPaletteChunkEntry[nEntries];

        //  Read each entry
        for (int i = 0; i < nEntries; i++)
        {
            RawPaletteChunkEntry entryChunk;

            //  Default optional values
            entryChunk.Name = default;

            //  Begin reading
            entryChunk.Flags = reader.ReadWord();       //  Entry flags
            entryChunk.Red = reader.ReadByte();         //  Red (0 - 255)
            entryChunk.Green = reader.ReadByte();       //  Green (0 - 255)
            entryChunk.Blue = reader.ReadByte();        //  Blue (0 - 255)
            entryChunk.Alpha = reader.ReadByte();       //  Alpha (0 - 255)

            //  Read optional values based on flags
            if (entryChunk.Flags == 1)
            {
                entryChunk.Name = reader.ReadString();  //  Color name
            }

            chunk.Entries[i] = entryChunk;
        }

        return chunk;
    }

    internal static RawUserDataChunk ReadRawUserDataChunk(AsepriteBinaryReader reader)
    {
        RawUserDataChunk chunk;

        //  Default optional values
        chunk.Text = default;
        chunk.Red = default;
        chunk.Green = default;
        chunk.Blue = default;
        chunk.Alpha = default;

        //  Begin reading
        chunk.Flags = reader.ReadDword();   //  Flags

        //  If flags have bit 1 set (has text)
        if ((chunk.Flags & 1) != 0)
        {
            chunk.Text = reader.ReadString();   //  Text
        }

        //  If flags have bit 2 set (has color)
        if ((chunk.Flags & 2) != 0)
        {
            chunk.Red = reader.ReadByte();      //  Color Red (0 - 255)
            chunk.Green = reader.ReadByte();    //  Color Green (0 - 255)
            chunk.Blue = reader.ReadByte();     //  Color Blue (0 - 255)
            chunk.Alpha = reader.ReadByte();    //  Color Alpha (0 - 255)
        }

        return chunk;
    }

    // -------------------------------------------------------------------------
    //  ThrowIfValid methods
    //
    //  These are various overload that each take in a different native
    //  struct and performs validation on the values within the struct to ensure
    //  they are as expected.  Each method will throw an exception if the struct
    //  values are not as expected.
    // -------------------------------------------------------------------------

    private static void ThrowIfInvalid(RawHeader header)
    {
        //  Validate magic number
        if (header.MagicNumber != 0xA5E0)
        {
            throw new InvalidOperationException();
        }

        //  Validate the width and height
        if (header.Width == 0 || header.Height == 0)
        {
            throw new InvalidOperationException();
        }

        //  Validate the color depth
        if (!Enum.IsDefined<ColorDepth>((ColorDepth)header.ColorDepth))
        {
            throw new InvalidOperationException();
        }
    }

    private static void ThrowIfInvalid(RawFrameHeader header)
    {
        //  Validate magic numbeer
        if (header.Magic != 0xF1FA)
        {
            throw new InvalidOperationException();
        }
    }

    private static void ThrowIfInvalid(RawChunkHeader header)
    {
        //  Validate the chunk type
        if (!Enum.IsDefined<ChunkType>((ChunkType)header.Type))
        {
            throw new InvalidOperationException();
        }
    }

    private static void ThrowIfInvalid(RawLayerChunk chunk)
    {
        //  Validate flags
        if ((chunk.Flags & (int)LayerFlags.All) != chunk.Flags)
        {
            throw new InvalidOperationException();
        }

        //  Validate layer type value
        if (!Enum.IsDefined<LayerType>((LayerType)chunk.Type))
        {
            throw new InvalidOperationException();
        }

        //  Validate blendmode value
        if (!Enum.IsDefined<BlendMode>((BlendMode)chunk.BlendMode))
        {
            throw new InvalidOperationException();
        }

        //  Validate that tileset index is provided if layer type == 2
        if (chunk.Type == 2 && chunk.TilsetIndex is null)
        {
            throw new InvalidOperationException();
        }
    }

    private static void ThrowIfInvalid(RawCelChunk chunk)
    {
        //  validate cel type
        if (!Enum.IsDefined<CelType>((CelType)chunk.Type))
        {
            throw new InvalidOperationException();
        }

        //  Not linked cels
        if (chunk.Type != 1)
        {
            //  Validate width and height
            if (chunk.Width is null || chunk.Width <= 0 || chunk.Height is null || chunk.Height <= 0)
            {
                throw new InvalidOperationException();
            }
        }
    }

    private static void ThrowIfInvalid(RawCelExtraChunk chunk)
    {
        //  Validate flags
        if (chunk.Flags != 1)
        {
            throw new InvalidOperationException();
        }
    }

    private static void ThrowIfInvalid(RawColorProfileChunk chunk)
    {
        if (!Enum.IsDefined<ColorProfileType>((ColorProfileType)chunk.Type))
        {
            throw new InvalidOperationException();
        }

        if (chunk.Flags != 1)
        {
            throw new InvalidOperationException();
        }

        //  ICC type validation
        if (chunk.Type == 2)
        {
            if (chunk.ICCProfileDataLength is null)
            {
                throw new InvalidOperationException();
            }

            if (chunk.ICCProfileData is null)
            {
                throw new InvalidOperationException();
            }
        }
    }

    private static void ThrowIfInvalid(RawTagsChunkTag chunk)
    {
        //  Validate loop direction
        if (!Enum.IsDefined<LoopDirection>((LoopDirection)chunk.LoopDirection))
        {
            throw new InvalidOperationException();
        }
    }

    // internal static RawFrameHeader ReadRawFrameHeader(AsepriteBinaryReader reader)
    // {
    //     RawFrameHeader raw;
    //     raw.Length = reader.ReadDword();
    //     raw.Magic = reader.ReadWord();
    //     raw.OldCount = reader.ReadWord();
    //     raw.Duration = reader.ReadWord();
    //     raw.Ignore1 = reader.ReadBytes(2);
    //     raw.NewCount = reader.ReadDword();

    //     return raw;
    // }

    // internal static Chunk ReadChunk(AsepriteBinaryReader reader)
    // {
    //     long start = reader.Position;

    //     RawChunkHeader rawHeader = ReadRawChunkHeader(reader);

    //     //  Chunk end position needs to be calcualted for eading some chunks
    //     //  like the cel chunk.
    //     long end = start + rawHeader.Length;

    //     ChunkType chunkType = (ChunkType)rawHeader.Type;

    //     return chunkType switch
    //     {
    //         ChunkType.OldPaletteChunkA => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
    //         ChunkType.OldPaletteChunkB => throw new NotSupportedException($"Old Palette Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
    //         ChunkType.LayerChunk => ReadLayerChunk(reader, rawHeader),
    //         ChunkType.CelChunk => ReadCelChunk(reader, rawHeader, end),
    //         ChunkType.CelExtraChunk => ReadCelExtraChunk(reader, rawHeader),
    //         ChunkType.ColorProfileChunk => ReadColorProfileChunk(reader, rawHeader),
    //         ChunkType.ExternalFilesChunk => ReadExternalFilesChunk(reader, rawHeader),
    //         ChunkType.MaskChunk => throw new NotSupportedException($"Mask Chunk (0x{rawHeader.Type:X4}) detected.  The version of Aseprite used to create this file is not supported"),
    //         ChunkType.PathChunk => throw new NotSupportedException($"Path Chunk (0x{rawHeader.Type:X2}) detected.  The version of Aseprite used to create this file is not supported"),
    //         ChunkType.TagsChunk => ReadTagsChunk(reader, rawHeader),
    //         ChunkType.PaletteChunk => ReadPaletteChunk(reader, rawHeader),
    //         _ => throw new InvalidOperationException($"Unknown chunk type (0x{rawHeader.Type:X4})")
    //     };

    // }

    // internal static RawChunkHeader ReadRawChunkHeader(AsepriteBinaryReader reader)
    // {
    //     RawChunkHeader raw;
    //     raw.Length = reader.ReadDword();
    //     raw.Type = reader.ReadWord();

    //     return raw;
    // }

    // private static Layer ReadLayerChunk(AsepriteBinaryReader reader)
    // {
    //     RawLayerChunk raw = ReadRawLayerChunk(reader);

    //     return raw.Type switch
    //     {
    //         0 or 1 => new Layer(raw),
    //         2 => new TilesetLayer(raw),
    //         _ => throw new InvalidOperationException($"Unknown Layer Type '{raw.Type}'")
    //     };
    // }

    // private static RawLayerChunk ReadRawLayerChunk(AsepriteBinaryReader reader)
    // {
    //     RawLayerChunk raw;
    //     raw.Flags = reader.ReadWord();
    //     raw.Type = reader.ReadWord();
    //     raw.ChildLevel = reader.ReadWord();
    //     raw.DefaultWidth = reader.ReadWord();
    //     raw.DefaultHeight = reader.ReadWord();
    //     raw.BlendMode = reader.ReadWord();
    //     raw.Opacity = reader.ReadByte();
    //     raw.Ignore = reader.ReadBytes(3);
    //     raw.Name = reader.ReadString();
    //     raw.TilsetIndex = default;

    //     if (raw.Type == 2)
    //     {
    //         raw.TilsetIndex = reader.ReadDword();
    //     }

    //     return raw;
    // }

    // private static Cel ReadCelChunk(AsepriteBinaryReader reader, long end)
    // {
    //     RawCelChunk raw = ReadRawCelChunk(reader, end);

    //     return raw.Type switch
    //     {
    //         0 or 2 => new ImageCel(raw),
    //         1 => new LinkedCel(raw),
    //         3 => new TilemapCel(raw),
    //         _ => throw new InvalidOperationException($"Unknown Cel Type '{raw.Type}'")
    //     };
    // }

    // private static RawCelChunk ReadRawCelChunk(AsepriteBinaryReader reader, long end)
    // {
    //     RawCelChunk raw;
    //     raw.LayerIndex = reader.ReadWord();
    //     raw.X = reader.ReadShort();
    //     raw.Y = reader.ReadShort();
    //     raw.Opacity = reader.ReadByte();
    //     raw.Type = reader.ReadWord();
    //     raw.Ignroe = reader.ReadBytes(7);

    //     raw.Width = default;
    //     raw.Height = default;
    //     raw.Pixels = default;
    //     raw.FramePosition = default;
    //     raw.CompressedPixels = default;
    //     raw.BitsPerTile = default;
    //     raw.TileIdBitmask = default;
    //     raw.XFlipBitmask = default;
    //     raw.YFlipBitmask = default;
    //     raw.RotationBitmask = default;
    //     raw.Ignore2 = default;
    //     raw.CompressedTiles = default;

    //     if (raw.Type == 0)
    //     {
    //         raw.Width = reader.ReadWord();
    //         raw.Height = reader.ReadWord();
    //         raw.Pixels = reader.ReadBytes((int)(end - reader.Position));
    //     }
    //     else if (raw.Type == 1)
    //     {
    //         raw.FramePosition = reader.ReadWord();
    //     }
    //     else if (raw.Type == 2)
    //     {
    //         raw.Width = reader.ReadWord();
    //         raw.Height = reader.ReadWord();
    //         raw.CompressedPixels = reader.ReadBytes((int)(end - reader.Position));
    //     }
    //     else if (raw.Type == 3)
    //     {
    //         raw.Width = reader.ReadWord();
    //         raw.Height = reader.ReadWord();
    //         raw.BitsPerTile = reader.ReadWord();
    //         raw.TileIdBitmask = reader.ReadDword();
    //         raw.XFlipBitmask = reader.ReadDword();
    //         raw.YFlipBitmask = reader.ReadDword();
    //         raw.RotationBitmask = reader.ReadDword();
    //         raw.Ignore2 = reader.ReadBytes(10);
    //         raw.CompressedTiles = reader.ReadBytes((int)(end - reader.Position));
    //     }

    //     return raw;
    // }

    // private static CelExtra ReadCelExtraChunk(AsepriteBinaryReader reader)
    // {
    //     RawCelExtraChunk raw = ReadRawCelExtraChunk(reader);
    //     return new CelExtra(raw);
    // }

    // private static RawCelExtraChunk ReadRawCelExtraChunk(AsepriteBinaryReader reader)
    // {
    //     RawCelExtraChunk raw;
    //     raw.Flags = reader.ReadDword();
    //     raw.PreciseX = reader.ReadFixed();
    //     raw.PreciseY = reader.ReadFixed();
    //     raw.PreciseWidth = reader.ReadFixed();
    //     raw.PreciseHeight = reader.ReadFixed();
    //     raw.Ignore = reader.ReadBytes(16);

    //     return raw;
    // }

    // private static ColorProfile ReadColorProfileChunk(AsepriteBinaryReader reader)
    // {
    //     RawColorProfileChunk raw = ReadRawColorProfileChunk(reader);
    //     return new ColorProfile(raw);
    // }

    // private static RawColorProfileChunk ReadRawColorProfileChunk(AsepriteBinaryReader reader)
    // {
    //     RawColorProfileChunk raw;
    //     raw.Type = reader.ReadWord();
    //     raw.Flags = reader.ReadWord();
    //     raw.FixedGamma = reader.ReadFixed();
    //     raw.Ignore = reader.ReadBytes(8);
    //     raw.ICCProfileDataLength = default;
    //     raw.ICCProfileData = default;

    //     if (raw.Type == 2)
    //     {
    //         raw.ICCProfileDataLength = reader.ReadDword();
    //         raw.ICCProfileData = reader.ReadBytes((int)raw.ICCProfileDataLength);
    //     }

    //     return raw;
    // }

    // private static AseExternalFilesChunk ReadExternalFilesChunk(AsepriteBinaryReader reader)
    // {
    //     ExternalFilesChunk raw = ReadRawExternalFilesChunk(reader);
    //     return new AseExternalFilesChunk(raw);
    // }

    // private static ExternalFilesChunk ReadRawExternalFilesChunk(AsepriteBinaryReader reader)
    // {
    //     ExternalFilesChunk raw;
    //     raw.NumberOfEntries = reader.ReadDword();
    //     raw.Ignore = reader.ReadBytes(8);

    //     raw.Entries = new RawExternalFileChunkEntry[raw.NumberOfEntries];

    //     for (int i = 0; i < raw.NumberOfEntries; i++)
    //     {
    //         RawExternalFileChunkEntry entry = ReadRawExternalFilesChunkEntry(reader);
    //         raw.Entries[i] = entry;
    //     }

    //     return raw;
    // }

    // private static RawExternalFileChunkEntry ReadRawExternalFilesChunkEntry(AsepriteBinaryReader reader)
    // {
    //     RawExternalFileChunkEntry raw;
    //     raw.EntryId = reader.ReadDword();
    //     raw.Ignore = reader.ReadBytes(8);
    //     raw.ExternalFileName = reader.ReadString();

    //     return raw;
    // }

    // private static AseTagsChunk ReadTagsChunk(AsepriteBinaryReader reader, RawChunkHeader header)
    // {
    //     RawTagsChunk raw = ReadRawTagsChunk(reader);
    //     return new AseTagsChunk(raw);
    // }

    // private static RawTagsChunk ReadRawTagsChunk(AsepriteBinaryReader reader)
    // {
    //     RawTagsChunk raw;
    //     raw.NumberOfTags = reader.ReadWord();
    //     raw.Ignore = reader.ReadBytes(8);

    //     raw.Tags = new RawTagsChunkTag[raw.NumberOfTags];

    //     for (int i = 0; i < raw.Tags.Length; i++)
    //     {
    //         RawTagsChunkTag rawTag = ReadRawTagsChunkTag(reader);
    //         raw.Tags[i] = rawTag;
    //     }

    //     return raw;
    // }

    // private static RawTagsChunkTag ReadRawTagsChunkTag(AsepriteBinaryReader reader)
    // {
    //     RawTagsChunkTag raw;
    //     raw.From = reader.ReadWord();
    //     raw.To = reader.ReadWord();
    //     raw.LoopDirection = reader.ReadByte();
    //     raw.Ignore1 = reader.ReadBytes(8);
    //     raw.Color = reader.ReadBytes(3);
    //     raw.Ignore2 = reader.ReadByte();
    //     raw.Name = reader.ReadString();

    //     return raw;
    // }

    // private static RawPaletteChunk ReadRawPaletteChunk(AsepriteBinaryReader reader)
    // {
    //     RawPaletteChunk raw;
    //     raw.NewPaletteSize = reader.ReadDword();
    //     raw.From = reader.ReadDword();
    //     raw.To = reader.ReadDword();
    //     raw.Ignore = reader.ReadBytes(8);

    //     raw.Entries = new RawPaletteChunkEntry

    //     for (uint i = raw.From; i <= raw.To; i++)
    //     {

    //     }
    // }
}