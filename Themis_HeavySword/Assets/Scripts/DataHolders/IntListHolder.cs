using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/IntList")]
    [System.Serializable]
    public class IntListHolder : IHoldListData
    {
        [field: SerializeField] public List<int> Variable { get; protected set; } = new List<int>();
        
        public void AddData(int newIntList)
        {
            Variable.Add(newIntList);
        }

        public void RemoveData(int newIntList)
        {
            Variable.Remove(newIntList);
        }

        public override void ClearData()
        {
            Variable.Clear();
        }
    }
}
