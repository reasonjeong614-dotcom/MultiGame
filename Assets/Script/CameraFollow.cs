using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("대상")]
    public Transform target; // 현재 따라가는 대상

    [Header("설정")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 10f;

    // 외부에서 타겟을 바꿀 수 있게 함수 추가
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 타겟이 꺼져있으면(비활성화) 추적 중지 (혹은 에러 방지)
        if (!target.gameObject.activeInHierarchy) return;

        float clampedY = Mathf.Clamp(target.position.y, minY, maxY);
        Vector3 targetPos = new Vector3(target.position.x, clampedY, -10f);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}



