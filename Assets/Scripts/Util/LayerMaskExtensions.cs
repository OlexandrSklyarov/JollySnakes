using UnityEngine;

namespace Util
{
    public static class LayerMaskExtensions
    {
        public static bool LayerContains(this int targetLayer, LayerMask mask)
        {
            return (mask & (1 << targetLayer)) != 0;
        }
    }
}