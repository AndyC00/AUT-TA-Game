using System;
using UnityEngine;

public class PlayerTransmission : MonoBehaviour
{
    // singleton pattern
    public static PlayerTransmission Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public static event Action<float> OnTeleported;

    public void TeleportTo(Vector3 worldPos)
    {
        transform.position = worldPos;

        OnTeleported?.Invoke(worldPos.x);
    }
}
