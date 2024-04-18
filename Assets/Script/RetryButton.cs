using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RetryButton : MonoBehaviour
{
    public static int level = 0;

    public void On_GameOver()
    {
        GameManager.Instance.GameOver();
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
        level = 1;
    }
    public void Stage02Scene() //스테이지 씬 2 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage02Scene");
        level = 2;
    }
    public void Stage03Scene() //스테이지 씬 3 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage03Scene");
        level = 3;
    }
    public void Stage04Scene() //스테이지 씬 4 불러오기
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage04Scene");
        level = 4;
    }
}