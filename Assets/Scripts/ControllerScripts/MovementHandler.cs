using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public CustomTouchInput touchInput;
    [SerializeField] private float _ballSpeed = 1f;

    private void Start()
    {
        if(touchInput == null)
        {
            Debug.LogError("touchInput instance is unassigned");
        }
    }

    /// <summary>
    /// Moveball instructions 
    /// </summary>
    private void Moveball()
    {
        if(touchInput.IsThereTouchOnScreen())
        {
            Vector2 currentDeltaPosition = touchInput.GetTouchDeltaPosition();
            currentDeltaPosition = currentDeltaPosition * _ballSpeed;
            Vector3 newGravityVector = new Vector3(currentDeltaPosition.y, Physics.gravity.y, -currentDeltaPosition.x);
            Physics.gravity = newGravityVector;
        }
    }

    private void Update()
    {
        Moveball();
    }
}
