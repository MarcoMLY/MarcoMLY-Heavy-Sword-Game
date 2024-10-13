using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DroppedMaterial : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private MaterialType _type;

    [SerializeField] private float _colliderRadius;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private TemporaryStorage _temporaryStorageType;

    [SerializeField] private GameEventString _sendControlPrompt;
    [SerializeField] private GameEventString _endControlPrompt;
    [SerializeField] private string _controlPrompt;
    [SerializeField] private int _importance;
    private bool _controlPromptSent, _pickingThingsUp = false;

    public void SetData(MaterialType crystalType, TemporaryStorage temporaryStorageType)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _type = crystalType;
        _temporaryStorageType = temporaryStorageType;
        _spriteRenderer.color = _type.Color;
        _spriteRenderer.sprite = _type.Sprite;
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (!CanBeSuckedUp())
        {
            if (_controlPromptSent)
                _endControlPrompt.EventTriggered(_controlPrompt + "|" + _importance.ToString());
            _controlPromptSent = false;
            return;
        }

        if (!_controlPromptSent)
        {
            _sendControlPrompt.EventTriggered(_controlPrompt + "|" + _importance.ToString());
            _controlPromptSent = true;
        }

        if (_pickingThingsUp)
        {
            _temporaryStorageType.StoreMaterial();
            _endControlPrompt.EventTriggered(_controlPrompt + "|" + _importance.ToString());
            GetPickedUp();
        }
    }

    private bool CanBeSuckedUp()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _colliderRadius, _layerMask);
        return hit;
    }
    
    private void GetPickedUp()
    {
        Destroy(gameObject);
    }

    public void PickingThingsUp()
    {
        _pickingThingsUp = true;
    }

    public void StoppedPickingUp()
    {
        _pickingThingsUp = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _colliderRadius);
    }
}
