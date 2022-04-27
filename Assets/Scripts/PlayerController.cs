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
    private int rechargeShootTimer = 0;

    private void FixedUpdate()
    {
        if (gameController.isPlay)
        {
            Movement();
            Rotate();

            if(InputController.IsDoubleTap())
            {
                Shoot();
            }
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
        var touchPosition = InputController.GetTouchPosition();

        if (touchPosition.x != 0)
        {
            if (touchPosition.x > Screen.width * 0.5f)
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

    #endregion

    #endregion
}