using UnityEngine;

public static class InputController
{
    public static Vector2 GetTouchPosition()
    {
        Vector2 touchPosition = new Vector2();
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            touchPosition = Input.mousePosition;
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
        }
#endif
        return touchPosition;
    }

    public static bool IsDoubleTap()
    {
#if UNITY_EDITOR

        return Event.GetEventCount() > 1;

#elif UNITY_ANDROID || UNITY_IOS

        var firstTouch = Input.GetTouch(0);

        if (Input.touchCount == 1 && firstTouch.phase == TouchPhase.Began)
        {
            return firstTouch.deltaTime > 0 && firstTouch.deltaTime < 1 && firstTouch.deltaPosition.magnitude < 1;
        }

        return false;

#endif
    }
}