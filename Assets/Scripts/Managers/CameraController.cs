using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [Header("Scene limits")]
    [SerializeField] private float halfWidth = 4f;

    [Header("Offset & smoothing (optional)")]
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float smoothTime = 0.2f;

    private float minX, maxX;
    private float currentVelocity;

    void OnEnable() => PlayerTransmission.OnTeleported += RecenterWindow;
    void OnDisable() => PlayerTransmission.OnTeleported -= RecenterWindow;

    private void Start()
    {
        player ??= GameObject.FindGameObjectWithTag("Player")?.transform;

        RecenterWindow(player.position.x);
    }

    void LateUpdate()
    {
        if (player == null) return;

        float targetX = Mathf.Clamp(player.position.x + xOffset, minX, maxX);
        float newX = smoothTime > 0 ? Mathf.SmoothDamp(transform.position.x, targetX, ref currentVelocity, smoothTime) : targetX;

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void RecenterWindow(float centerX)
    {
        minX = centerX - halfWidth;
        maxX = centerX + halfWidth;

        float clamped = Mathf.Clamp(player.position.x + xOffset, minX, maxX);
        transform.position = new Vector3(clamped, transform.position.y, transform.position.z);
        currentVelocity = 0;

    }
}
