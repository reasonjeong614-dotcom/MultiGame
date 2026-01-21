using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        // Input Actions 초기화
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        // Input Actions 활성화
        inputActions.Black.Enable();
    }

    void OnDisable()
    {
        // Input Actions 비활성화
        inputActions.Black.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력값 받기
        Vector2 moveInput = inputActions.Black.Move.ReadValue<Vector2>();

        // 플레이어 이동
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }
}


