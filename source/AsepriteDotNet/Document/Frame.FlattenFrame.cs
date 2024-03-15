//// Copyright (c) Christopher Whitley. All rights reserved.
//// Licensed under the MIT license.
//// See LICENSE file in the project root for full license information.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;
//using AsepriteDotNet.Helpers;

//namespace AsepriteDotNet.Document;

//public static class FrameExtensions
//{
//    public static AseColor[] FlattenFrame(this Frame frame, bool onlyVisibleLayers)
//    {
//        return frame.FlattenFrame<AseColor>(onlyVisibleLayers, (color) => color);
//    }

//    public static T[] FlattenFrame<T>(this Frame frame, bool onlyVisibleLayers, Func<AseColor, T> colorProcessor) where T : struct
//    {
//        AseColor[] result = new AseColor[frame.Width * frame.Height];
//        ReadOnlySpan<Cel> cels = frame.Cels;

//        for(int celNum = 0; celNum < cels.Length; celNum++)
//        {
//            Cel cel = cels[celNum];
//            if(cel is LinkedCel linkedCel)
//            {
//                cel = linkedCel.Cel;
//            }

//            if (cel is not ImageCel imageCel) { continue; }
//            if (onlyVisibleLayers && !imageCel.Layer.IsVisible) { continue; }

//            ReadOnlySpan<AseColor> pixels = imageCel.Pixels;
//            byte opacity = Usigned8Bit.Multiply(imageCel.Opacity, imageCel.Layer.Opacity);

//            for(int pixelNum = 0; pixelNum < pixels.Length; pixelNum++)
//            {
//                int x = (pixelNum % imageCel.Width) + imageCel.X;
//                int y = (pixelNum / imageCel.Width) + imageCel.Y;
//                int index = y * frame.Width + x;

//                //  Sometimes a cel can have a negative x and/or y value.  This is caused by selecting an area within
//                //  aseprite and then moving a portion of the selected pixels outside the canvas.  We don't care about
//                //  these pixels, so if the index is outside the range of the array to store them in, we'll just
//                //  discard them
//                if (index < 0 || index > result.Length) { continue; }

//                AseColor background = result[index];
//                AseColor source = pixels[pixelNum];
//                AseColor blended = 
//            }
//        }
//    }
//}

