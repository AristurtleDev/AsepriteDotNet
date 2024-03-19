// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

internal static class ProcessorUtilities
{
    internal static Slice[] GetSlicesForFrame(int frameIndex, ReadOnlySpan<AsepriteSlice> aseSlices)
    {
        List<Slice> slices = new List<Slice>();
        HashSet<string> sliceNameCheck = new HashSet<string>();

        for (int s = 0; s < aseSlices.Length; s++)
        {
            AsepriteSlice aseSlice = aseSlices[s];
            ReadOnlySpan<AsepriteSliceKey> aseSliceKeys = aseSlice.Keys;

            //  Traverse keys backwards until we find a match for the frame index
            for (int k = aseSliceKeys.Length - 1; k >= 0; k--)
            {
                AsepriteSliceKey aseSliceKey = aseSliceKeys[k];

                if (aseSliceKey.FrameIndex > frameIndex) { continue; }

                string name = aseSlice.Name;
                if (!sliceNameCheck.Add(name))
                {
                    throw new InvalidOperationException($"Duplicate slice name '{name}' found.  Slices must have unique names");
                }

                Rectangle bounds = aseSliceKey.Bounds;
                Rgba32 color = aseSlice.UserData.Color.GetValueOrDefault();
                Point origin = aseSliceKey.Pivot;

                Slice slice = aseSlice.IsNinePatch
                    ? new NinePatchSlice(name, bounds, origin, color, aseSliceKey.CenterBounds)
                    : new Slice(name, bounds, origin, color);
                slices.Add(slice);
                break;
            }
        }

        return [.. slices];
    }
}
