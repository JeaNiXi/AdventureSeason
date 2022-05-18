using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [SerializeField] private Vector2 _parallaxEffectMultiplier;

    [SerializeField] private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;

    private void Start()
    {
        _lastCameraPosition = _cameraTransform.position;
        Sprite _backSprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D _backTexture = _backSprite.texture;
        _textureUnitSizeX = (_backTexture.width / _backSprite.pixelsPerUnit) * transform.localScale.x;
        //Debug.Log(_backTexture.width + " is width of backtexture");
        //Debug.Log(_backSprite.pixelsPerUnit + " is pixel per unit size of sprite");

        //Debug.Log(_textureUnitSizeX + " is texture unit size X = 1/2");
    }

    private void LateUpdate()
    {
        Vector3 _deltaMovement = _cameraTransform.position - _lastCameraPosition;
        gameObject.transform.position += new Vector3(_deltaMovement.x * _parallaxEffectMultiplier.x, _deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.transform.position;

        if (Mathf.Abs(_cameraTransform.position.x - gameObject.transform.position.x) >= _textureUnitSizeX)
        {
            float offsetPositionX = (_cameraTransform.position.x - gameObject.transform.position.x) % _textureUnitSizeX;
            transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
