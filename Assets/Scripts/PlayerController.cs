using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Variables To Control")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Objects From Scene")]
    [SerializeField] private ShopController shopController;
    [SerializeField] private GameController gameController;
    [SerializeField] private InputController inputController;

    private float acceleration = 1;
    private int rechargeShootTimer = 0;

    private void FixedUpdate()
    {
        if (gameController.isPlay)
        {
            Movement();
            Rotate();
        }
    }

    #region Custom Methods

    public void SetColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

    public void Shoot()
    {
        if (rechargeShootTimer == 0)
        {
            gameController.RemoveNearestToPlayerWall();
            rechargeShootTimer = 10;
            StartCoroutine(TimerRecovery());
        }
    }

    private void Movement()
    {
        acceleration += Time.fixedDeltaTime / 2;
        transform.Translate(transform.forward * (moveSpeed / 10) * acceleration * Time.smoothDeltaTime);
    }

    private void Rotate()
    {
        if (inputController.target.x > 0)
        {
            transform.rotation *= inputController.target.x > Screen.width / 2 ? Quaternion.Euler(0, 90f * Time.deltaTime, 0) : Quaternion.Euler(0, -90f * Time.deltaTime, 0);
        }
    }

    private IEnumerator TimerRecovery()
    {
        float temp = rechargeShootTimer;
        while (temp > 0)
        {

            temp -= Time.fixedDeltaTime;
            yield return null;
        }
        rechargeShootTimer = (int)temp;
    }

    #endregion
}