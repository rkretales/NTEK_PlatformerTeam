using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapRotation : MonoBehaviour
{
    public float speed = 50f;
    protected float _verticalMovement;
    protected float _horizontalMovement;
    protected bool _axisBased = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Rotate(_verticalMovement, _horizontalMovement, 0);
    }

    public virtual void Move(Vector2 newMovement)
    {
        if (!_axisBased)
        {
            _horizontalMovement = newMovement.x;
            _verticalMovement = newMovement.y;
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
