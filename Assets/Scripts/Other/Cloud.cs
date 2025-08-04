using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private bool moveLeft = true;
    [SerializeField] private float speed = 1f;

    void Update()
    {
        if (moveLeft)   transform.position += Vector3.left * speed * Time.deltaTime;
        else   transform.position += Vector3.right * speed * Time.deltaTime;
    }

}