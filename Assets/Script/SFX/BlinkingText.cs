using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private float speed = 2f; // 깜빡이는 속도
    [SerializeField] private Color myColor = Color.white; // 글씨 색깔

    private TextMeshProUGUI myText;

    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // 1. 투명도(Alpha) 계산 (0 ~ 1 사이를 오르락내리락)
        // Mathf.Sin은 -1 ~ 1을 반복하는데, 이걸 0 ~ 1로 예쁘게 가공합니다.
        float alpha = (Mathf.Sin(Time.time * speed) * 0.5f) + 0.5f;

        // 2. 색상 적용 (기존 색상 유지하되 투명도만 바꿈)
        myText.color = new Color(myColor.r, myColor.g, myColor.b, alpha);
    }
}
