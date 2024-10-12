using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class LayerMaskExtention
{
    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return (layerMask & (1 << layer)) != 0;
    }
}
