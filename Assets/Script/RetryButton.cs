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
    public void SelectScene() //���� �� �ҷ�����
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("SelectScene");
    }
    public void Stage01Scene() //�������� �� 1 �ҷ�����
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage01Scene");
        level = 1;
    }
    public void Stage02Scene() //�������� �� 2 �ҷ�����
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage02Scene");
        level = 2;
    }
    public void Stage03Scene() //�������� �� 3 �ҷ�����
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage03Scene");
        level = 3;
    }
    public void Stage04Scene() //�������� �� 4 �ҷ�����
    {
        AudioManager.Instance.btnclick();
        SceneManager.LoadScene("Stage04Scene");
        level = 4;
    }
}