// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet;
using AsepriteDotNet.Core;
using AsepriteDotNet.Core.IO;
using AsepriteDotNet.Processors;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// Load the file using the AsepriteFileLoader.  In this example, we are passing the path to the file.  There is also
/// an overload where you can pass a stream instead if you needed.
///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
AsepriteFile aseFile = AsepriteFileLoader.FromFile("adventurer.aseprite");


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// The SpriteProcessor is used to process a Sprite from a single frame
///
/// The onlyVisibleLayers, includeBackgroundLayer, and includeTilemapLayers parameters are optional. Below shows their
/// default values if not specified.
///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Sprite sprite = SpriteProcessor.Process(aseFile,
                                        0,
                                        onlyVisibleLayers: true,
                                        includeBackgroundLayer: false,
                                        includeTilemapLayers: false);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// The TextureAtlas processor will process a TextureAtlas from the aseprite file.  A TextureAtlas contains single
/// image representation of all frames in the Aseprite file that have been box packed into the pixel data.  Additional
/// TextureRegion properties are provided that define the source rectangle for each frame within the base texture
/// representation.
///
/// The onlyVisibleLayers, includeBackgroundLayer, includeTileMapLayers, mergeDuplicateFrames, borderPadding, spacing,
/// and innerPadding parameters are optional.  Below shows their default values when not specified.
///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
TextureAtlas atlas = TextureAtlasProcessor.Process(aseFile,
                                                   onlyVisibleLayers: true,
                                                   includeBackgroundLayer: false,
                                                   includeTilemapLayers: false,
                                                   mergeDuplicateFrames: true,
                                                   borderPadding: 0,
                                                   spacing: 0,
                                                   innerPadding: 0);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// The SpriteSheet processor will process a SpriteSheet from the aseprite file.  A SpriteSheet uses a TextureAtlas as
/// its source for the image and provides additional properties that define animations as defined by the tags in
/// Aseprite.
///
/// The onlyVisibleLayers, includeBackgroundLayer, includeTileMapLayers, mergeDuplicateFrames, borderPadding, spacing,
/// and innerPadding parameters are optional.  Below shows their default values when not specified.
///
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
SpriteSheet spriteSheet = SpriteSheetProcessor.Process(aseFile,
                                                       onlyVisibleLayers: true,
                                                       includeBackgroundLayer: false,
                                                       includeTilemapLayers: false,
                                                       mergeDuplicateFrames: true,
                                                       borderPadding: 0,
                                                       spacing: 0,
                                                       innerPadding: 0);
