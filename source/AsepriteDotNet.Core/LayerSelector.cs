using AsepriteDotNet.Core.Types;

namespace AsepriteDotNet.Core;

/// <summary>
/// Provides strategies for selecting layers from an Aseprite file.
/// </summary>
public abstract class LayerSelector
{
    /// <summary>
    /// Creates a selector that includes all layers.
    /// </summary>
    /// <returns>A layer selector that selects all layers.</returns>
    public static LayerSelector AllLayers() => new AllLayerSelector();

    /// <summary>
    /// Creates a selector that includes only visible layers.
    /// </summary>
    /// <returns>A layer selector that selects only visible layers.</returns>
    public static LayerSelector VisibleLayers() => new VisibleLayerSelector();

    /// <summary>
    /// Creates a selector that includes layers with the specified names.
    /// </summary>
    /// <param name="layerNames">The names of layers to select.</param>
    /// <returns>A layer selector that selects layers by name.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="layerNames"/> is <see langword="null"/>.</exception>
    public static LayerSelector ByName(params string[] layerNames) => new NameLayerSelector(layerNames);

    /// <summary>
    /// Creates a selector that includes layers matching the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each layer for inclusion.</param>
    /// <returns>A layer selector that selects layers using the predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is <see langword="null"/>.</exception>
    public static LayerSelector Predicate(Func<AsepriteLayer, bool> predicate) => new PredicateLayerSelector(predicate);

    /// <summary>
    /// Selects layers from the provided collection based on the selector's criteria.
    /// </summary>
    /// <param name="layers">The layers to select from.</param>
    /// <returns>A span of selected layers maintaining the original order.</returns>
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
