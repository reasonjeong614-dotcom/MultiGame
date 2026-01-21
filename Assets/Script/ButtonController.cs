using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("제어할 대상 (둘 중 하나만 연결해도 됨)")]
    [SerializeField] private DoorController targetDoor;      // 문 연결
    [SerializeField] private MovingPlatform targetPlatform;  // 발판 연결

    [Header("누가 밟아야 작동할까?")]
    [SerializeField] private LayerMask playerLayer; // 레이어 감지 방식

    [Header("버튼 색깔")]
    [SerializeField] private Color pressedColor = Color.green; // 눌렸을 때
    private Color originalColor = Color.red; // 뗐을 때
    private SpriteRenderer mySprite;

    private int pressCount = 0; // 몇 명이 밟고 있는지 카운트

    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        originalColor = mySprite.color; // 원래 색 기억
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 레이어인지 확인
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            pressCount++; // 밟은 사람 추가

            if (pressCount == 1)
            {
                // 문이 연결되어 있으면 문 열기
                if (targetDoor != null) targetDoor.OpenDoor();

                // 발판이 연결되어 있으면 발판 가동!
                if (targetPlatform != null) targetPlatform.StartMoving();

                mySprite.color = pressedColor;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            pressCount--;

            if (pressCount <= 0)
            {
                pressCount = 0;

                // 문 닫기
                if (targetDoor != null) targetDoor.CloseDoor();

                // 발판 멈추기
                if (targetPlatform != null) targetPlatform.StopMoving();

                mySprite.color = originalColor;
            }
        }
    }
}

