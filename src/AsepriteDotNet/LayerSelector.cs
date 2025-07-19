// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using AsepriteDotNet.Types;

namespace AsepriteDotNet;

/// <summary>
/// Provides layer filtering strategies for selecting subsets of layers from Aseprite files.
/// </summary>
/// <remarks>
/// All selector implementations return new arrays containing references to the original layers,
/// preserving the original layer order while filtering based on specific criteria.
/// </remarks>
public abstract class LayerSelector
{
    /// <summary>
    /// Creates a selector that returns all layers without filtering.
    /// </summary>
    /// <returns>A <see cref="LayerSelector"/> that selects every layer.</returns>
    public static LayerSelector AllLayers() => new AllLayerSelector();

    /// <summary>
    /// Creates a selector that returns only layers marked as visible.
    /// </summary>
    /// <returns>A <see cref="LayerSelector"/> that filters layers based on their visibility state.</returns>
    public static LayerSelector VisibleLayers() => new VisibleLayerSelector();

    /// <summary>
    /// Creates a selector that returns layers whose names match any of the specified values.
    /// </summary>
    /// <param name="layerNames">The layer names to match against.</param>
    /// <returns>A <see cref="LayerSelector"/> that filters layers by exact name matching.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="layerNames"/> is null.</exception>
    public static LayerSelector ByName(params string[] layerNames) => new NameLayerSelector(layerNames);

    /// <summary>
    /// Creates a selector that returns layers matching the specified predicate function.
    /// </summary>
    /// <param name="predicate">The function that determines whether a layer should be selected.</param>
    /// <returns>A <see cref="LayerSelector"/> that filters layers using the provided predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is null.</exception>
    public static LayerSelector Predicate(Func<AsepriteLayer, bool> predicate) => new PredicateLayerSelector(predicate);

    /// <summary>
    /// Selects layers from the provided collection based on the selector's filtering criteria.
    /// </summary>
    /// <param name="layers">The collection of layers to filter.</param>
    /// <returns>A span containing the selected layers in their original order.</returns>
    public abstract ReadOnlySpan<AsepriteLayer> SelectLayers(ReadOnlySpan<AsepriteLayer> layers);

    private sealed class AllLayerSelector : LayerSelector
    {
        public override ReadOnlySpan<AsepriteLayer> SelectLayers(ReadOnlySpan<AsepriteLayer> layers)
        {
            return layers.ToArray();
        }
    }

    private sealed class VisibleLayerSelector : LayerSelector
    {
        public override ReadOnlySpan<AsepriteLayer> SelectLayers(ReadOnlySpan<AsepriteLayer> layers)
        {
            List<AsepriteLayer> matchedLayers = [];

            for (int i = 0; i < layers.Length; i++)
            {
                AsepriteLayer layer = layers[i];
                if (layer.IsVisible)
                {
                    matchedLayers.Add(layer);
                }
            }

            return matchedLayers.ToArray();
        }
    }

    private sealed class NameLayerSelector : LayerSelector
    {
        private readonly HashSet<string> _layerNames;

        public NameLayerSelector(string[] layerNames)
        {
            ArgumentNullException.ThrowIfNull(layerNames);
            _layerNames = new HashSet<string>(layerNames, StringComparer.Ordinal);
        }

        public override ReadOnlySpan<AsepriteLayer> SelectLayers(ReadOnlySpan<AsepriteLayer> layers)
        {
            List<AsepriteLayer> matchedLayers = [];

            for (int i = 0; i < layers.Length; i++)
            {
                AsepriteLayer layer = layers[i];
                if (_layerNames.Contains(layer.Name))
                {
                    matchedLayers.Add(layer);
                }
            }

            return matchedLayers.ToArray();
        }
    }

    private sealed class PredicateLayerSelector : LayerSelector
    {
        private readonly Func<AsepriteLayer, bool> _predicate;

        public PredicateLayerSelector(Func<AsepriteLayer, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            _predicate = predicate;
        }

        public override ReadOnlySpan<AsepriteLayer> SelectLayers(ReadOnlySpan<AsepriteLayer> layers)
        {
            List<AsepriteLayer> matchedLayers = [];

            for (int i = 0; i < layers.Length; i++)
            {
                AsepriteLayer layer = layers[i];
                if (_predicate(layer))
                {
                    matchedLayers.Add(layer);
                }
            }

            return matchedLayers.ToArray();
        }
    }
}
