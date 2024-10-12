using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private MaterialType _materialType;
    [SerializeField] private TemporaryStorage _temporaryStorageType;
    [SerializeField] private GameObject _droppedMaterial;
    [SerializeField] private float _dropChance;

    public void Drop()
    {
        if (Random.Range(0, 100) > _dropChance)
            return;
        Vector3 randomAmount = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
        DroppedMaterial droppedMaterial = Instantiate(_droppedMaterial, transform.position + randomAmount, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<DroppedMaterial>();
        droppedMaterial.SetData(_materialType, _temporaryStorageType);
    }
}
