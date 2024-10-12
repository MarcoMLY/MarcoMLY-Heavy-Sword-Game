using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour, ISwordStickable
{
    [SerializeField] private MaterialType _crystalType;
    [SerializeField] private TemporaryStorage _temporaryStorageType;
    [SerializeField] private GameObject _droppedCrystal;
    [SerializeField] private int _amountDroppedMin;
    [SerializeField] private int _amountDroppedMax;

    public void DestroyCrystal()
    {
        int amountDropped = Random.Range(_amountDroppedMin, _amountDroppedMax + 1);
        for (int i = 0; i < amountDropped; i++)
        {
            Vector3 randomAmount = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            DroppedMaterial droppedCrystal = Instantiate(_droppedCrystal, transform.position + randomAmount, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<DroppedMaterial>();
            droppedCrystal.SetData(_crystalType, _temporaryStorageType);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
