// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite;
using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;
using AsepriteDotNet.IO;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// Load the file using the AsepriteFileLoader.  In this example, we are passing the path to the file.  There is also
/// an overload where you can pass a stream instead if you needed.
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
AsepriteFile aseFile = AsepriteFileLoader.FromFile("adventurer.aseprite");

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///
/// Flatten a single frame to get the pixel representation of that frame.
/// 
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Rgba32[] frame0Pixels = aseFile.Frames[0].FlattenFrame(onlyVisibleLayers: true, includeBackgroundLayer: false, includeTilemapCels: true);


//  At this point you can use the pixel data of that frame in your application ever how you need it.  
