using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables To Control"), SerializeField]
    private float moveSpeed = 2f;

    [Header("Objects From Scene"), SerializeField]
    private ShopController shopController;
    [SerializeField]
    private GameController gameController;

    private float acceleration = 1;
    private float doubleClickTimeLimit = 0.25f;
    private int rechargeShootTimer = 0;

    private void Start()
    {
        StartCoroutine(InputListener());
    }

    private void FixedUpdate()
    {
        if (gameController.isPlay)
        {
            Movement();
            Rotate();
        }
    }

    #region Custom Methods

    private void Movement()
    {
        acceleration += Time.fixedDeltaTime / 2;
        transform.Translate(transform.forward * (moveSpeed / 10) * acceleration * Time.smoothDeltaTime);
    }

    private void Rotate()
    {
        if (GetTouchPosition().x != 0)
        {
            if (GetTouchPosition().x > Screen.width * 0.5f)
            {
                transform.rotation *= Quaternion.Euler(0, 90f * Time.deltaTime, 0);
            }
            else
            {
                transform.rotation *= Quaternion.Euler(0, -90f * Time.deltaTime, 0);
            }
        }
    }

    private void Shoot()
    {
        if (rechargeShootTimer == 0)
        {
            gameController.RemoveNearestToPlayerWall();
            rechargeShootTimer = 10;
            StartCoroutine(TimerRecovery());
        }
    }

    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    #region Helpers

    private IEnumerator InputListener()
    {
        while (enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                yield return ClickEvent();
            }

            yield return null;
        }
    }

    private IEnumerator ClickEvent()
    {
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < doubleClickTimeLimit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator TimerRecovery()
    {
        float temp = rechargeShootTimer;
        while(temp > 0)
        {
            
            temp -= Time.fixedDeltaTime;
            yield return null;
        }
        rechargeShootTimer = (int)temp;
    }

    private Vector2 GetTouchPosition()
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

    #endregion

    #endregion
}