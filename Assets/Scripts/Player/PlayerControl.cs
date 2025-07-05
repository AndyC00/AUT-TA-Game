using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody2D rb;
    private PlayerControls controls;

    private float moveInput;     // ?? X?-1 ~ 1?
    private bool jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // ????????
        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>().x;
        controls.Player.Move.canceled += _ => moveInput = 0f;

        controls.Player.Jump.performed += _ => jumpQueued = true;
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void FixedUpdate()
    {
        // ? ??????????????
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ? ??????????“y ???0 ?????”?
        if (jumpQueued && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        jumpQueued = false;   // ??????
    }
}
