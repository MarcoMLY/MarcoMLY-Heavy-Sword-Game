using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/Vector")]
    [System.Serializable]
    public class Vector3Holder : ScriptableObject
    {
        [SerializeField] public float[] Variable;
    }
}
