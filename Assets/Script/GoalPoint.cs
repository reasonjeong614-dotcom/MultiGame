using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalPoint : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private string nextSceneName = "Stage2"; // 이동할 맵 이름

    [Header("누가 닿아야 문을 열어줄까?")]
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 물체의 레이어가 playerLayer에 포함되어 있는지 확인 (비트 연산)
        if ((playerLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("다음 스테이지로!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

