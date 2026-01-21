using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // 플레이어 타입 정의 (열거형)
    public enum PlayerType { Black, White }

    [Header("내 역할 설정 (테스트용)")]
    // 이 값을 Inspector에서 바꾸면 그 사람 시점으로 시작됨!
    public PlayerType myPlayerType = PlayerType.Black;

    [SerializeField] private CameraFollow cameraScript;

    [Header("캐릭터들")]
    [SerializeField] private GameObject blackPlayer;
    [SerializeField] private GameObject whitePlayer;
    [SerializeField] private GameObject mergedPlayer;

    [Header("설정")]
    [SerializeField] private float separateOffset = 1.1f; // 분리될 때 위아래 간격

    // 외부에서 현재 상태를 알 수 있게 프로퍼티 추가
    public bool IsMerged { get; private set; } = false;

    private InputSystem_Actions inputActions;
    private bool isMerged = false;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Black.Enable();
        inputActions.White.Enable();
    }

    void OnDisable()
    {
        inputActions.Black.Disable();
        inputActions.White.Disable();
    }

    void Start()
    {
        // 게임 시작 시, 내 역할에 맞는 캐릭터를 카메라 타겟으로 설정!
        if (myPlayerType == PlayerType.Black)
        {
            cameraScript.SetTarget(blackPlayer.transform);
        }
        else
        {
            cameraScript.SetTarget(whitePlayer.transform);
        }
    }

    void Update()
    {
        // 합체/분리 키 (E) 감지
        bool blackPressed = inputActions.Black.Interact.WasPerformedThisFrame();
        bool whitePressed = inputActions.White.Interact.WasPerformedThisFrame();

        if (blackPressed || whitePressed)
        {
            if (isMerged) Separate();
            else TryMerge();
        }
    }

    void TryMerge()
    {
        // 둘 사이의 거리 체크 (너무 멀면 합체 X)
        float distance = Vector2.Distance(blackPlayer.transform.position, whitePlayer.transform.position);

        if (distance <= 1.5f) // 합체 가능 거리
        {
            // 1. 합체 위치 계산 (둘의 중간 위치)
            Vector3 centerPos = (blackPlayer.transform.position + whitePlayer.transform.position) / 2;

            // 2. 개별 캐릭터 숨기기
            blackPlayer.SetActive(false);
            whitePlayer.SetActive(false);

            // 3. 합체 캐릭터 등장!
            mergedPlayer.transform.position = centerPos;
            mergedPlayer.SetActive(true);
            cameraScript.SetTarget(mergedPlayer.transform); //카메라 타겟 변경

            isMerged = true;
        }
    }

    public void Separate()
    {
        // 현재 합체 캐릭터의 위치 가져오기
        Vector3 currentPos = mergedPlayer.transform.position;

        // 합체 캐릭터가 켜져 있을 때만
        if (mergedPlayer.activeSelf)
        {
            // 분리될 때 위치를 잡아주는 로직
            blackPlayer.transform.position = currentPos + new Vector3(0, separateOffset, 0);
            whitePlayer.transform.position = currentPos + new Vector3(0, 0.1f, 0);
        }

        mergedPlayer.SetActive(false); // 합체 꺼!
        blackPlayer.SetActive(true);   // 까망이 켜!
        whitePlayer.SetActive(true);   // 하양이 켜!

        // "내 캐릭터"를 다시 따라가야 함
        if (myPlayerType == PlayerType.Black)
        {
            cameraScript.SetTarget(blackPlayer.transform);
        }
        else
        {
            cameraScript.SetTarget(whitePlayer.transform);
        }

        isMerged = false;
    }
}

