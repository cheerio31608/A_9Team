using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("MainScene");
    }

    public void SelectScene() //선택 씬 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("SelectScene");
    }

    public void Stage01Scene() //스테이지 씬 1 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage01Scene");
    }
    public void Stage02Scene() //스테이지 씬 2 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage02Scene");
    }
    public void Stage03Scene() //스테이지 씬 3 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage03Scene");
    }
    public void Stage04Scene() //스테이지 씬 4 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage04Scene");
    }

}
