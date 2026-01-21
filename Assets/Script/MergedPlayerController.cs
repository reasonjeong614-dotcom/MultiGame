using UnityEngine;
using UnityEngine.InputSystem;

public class MergedPlayerController : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private float rollSpeed = 50f; // 구르기 속도 조절

    [Header("참조")]
    [SerializeField] private Transform visualTransform; // 회전시킬 이미지 오브젝트
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rb;
    private InputSystem_Actions inputActions;
    private bool isGrounded;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    // 활성화(합체)될 때 입력을 켜고, 비활성화(분리)될 때 입력을 꺼야 함
    void OnEnable()
    {
        inputActions.Black.Enable();
        inputActions.White.Enable();
        rb = GetComponent<Rigidbody2D>(); // 켜질 때 Rigidbody 다시 잡기

        /* 합체할 때마다 이미지를 똑바로(0도) 세우기!
        if (visualTransform != null)
        {
            visualTransform.localRotation = Quaternion.identity;
        }
        */
    }

    void OnDisable()
    {
        inputActions.Black.Disable();
        inputActions.White.Disable();
    }

    void Update()
    {
        // 1. 땅 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 1. 이동 (검정 유저)
        float moveInput = inputActions.Black.Move.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 2. 점프 (하양 유저)
        if (inputActions.White.Jump.WasPerformedThisFrame() && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        // 3. 데굴데굴 구르기
        RollVisual();
    }

    void RollVisual()
    {
        if (visualTransform == null) return;

        // 현재 실제로 움직이는 속도를 가져옴
        float currentSpeed = rb.linearVelocity.x;

        // 속도가 거의 없으면 회전 안 함
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            // 이동 방향 반대로 회전해야 굴러가는 것처럼 보임
            // 오른쪽 이동(+) -> 시계방향 회전(-)
            float rotateAmount = -currentSpeed * rollSpeed * Time.deltaTime;
            visualTransform.Rotate(0, 0, rotateAmount);
        }
    }
}

