// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace AsepriteDotNet.Processors;

/// <summary>
/// Defines the options to use when processing an Aseprite file.
/// </summary>
/// <param name="OnlyVisibleLayers">Indicates whether only visible layers should be processed.</param>
/// <param name="IncludeBackgroundLayer">Indicates whether the layer assigned as the background layer should be processed.</param>
/// <param name="IncludeTilemapLayers">Indicates whether tilemap layers should be processed.</param>
/// <param name="MergeDuplicateFrames">Indicates whether duplicates frames should be merged.</param>
/// <param name="BorderPadding">The amount of transparent pixels to add to the edge of the generated texture.</param>
/// <param name="Spacing">The amount of transparent pixels to add between each texture region in the generated texture.</param>
/// <param name="InnerPadding">The amount of transparent pixels to add around the edge of each texture region in the generated texture.</param>
[Obsolete("ProcessorOptions will be removed in a future release.  Users should switch to one of the Process method overloads that does not require an instance of these options", false)]
public record ProcessorOptions(bool OnlyVisibleLayers,
                                           bool IncludeBackgroundLayer,
                                           bool IncludeTilemapLayers,
                                           bool MergeDuplicateFrames,
                                           int BorderPadding,
                                           int Spacing,
                                           int InnerPadding)
{
    /// <summary>
    /// Provides a default set of options for the <see cref="ProcessorOptions"/>.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Property</term>
    ///         <description>Default Value</description>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="OnlyVisibleLayers"/></term>
    ///         <description><see langword="true"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="IncludeBackgroundLayer"/></term>
    ///         <description><see langword="false"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="IncludeTilemapLayers"/></term>
    ///         <description><see langword="true"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="MergeDuplicateFrames"/></term>
    ///         <description><see langword="true"/></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="BorderPadding"/></term>
    ///         <description><c>0</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Spacing"/></term>
    ///         <description><c>0</c></description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="InnerPadding"/></term>
    ///         <description><c>0</c></description>
    ///     </item>
    /// </list>
    /// </summary>
    [Obsolete("ProcessorOptions will be removed in a future release.  Users should switch to one of the Process method overloads that does not require an instance of these options", false)]
    public static readonly ProcessorOptions Default = new ProcessorOptions(true, false, true, true, 0, 0, 0);
}

