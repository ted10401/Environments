using UnityEngine;

public static class LayerMaskUtils
{
    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
