using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private Vector3 moveOffset = new Vector3(5, 0, 0); // 이동할 거리
    [SerializeField] private float speed = 2f; // 이동 속도

    [Header("시작 옵션")]
    [SerializeField] private bool isAutomatic = false; // 체크하면 버튼 없이 혼자 움직임

    private Vector3 startPos;
    private Vector3 endPos;
    private bool isMoving = false;
    private float currentTime = 0f; // 나만의 시간 (멈추면 얘도 멈춤)

    // 0 ~ 1 사이를 왕복하는 핑퐁 값
    private float pingPongValue;

    void Start()
    {
        startPos = transform.position; // 시작 위치
        endPos = startPos + moveOffset; // 끝 위치 계산

        // 자동 모드면 시작하자마자 움직임 켜기
        if (isAutomatic) isMoving = true;
    }

    void Update()
    {
        // 움직이는 상태일 때만 계산!
        if (isMoving)
        {
            // 1. 나만의 시간을 흐르게 함
            currentTime += Time.deltaTime * speed;

            // 2. 0~1 사이를 왕복 (PingPong)
            float t = Mathf.PingPong(currentTime, 1f);

            // 3. 위치 이동
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }
    }

    // 버튼이 부를 함수: "움직여!"
    public void StartMoving()
    {
        isMoving = true;
    }

    // 버튼이 부를 함수: "멈춰!"
    public void StopMoving()
    {
        isMoving = false;
    }
}

