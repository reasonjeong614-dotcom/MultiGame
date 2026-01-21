using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectManager : MonoBehaviour
{
    // 1탄 버튼에 연결할 함수
    public void GoToStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    // 2탄 버튼에 연결할 함수
    public void GoToStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void GoToStage3()
    {
        SceneManager.LoadScene("Stage3");
    }

    public void GoToStage4()
    {
        SceneManager.LoadScene("Stage4");
    }

    // (선택) 다시 타이틀로 돌아가는 뒤로가기 버튼용
    public void GoBack()
    {
        SceneManager.LoadScene("TitleScene");
    }
}

