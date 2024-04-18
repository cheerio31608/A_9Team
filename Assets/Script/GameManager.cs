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
    public GameObject profilePanel;

    public Text timeTxt;
    public Text nameTxt;
    public Text bestTimeTxt;
    public Text CountTxt;
    public Text ScoreTxt;
    public Text BestScoreTxt;
    public Text EndpanelTitle;
    public Text EndpanelStage;
    public Text Comment;

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

    public static bool game_started = false; // 카드 준비 완료 됐는지 확인 용

    string key = "BestTime";
    string skey = "BestScore";

    string[] match_fail = { "까비", "ㅋ", "실패 !", "뭐해?", "땡 !" };
    string[] stage_1 = { "유년기 김민우", "얼빡샷 김민우", "양치하는 김민우", "거울샷 김민우", "SD김민우", "김민우리신모스트", "JEP김민우", "리 신" };
    string[] stage_2 = { "폭파범", "야너두 할수있는 최지원", "신궁", "JEP최지원", "유년기최지원", "SD최지원", "셀카 최지원", "악수하는 최지원", "스키장 최지원", "네컷 최지원" };
    string[] stage_3 = { "핸드폰 김신우", "SD김신우", "JEP김신우", "피곤한 김신우", "브이 김신우",
        "한복 김신우", "장발 김신우", "여장 김신우", "교복 김신우", "유년기 김신우", "벌칙걸린 김신우", "곰은 사람을 찢어" };
    string[] stage_4 = { "정이현의 라볶이", "SD정이현", "JEP정이현", "머리띠 쓴 정이현", "노래방 정이현",
        "벚꽃샷 정이현", "삼색냥만지는정이현부럽다", "모자쓴 정이현", "정이현의 옥수수", "정이현", "아칼리", "브이 정이현", "고양이몰래찍는정이현", "자는고양이구경하는정이현" };


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
        game_started = false;

        stage = RetryButton.level;

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
        timer_anim = timeTxt.GetComponent<Animator>();
        timer_anim.SetBool("isWarning", false);

        audioSource_tictok = GetComponent<AudioSource>();
        audioSource_tictok.clip = clip_tictok;
        Score = 0;

    }
    void Update()
    {

        if (!game_started)
            return;

        time -= Time.deltaTime;
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

    public void On_Panel()
    {
        profilePanel.SetActive(true);
    }
    public void GameOver()
    {
        Calculate_Score();
        EndPanel();
        profilePanel.SetActive(false);
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
            if (stage == 1)
                nameTxt.text = stage_1[firstCard.idx];
            else if (stage == 2)
                nameTxt.text = stage_2[firstCard.idx];
            else if (stage == 3)
                nameTxt.text = stage_3[firstCard.idx];
            else if (stage == 4)
                nameTxt.text = stage_4[firstCard.idx];

            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();

            cardCount -= 2;
            match_score += 2;

            if (cardCount == 0)
            {
                Time.timeScale = 0.0f;
                On_Panel();
                audioSource.PlayOneShot(finish);
            }
            else // 맞추면 보너스 시간 +1초
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

        //ENDPANEL 이름 변경
        if (cardCount == 0)
        {
            EndpanelTitle.text = "CLEAR";
            Comment.text = "NICE PLAY!";
        }
        else
        {
            EndpanelTitle.text = "GAME OVER";
            Comment.text = "좀 더 노력하세요.";
        }

        if (stage == 1)
            EndpanelStage.text = "STAGE 1";
        else if (stage == 2)
            EndpanelStage.text = "STAGE 2";
        else if (stage == 3)
            EndpanelStage.text = "STAGE 3";
        else
            EndpanelStage.text = "STAGE 4";
    }
}