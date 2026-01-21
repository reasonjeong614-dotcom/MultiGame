using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("연결 필요")]
    [SerializeField] private GameManager gameManager; // 합체 해제 요청용
    [SerializeField] private Transform blackPlayer;
    [SerializeField] private Transform whitePlayer;

    [SerializeField] private WhitePlayerController whiteController;

    // 시작 위치 저장용
    private Vector3 blackStartPos;
    private Vector3 whiteStartPos;

    void Start()
    {
        // 1. 게임 시작 시점의 위치를 '세이브 포인트'로 저장
        if (blackPlayer != null) blackStartPos = blackPlayer.position;
        if (whitePlayer != null) whiteStartPos = whitePlayer.position;

        // 하양이 컨트롤러 자동 찾기 (혹시 연결 안 했을까봐)
        if (whiteController == null && whitePlayer != null)
        {
            whiteController = whitePlayer.GetComponent<WhitePlayerController>();
        }
    }

    // 누군가 죽으면 이 함수를 호출함
    public void RespawnPlayers()
    {
        Debug.Log("리스폰 프로세스 가동");

        // 1. 합체 상태라면 강제 분리 요청!
            gameManager.Separate();

        // 2. 하양이의 '탑승(Riding)' 상태 해제
        if (whiteController != null)
        {
            whiteController.ForceReset(); // 하양이 스크립트에 만들어둔 초기화 함수 호출
        }

        // 3. 하양이, 까망이 위치 초기화
        ResetPlayer(blackPlayer, blackStartPos);
        ResetPlayer(whitePlayer, whiteStartPos);
    }

    // 위치랑 속도 초기화
    private void ResetPlayer(Transform player, Vector3 targetPos)
    {
        if (player == null) return;

        player.position = targetPos;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // 떨어지던 속도 멈춤
        }
    }
}

