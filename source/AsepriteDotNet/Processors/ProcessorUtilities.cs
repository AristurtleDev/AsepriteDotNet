// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Aseprite.Types;
using AsepriteDotNet.Common;

namespace AsepriteDotNet.Processors;

internal static class ProcessorUtilities
{
    internal static Slice<T>[] GetSlicesForFrame<T>(int frameIndex, ReadOnlySpan<AsepriteSlice<T>> aseSlices)
    where T: IColor, new()
    {
        List<Slice<T>> slices = new List<Slice<T>>();
        HashSet<string> sliceNameCheck = new HashSet<string>();

        for (int s = 0; s < aseSlices.Length; s++)
        {
            AsepriteSlice<T> aseSlice = aseSlices[s];
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
                T color = aseSlice.UserData.Color ?? new();
                Point origin = aseSliceKey.Pivot;

                Slice<T> slice = aseSlice.IsNinePatch
                    ? new NinePatchSlice<T>(name, bounds, origin, color, aseSliceKey.CenterBounds)
                    : new Slice<T>(name, bounds, origin, color);
                slices.Add(slice);
                break;
            }
        }

        return [.. slices];
    }
}
