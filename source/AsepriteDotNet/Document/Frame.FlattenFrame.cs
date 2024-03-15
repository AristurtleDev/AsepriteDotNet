// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Helpers;

namespace AsepriteDotNet.Document;

public static class FrameExtensions
{
    public static AseColor[] FlattenFrame(this Frame frame, bool onlyVisibleLayers)
    {
        return frame.FlattenFrame<AseColor>(onlyVisibleLayers, (color) => color);
    }

    public static T[] FlattenFrame<T>(this Frame frame, bool onlyVisibleLayers, Func<AseColor, T> colorProcessor) where T : struct
    {
        ArgumentNullException.ThrowIfNull(frame);
        ArgumentNullException.ThrowIfNull(colorProcessor);

        AseColor[] flattened = new AseColor[frame.Width * frame.Height];
        ReadOnlySpan<Cel> cels = frame.Cels;

        for (int celNum = 0; celNum < cels.Length; celNum++)
        {
            Cel cel = cels[celNum];
            if (cel is LinkedCel linkedCel)
            {
                cel = linkedCel.Cel;
            }

            if (cel is not ImageCel imageCel) { continue; }
            if (onlyVisibleLayers && !imageCel.Layer.IsVisible) { continue; }

            ReadOnlySpan<AseColor> pixels = imageCel.Pixels;
            byte opacity = imageCel.Opacity.MUL_UN8(imageCel.Layer.Opacity);

            for (int pixelNum = 0; pixelNum < pixels.Length; pixelNum++)
            {
                int x = (pixelNum % imageCel.Width) + imageCel.X;
                int y = (pixelNum / imageCel.Width) + imageCel.Y;
                int index = y * frame.Width + x;

                //  Sometimes a cel can have a negative x and/or y value.  This is caused by selecting an area within
                //  aseprite and then moving a portion of the selected pixels outside the canvas.  We don't care about
                //  these pixels, so if the index is outside the range of the array to store them in, we'll just
                //  discard them
                if (index < 0 || index > flattened.Length) { continue; }

                AseColor backdrop = flattened[index];
                AseColor source = pixels[pixelNum];
                flattened[index] = backdrop.Blend(source, opacity, imageCel.Layer.BlendMode);
            }
        }

        T[] result = new T[flattened.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = colorProcessor(flattened[i]);
        }

        return result;
    }
}

