using UnityEngine;
using UnityEngine.InputSystem;

public class WhitePlayerController : MonoBehaviour
{
    [Header("설정값")]
    [SerializeField] private float jumpPower = 7f; // 점프 힘
    [SerializeField] private LayerMask groundLayer; // 땅 레이어
    [SerializeField] private Transform groundCheck; // 발바닥 위치
    [SerializeField] private Vector2 groundBoxSize = new Vector2(0.5f, 0.1f); //발바닥 크기

    [Header("점프 보정 (코요테 타임)")]
    [SerializeField] private float coyoteTime = 0.1f; // 0.1초의 여유
    private float coyoteTimeCounter; // 타이머
    private float mountCooldown = 0f;

    [Header("슈퍼 점프 설정")]
    [SerializeField] private float pushForce = 15f; // 검정이를 밀어 올리는 힘
    [SerializeField] private LayerMask playerLayer; // 검정이를 감지할 레이어

    private Rigidbody2D rb;
    private Collider2D myCollider;
    private InputSystem_Actions inputActions;

    private bool isGrounded; // 땅에 있는지 여부
    public bool isRiding { get; private set; } = false;  // 내 리짓바디 상태를 저장할 변수
    private Collider2D currentBusCollider; // 검정이 충돌체

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable() 
    { 
        inputActions.White.Enable(); 
    }
    void OnDisable() 
    { 
        inputActions.White.Disable(); 
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // 내 충돌체 가져오기
    }

    void Update()
    {
        // 쿨타임 줄이기
        if (mountCooldown > 0) mountCooldown -= Time.deltaTime;

        // 땅 체크
        Collider2D hitCollider = Physics2D.OverlapBox(groundCheck.position, groundBoxSize, 0f, groundLayer);
        isGrounded = hitCollider != null;

        // 코요테 타임 계산
        if (isGrounded || isRiding)
        {
            coyoteTimeCounter = coyoteTime; // 땅이나 버스에 있으면 시간 충전
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // 공중이면 시간 깎임
        }

        // 점프
        if (inputActions.White.Jump.WasPerformedThisFrame())
        {
            // "땅에 붙어있니?" 대신 "땅에서 떨어진지 얼마 안 됐니?"를 체크
            if (coyoteTimeCounter > 0f)
            {
                Jump();
                return; // 점프했으면 아래 "버스 타기/내리기" 로직은 건너뜀!
            }
        }

        if (mountCooldown <= 0)        // 쿨타임 중이면 타기 로직을 아예 실행하지 않음!
        {
            // 부모 설정 (버스 타기) 로직
            if (isGrounded)
            {
                // 내가 밟은 게 검정이에 속하는가?
                if ((playerLayer.value & (1 << hitCollider.gameObject.layer)) > 0)
                {
                    if (!isRiding) // 처음 타는 순간 딱 한 번만 실행
                    {
                        GetOnBus(hitCollider);
                    }
                }
                else
                {
                    // 그냥 땅이다 -> 독립
                    GetOffBus();
                }
            }
            else
            {
                // 공중이다 -> 독립 (점프하면 내려야 함)
                GetOffBus();
            }
        }
    }

    // 버스 타기 함수
    void GetOnBus(Collider2D busCollider)
    {
        if (isRiding) return;

        isRiding = true;
        currentBusCollider = busCollider;

        transform.SetParent(busCollider.transform);

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        // 서로 충돌 끄기 (물리 엔진 싸움 방지)
        Physics2D.IgnoreCollision(myCollider, currentBusCollider, true);
    }

    // 버스 내리기 함수
    void GetOffBus()
    {
        if (isRiding)
        {
            transform.SetParent(null);
            rb.bodyType = RigidbodyType2D.Dynamic;

            // 내렸으니 다시 충돌 켜기 (안 그러면 검정이 뚫고 지나감)
            if (currentBusCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, currentBusCollider, false);
                currentBusCollider = null;
            }

            isRiding = false;
        }
        else if (transform.parent != null)
        {
            transform.SetParent(null);
        }
    }


    void Jump()
    {
        // 점프하면 코요테 타임 즉시 0으로 (무한 점프 방지)
        coyoteTimeCounter = 0f;
        // 점프했으니 0.2초 동안은 다시 타지 마! (요요 방지)
        mountCooldown = 0.2f;

        // 0. 점프 전, 검정이의 속도를 미리 저장할 변수
        Vector2 parentVelocity = Vector2.zero;

        // 1. 만약 타고 있었다면? 부모(검정이)의 속도를 훔쳐오자!
        if (isRiding && transform.parent != null)
        {
            Rigidbody2D parentRb = transform.parent.GetComponent<Rigidbody2D>();
            if (parentRb != null)
            {
                parentVelocity = parentRb.linearVelocity; // 검정이 속도 복사
            }
            GetOffBus(); // 이제 내림 (물리 켜짐)
        }

        // 2. 점프! (검정이 속도 + 점프 힘)
        // X축: 검정이 속도 그대로 유지
        // Y축: 점프 힘
        rb.linearVelocity = new Vector2(parentVelocity.x, jumpPower);

        // 머리 위에 검정이가 있는지 검사 (레이캐스트 쏘기)
        // 내 머리 위 0.5f 거리 안에 "Player" 레이어를 가진 물체가 있나?
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, playerLayer);

        if (hit.collider != null)
        {
            // 맞은 놈(검정이)의 리짓바디 가져오기
            Rigidbody2D targetRb = hit.collider.GetComponent<Rigidbody2D>();

            if (targetRb != null)
            {
                // 3. 검정이에게 강제로 위쪽 힘을 팍! 줌 (Impulse = 순간적인 힘)
                targetRb.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);

                Debug.Log("슈퍼 점프 발동!");
            }
        }
    }

    // 리스폰 매니저가 부르는 강제 초기화 (멱살잡기용)
    public void ForceReset()
    {
        GetOffBus();

        if (rb != null) rb.linearVelocity = Vector2.zero;
        coyoteTimeCounter = 0;
        mountCooldown = 0;
    }

}

