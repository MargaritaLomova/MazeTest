using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector2 target { get; private set; }

    private Coroutine checkLongMousePress;

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Stationary:
                    target = touch.deltaPosition;
                    break;
            }
        }
        else
        {
            target = Vector2.zero;
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && checkLongMousePress == null)
        {
            checkLongMousePress = StartCoroutine(CheckLongMousePress());
        }
        else if (!Input.GetMouseButtonDown(0))
        {
            checkLongMousePress = null;
            target = Vector2.zero;
        }
#endif
    }

    private IEnumerator CheckLongMousePress()
    {
        yield return new WaitForSeconds(0.25f);
        while (Input.GetMouseButton(0))
        {
            target = Input.mousePosition;
            yield return null;
        }

        target = Vector2.zero;
    }
}