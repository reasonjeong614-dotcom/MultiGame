using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("문 설정")]
    [SerializeField] private float openHeight = 3f; // 문이 위로 얼마나 올라갈지
    [SerializeField] private float speed = 2f; // 문이 열리는 속도

    private Vector3 closedPos; // 닫힌 위치 (원래 위치)
    private Vector3 targetPos; // 목표 위치 (열린 위치 or 닫힌 위치)

    void Start()
    {
        closedPos = transform.position; // 시작 위치 기억
        targetPos = closedPos; // 처음엔 닫혀있음
    }

    void Update()
    {
        // 목표 위치로 부드럽게 이동 (Lerp나 MoveTowards 사용)
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    // 버튼이 부를 함수: "열려라!"
    public void OpenDoor()
    {
        // 원래 위치에서 openHeight만큼 위로 설정
        targetPos = closedPos + new Vector3(0, openHeight, 0);
    }

    // 버튼이 부를 함수: "닫혀라!"
    public void CloseDoor()
    {
        targetPos = closedPos;
    }
}

