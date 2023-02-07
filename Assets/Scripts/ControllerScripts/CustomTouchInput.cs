using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomTouchInput : MonoBehaviour
{
    public event Action onTouchAction;

    public GameObject scaleButton;
    private Vector2 _secondTouchPos; //To imitate a second touch
    private Vector2 _initialTouchPosition; //Registrate where your initial touch took place (on left click if you use mouse)
    private float _initialDistanceBetweenTwoTouches;
    private bool _isSwipingRight;
    private bool _isScaling;

    private void Start()
    {
        _isScaling = false;
        _isSwipingRight = false;
        _secondTouchPos = new Vector2(150, 150);
        onTouchAction += DetectRightSwipe;
    }

    private void DetectRightSwipe()
    {
        if (Input.touchCount > 0) // Conditions for a right swipe
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _initialTouchPosition = touch.position;
            }
            if (touch.position.x - _initialTouchPosition.x > 100 && Mathf.Abs(touch.position.y - _initialTouchPosition.y) < 50) // Conditions to judge one swipe complete
            {
                Debug.Log("Swiped right");
                _initialTouchPosition = touch.position; // Here we zero an initial swipe position to the point where the current swipe is complete to make possibile
                                                        // a new swipe without taking off from where we finished 
            }

            if (touch.deltaPosition.x > 0 && !_isSwipingRight) // If we start swiping right, an initial swipe position is assigned to the point where we start swiping from
            {
                _initialTouchPosition = touch.position;
                _isSwipingRight = true;
            }
            if (touch.deltaPosition.x < 0 && _isSwipingRight) // If we start swiping left, an initial swipe position is assigned to the point where we start swiping from
            {
                _initialTouchPosition = touch.position;
                _isSwipingRight = false;
            }
        }
    }

    private void DetectScaling()
    {
        if (Input.touchCount > 0) // We are going to use only one touch, the second is imitated by _secondTouchPos
        {
            Touch touch1 = Input.GetTouch(0);
            if (touch1.phase == TouchPhase.Began)
            {
                _initialDistanceBetweenTwoTouches = Vector2.Distance(touch1.position, _secondTouchPos);
            }

            if (Vector2.Distance(touch1.position, _secondTouchPos) - _initialDistanceBetweenTwoTouches > _initialDistanceBetweenTwoTouches*0.1f) // Should be 10% larger
            {
                Debug.Log("Scaling is in progress");
            }
        }
    }

    public void ActivateScalingSystem()
    {
        _isScaling = !_isScaling;
        if (_isScaling)
        {
            onTouchAction -= DetectRightSwipe;
            onTouchAction += DetectScaling;
            scaleButton.GetComponent<Image>().color = Color.green;
        }
        else if (!_isScaling)
        {
            onTouchAction += DetectRightSwipe;
            onTouchAction -= DetectScaling;
            scaleButton.GetComponent<Image>().color = Color.white;
        }
        
    }

    public Vector2 GetTouchDeltaPosition()
    {
        if(Input.touchCount > 0)
        {
            return Input.GetTouch(0).deltaPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public bool IsThereTouchOnScreen() 
    {
        if (Input.touchCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        onTouchAction?.Invoke();
    }
}
