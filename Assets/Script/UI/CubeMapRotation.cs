using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapRotation : MonoBehaviour
{
    public float speed = 50f;
    protected float _verticalMovement;
    protected float _horizontalMovement;
    protected bool _axisBased = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(_verticalMovement, _horizontalMovement, 0);
    }

    public virtual void Move(Vector2 newMovement)
    {
        if (!_axisBased)
        {
            _horizontalMovement = newMovement.x * speed;
            _verticalMovement = newMovement.y * speed;
            // Debug.Log(newMovement.x);
        }
    }

    public virtual void SetHorizontalAxis(float value)
    {
        _axisBased = true;
        _horizontalMovement = value;
        Debug.Log(_horizontalMovement);
    }

    public virtual void SetVerticalAxis(float value)
    {
        _axisBased = true;
        _verticalMovement = value;
        Debug.Log(_verticalMovement);
    }
}
