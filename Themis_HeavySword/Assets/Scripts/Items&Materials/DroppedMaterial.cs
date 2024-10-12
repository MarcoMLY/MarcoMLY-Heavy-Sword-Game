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

    private InputAction _playerPickingUp;
    [SerializeField] private InputActionAsset _inputAsset;

    [SerializeField] private TemporaryStorage _temporaryStorageType;

    [SerializeField] private GameEventString _sendControlPrompt;
    [SerializeField] private GameEventString _endControlPrompt;
    [SerializeField] private string _controlPrompt;
    private bool _controlPromptSent;

    public void SetData(MaterialType crystalType, TemporaryStorage temporaryStorageType)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _type = crystalType;
        _temporaryStorageType = temporaryStorageType;
        _spriteRenderer.color = _type.Color;
        _spriteRenderer.sprite = _type.Sprite;
        gameObject.AddComponent<PolygonCollider2D>();

        InputActionMap playerMap = _inputAsset.FindActionMap("Player");
        playerMap.Enable();

        _playerPickingUp = playerMap.FindAction("PickThingsUp");
    }

    private void Update()
    {
        if (!CanBeSuckedUp())
        {
            if (_controlPromptSent)
                _endControlPrompt.EventTriggered(_controlPrompt + "|" + gameObject.GetInstanceID());
            _controlPromptSent = false;
            return;
        }

        if (!_controlPromptSent)
        {
            _sendControlPrompt.EventTriggered(_controlPrompt + "|" + gameObject.GetInstanceID());
            _controlPromptSent = true;
        }

        if (_playerPickingUp.IsPressed())
        {
            _temporaryStorageType.StoreMaterial();
            _endControlPrompt.EventTriggered(_controlPrompt + "|" + gameObject.GetInstanceID());
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _colliderRadius);
    }
}
