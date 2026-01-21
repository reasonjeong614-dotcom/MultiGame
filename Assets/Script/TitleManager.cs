using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    [Header("효과음 설정")]
    [SerializeField] private AudioSource audioSource; // 스피커
    [SerializeField] private AudioClip startSound;    // "띠링" 소리 파일

    private bool isEntered = false; // 중복 입력 방지

    void Update()
    {
        // 엔터(Enter) 키가 눌렸는지 매 프레임 검사
        if (Keyboard.current.enterKey.wasPressedThisFrame && !isEntered)
        {
            StartCoroutine(EnterProcess());
        }
    }

    // 시간차 공격을 위한 코루틴 함수
    IEnumerator EnterProcess()
    {
        isEntered = true; // "나 눌렸어!" 표시 (중복 실행 방지)

        Debug.Log("엔터 눌림!");

        // 1. 소리 재생
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound);
        }

        // 2. 소리가 들릴 만큼 잠깐 대기 (0.5초)
        yield return new WaitForSeconds(0.5f);

        // 3. 씬 이동
        SceneManager.LoadScene("SelectScene");
    }
}

