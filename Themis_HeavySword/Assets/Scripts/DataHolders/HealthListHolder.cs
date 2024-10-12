using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/DataHolders/HealthList")]
    [System.Serializable]
    public class HealthListHolder : IHoldListData
    {
        public List<Health> Variable { get; private set; } = new List<Health>();

        public void AddData(Health newVariable)
        {
            Variable.Add(newVariable);
        }

        public void RemoveData(Health newVariable)
        {
            Variable.Remove(newVariable);
        }

        public override void ClearData()
        {
            for (int i = Variable.Count - 1; i >= 0; i--)
            {
                Variable.RemoveAt(i);
            }
            Variable.Clear();
        }
    }
}
