using UnityEngine;

public class BreathEffect : MonoBehaviour
{

    [SerializeField] float amplitude = 0.03f;   // amplitude of the scale change
    [SerializeField] float period = 4f;         // period of the scale change in seconds
    Vector3 baseScale;

    void Awake()
    {
        baseScale = transform.localScale;
        period = Random.Range(period - 0.5f, period + 0.5f);
    }

    void Update()
    {
        float t = Time.time;
        float scale = 1 + amplitude * Mathf.Sin(t * Mathf.PI * 2 / period);
        transform.localScale = new Vector3(baseScale.x, baseScale.y * scale, baseScale.z);
    }
}