using UnityEngine;

public class PlatformAttacher : MonoBehaviour
{
    [Header("누가 탈 수 있나요?")]
    [SerializeField] private LayerMask playerLayer;

    // 충돌 시작: "내 위에 탔니? 같이 가자!"
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 닿은 녀석의 레이어가 playerLayer에 포함되는지 확인
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // 플레이어를 발판의 자식으로 만듦 -> 발판 움직일 때 같이 딸려감!
            collision.transform.SetParent(transform);
        }
    }

    // 충돌 끝: "내렸니? 잘 가!"
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.transform.SetParent(null); // 독립 시키기
        }
    }
}
