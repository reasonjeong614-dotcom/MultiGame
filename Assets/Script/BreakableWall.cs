using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("설정")]
    // 인스펙터에서 'Player3' 레이어 체크
    [SerializeField] private LayerMask destroyerLayer;

    // 부서지는 데 필요한 최소 충돌 속도
    [SerializeField] private float breakSpeed = 5f;

    // 충돌했을 때 발생하는 함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 레이어 확인 (비트 연산)
        // "부딪힌 녀석의 레이어가 destroyerLayer 설정에 포함되어 있는가?"
        if ((destroyerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // 2. 충분히 빠른 속도로 들이받았나?
            if (collision.relativeVelocity.magnitude > breakSpeed)
            {
                BreakWall();
            }
        }
    }

    void BreakWall()
    {
        Debug.Log("쿠궁! 벽이 부서졌다!");

        // (심화) 나중에 여기에 '파편 튀는 이펙트'나 '쾅 소리' 넣으면 좋음

        Destroy(gameObject); // 벽 삭제!
    }
}

