using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Variables To Control"), SerializeField]
    private float smoothSpeed = 0.125f;
    [SerializeField]
    private Vector3 offset;

    [Header("Scene Objects"), SerializeField]
    private Transform player;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition / 0.8f, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void FastFocusOnPlayer()
    {
        transform.position = player.position + offset;
    }
}