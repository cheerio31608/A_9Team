using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card firstCard;
    public Card secondCard;

    public GameObject endPanel;

    public Text timeTxt;
    public Text nameTxt;
    public Text bestTimeTxt;
    public Text CountTxt;
    public Text ScoreTxt;
    public Text BestScoreTxt;

    public AudioSource audioSource;
    public AudioSource audioSource_tictok;

    public AudioClip clip;
    public AudioClip miss;
    public AudioClip finish;
    public AudioClip fail;
    public AudioClip clip_tictok;

    public Animator timer_anim;

    int stage;
    float time = 30.0f;
    float warning_time = 5.0f;

    public int cardCount = 0;
    int MatchCount = 0;

    int time_score = 0;
    int match_score = 0;
    int match_cnt_score = 0;
    int Score = 0;

    bool is_tictok = false; // clip_tictok 이 플레이되고 있는지
    bool game_started = true; // 게임이 시작됐는지

    string key = "BestTime";
    string skey = "BestScore";
    string[] match_success = { "성공 !", "Good !", "Great !", "Perfect !" }; 
    string[] match_fail = { "까비", "ㅋ", "실패 !", "뭐해?", "땡 !" };

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        stage = RetryButton.level;
        Debug.Log(stage);
        // 스테이지 별 시간 설정
        if (stage == 1)
            time = 30.0f;
        else if (stage == 2)
            time = 45.0f;
        else if (stage == 3)
            time = 60.0f;
        else
            time = 80.0f;
        audioSource = GetComponent<AudioSource>();
        timer_anim = timeTxt.GetComponentInChildren<Animator>();
        timer_anim.SetBool("isWarning", false);
        audioSource_tictok = GetComponent<AudioSource>();
        audioSource_tictok.clip = clip_tictok;
        Score = 0;
    }
    void Update()
    {
        StartCoroutine(TimeSetting(1 + stage * 0.1f));
    }
    IEnumerator TimeSetting(float t)  //Timer 함수 그대로 코루틴으로 바꿨습니다. (게임 시작 후 몇초간 딜레이 적용 가능)
    {
        yield return new WaitForSeconds(t);
        time -= Time.deltaTime;
        if (game_started)
        {
            nameTxt.text = "시작 !";
            game_started = false;
        }
        Card.time_started = true;
        if (time <= warning_time) // 경고
        {
            if (!is_tictok) // tictok 오디오
            {
                timer_anim.SetBool("isWarning", true);
                is_tictok = true;
                audioSource_tictok.loop = true;
                audioSource_tictok.Play();
            }
        }
        if (time < 0.0f)
        {
            // 김신우 - fail 사운드 추가
            time = 0f;
            endPanel.SetActive(true);
            audioSource_tictok.Stop();
            GameOver();
            //EndPanel();
        }
        timeTxt.text = time.ToString("N2");
    }
    void GameOver()
    {
        Time.timeScale = 0.0f;
        Calculate_Score();
        EndPanel();
        endPanel.SetActive(true);
    }
    public void Matched()
    {
        CancelInvoke();
        MatchCount++;
        // 김신우 - 이미지에 따른 조원 이름 표시, 사운드 추가
        // 스테이지별로 인원을 나눴기 때문에 해당 부분 코드 수정이 필요합니다!! (지원)
        if (firstCard.idx == secondCard.idx)
        {
            nameTxt.text = match_success[Random.Range(0, match_success.Length)];
            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            match_score += 2;
            if (cardCount == 0)
            {
                GameOver();
                //Time.timeScale = 0.0f;
                //endPanel.SetActive(true);
                //EndPanel();
                audioSource.PlayOneShot(finish);
            }
            else // 맞추면 보너스 시간 +0.2초
            {
                timer_anim.SetTrigger("PlayIncrease");
                time += 1.0f;
            }
               
        }
        else
        {
            nameTxt.text = match_fail[Random.Range(0, match_fail.Length)];
            audioSource.PlayOneShot(miss);
            timer_anim.SetTrigger("PlayDecrease");
            time -= 0.5f; // 틀리면 페널티 시간 -0.5초
            firstCard.CloseCard();
            secondCard.CloseCard();
        }
        //firstCard = null;
        //secondCard = null;
    }
    void Calculate_Score()
    {
        if (stage == 1) // 50 + 34 + 16 = 100
        {
            // 시간 점수
            if (time >= 15.0f)
                time_score = 50;
            else if (time >= 10.0f)
                time_score = 40;
            else if (time >= 5.0f)
                time_score = 30;
            else if (time > 0.0f)
                time_score = 20;
            else
                time_score = 0;
            // 매칭 시도 횟수 점수
            if (cardCount == 0)
            {
                if (MatchCount <= 16)
                    match_cnt_score = 34;
                else if (MatchCount <= 20)
                    match_cnt_score = 30;
                else if (MatchCount <= 24)
                    match_cnt_score = 26;
                else if (MatchCount <= 28)
                    match_cnt_score = 22;
                else if (MatchCount <= 32)
                    match_cnt_score = 18;
                else
                    match_cnt_score = 0;
            }
        }
        else if (stage == 2) // 50 + 30 + 20
        {
            // 시간 점수
            if (time >= 20.0f)
                time_score = 50;
            else if (time >= 15.0f)
                time_score = 40;
            else if (time >= 10.0f)
                time_score = 30;
            else if (time >= 5.0f)
                time_score = 20;
            else if (time > 0.0f)
                time_score = 10;
            else
                time_score = 0;
            // 매칭 시도 횟수 점수
            if (cardCount == 0)
            {
                if (MatchCount <= 20)
                    match_cnt_score = 30;
                else if (MatchCount <= 25)
                    match_cnt_score = 25;
                else if (MatchCount <= 30)
                    match_cnt_score = 20;
                else if (MatchCount <= 35)
                    match_cnt_score = 15;
                else if (MatchCount <= 40)
                    match_cnt_score = 10;
                else
                    match_cnt_score = 0;
            }
        }
        else if (stage == 3) // 50 + 26 + 24
        {
            // 시간 점수
            if (time >= 30.0f)
                time_score = 50;
            else if (time >= 20.0f)
                time_score = 40;
            else if (time >= 15.0f)
                time_score = 30;
            else if (time >= 5.0f)
                time_score = 20;
            else if (time > 0.0f)
                time_score = 10;
            else
                time_score = 0;
            // 매칭 시도 횟수 점수
            if (cardCount == 0)
            {
                if (MatchCount <= 24)
                    match_cnt_score = 26;
                else if (MatchCount <= 30)
                    match_cnt_score = 20;
                else if (MatchCount <= 36)
                    match_cnt_score = 14;
                else if (MatchCount <= 42)
                    match_cnt_score = 8;
                else if (MatchCount <= 48)
                    match_cnt_score = 2;
                else
                    match_cnt_score = 0;
            }
        }
        else  // level == 4,  50 + 22 + 28
        {
            // 시간 점수
            if (time >= 40.0f)
                time_score = 50;
            else if (time >= 20.0f)
                time_score = 40;
            else if (time >= 10.0f)
                time_score = 30;
            else if (time >= 5.0f)
                time_score = 20;
            else if (time > 0.0f)
                time_score = 10;
            else
                time_score = 0;
            // 매칭 시도 횟수 점수
            if (cardCount == 0)
            {
                if (MatchCount <= 28)
                    match_cnt_score = 22;
                else if (MatchCount <= 38)
                    match_cnt_score = 16;
                else if (MatchCount <= 48)
                    match_cnt_score = 12;
                else if (MatchCount <= 58)
                    match_cnt_score = 6;
                else
                    match_cnt_score = 0;
            }
        }
        Score = time_score + match_cnt_score + match_score;
    }
    void EndPanel()
    {
        BestScoreTxt.text = Score.ToString("N2");
        CountTxt.text = MatchCount.ToString();
        ScoreTxt.text = Score.ToString();
        // 최단시간 판단
        string stage_key = key + stage.ToString();
        string stage_skey = skey + stage.ToString();
        if (PlayerPrefs.HasKey(stage_key))
        {
            float best = PlayerPrefs.GetFloat(stage_key);
            if (best < time)
            {
                PlayerPrefs.SetFloat(stage_key, time);
                bestTimeTxt.text = time.ToString("N2");
            }
            else
                bestTimeTxt.text = best.ToString("N2");
        }
        else
        {
            PlayerPrefs.SetFloat(stage_skey, time);
            bestTimeTxt.text = time.ToString("N2");
        }
        // 최고점수 판단
        if (PlayerPrefs.HasKey(stage_skey))
        {
            float best = PlayerPrefs.GetFloat(stage_skey);
            if (best < Score)
            {
                PlayerPrefs.SetFloat(stage_skey, Score);
                BestScoreTxt.text = Score.ToString();
            }
            else
                BestScoreTxt.text = best.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat(stage_skey, Score);
            BestScoreTxt.text = Score.ToString();
        }
    }
}