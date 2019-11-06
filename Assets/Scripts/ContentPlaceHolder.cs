using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentPlaceHolder : MonoBehaviour
{
    [SerializeField] protected float _movementDamper = 1f;
    [SerializeField] protected float _scaleSpeed = 0.5f;
    [SerializeField] protected Vector3 _initialPosition;
    [SerializeField] protected Vector3 _initialScale;


    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        HandleTouch();
    }

    protected virtual void Init()
    {
        transform.localPosition = _initialPosition;
        transform.localScale = _initialScale;
    }

    protected virtual void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            transform.position += (Vector3)Input.GetTouch(0).deltaPosition * _movementDamper * Time.deltaTime;
        }
        else if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;
            Vector3 deltaScaleDiff = Vector3.one * deltaMagnitudeDiff * Time.deltaTime;

            transform.localScale += deltaScaleDiff * _scaleSpeed;
        }
        
    }
}
