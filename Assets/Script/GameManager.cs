using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card firstCard;
    public Card secondCard;
    // 김신우 - timeTxt, nameTxt 추가
    public Text timeTxt;
    public Text nameTxt;
    // 정이현 - bestTimeTxt 추가
    public Text bestTimeTxt;
    public GameObject endTxt;
    public AudioSource audioSource;
    public AudioClip clip;
    // 김신우 - miss, finish, fail 추가
    public AudioClip miss;
    public AudioClip finish;
    public AudioClip fail;

    public int cardCount = 0;

    float time = 30.0f;
    int MatchCount = 0;

    int time_score = 0;
    int match_score = 0;
    int match_cnt_score = 0;
    int Score = 0;

    public GameObject endPanel;

    public Text CountTxt; //매치 카운트 텍스트
    public Text ScoreTxt;
    public Text EndTimeTxt;

    AudioSource audioSource_tictok; // 5초 남았을 때 재생할 AudioSource 컴포넌트를 받을 변수 audioSource_tictok
    public AudioClip clip_tictok;
    float warning_time = 5.0f; // 경고 나타낼 시간
    bool is_tictok = false; // clip_tictok 이 플레이되고 있는지

    Color originalColor = new Color(90 / 255f, 90 / 255f, 255 / 255f);
    Color targetColor = Color.red;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();

        Camera.main.backgroundColor = new Color(90 / 255f, 90 / 255f, 255 / 255f);
        timeTxt.transform.localScale = new Vector3(1f, 1f, 1f);

        audioSource_tictok = GetComponent<AudioSource>();
        audioSource_tictok.clip = clip_tictok;

        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        // UI에 최단 기록 표시
        bestTimeTxt.text = "Best Time: " + bestTime.ToString("N2");

        // 현재 기록이 최단 기록보다 빠른지 비교 
        if (time < bestTime) // time이 bestTime보다 작으면 최단 기록 발생
        {
            // 현재 기록이 최단 기록보다 빠르다면 최단 기록 업데이트
            bestTime = time;

            // 최단 기록 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);

            // UI에 최단 기록 표시
            timeTxt.text = "Best Time: " + bestTime.ToString("N2");
        }

        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time <= warning_time) // 경고
        {
            if (!is_tictok) // tictok 오디오
            {
                is_tictok = true;
                audioSource_tictok.loop = true;
                audioSource_tictok.Play();
            }

            // 시간 UI 에 변화 주기
            for (int i = 1; i <= (int)warning_time; ++i)
            {
                float t = (time) / 0.6f;
                float scaleValue = Mathf.Sin(t * Mathf.PI); // 0에서 1로, 그리고 다시 0으로 변화
                timeTxt.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), scaleValue);
                timeTxt.color = Color.Lerp(Color.white, Color.red, scaleValue);
            }
        }

        if (time < 0.0f)
        {
            // 김신우 - fail 사운드 추가
            time = 0f;
            timeTxt.text = time.ToString("N2");

            endPanel.SetActive(true);
            audioSource_tictok.Stop(); // tictok 오디오 종료
            EndPanel();
            //audioSource.PlayOneShot(fail);            
            Time.timeScale = 0.0f;
        }



    }

    public void Matched()
    {
        // 김신우 - 이미지에 따른 조원 이름 표시, 사운드 추가
        // 스테이지별로 인원을 나눴기 때문에 해당 부분 코드 수정이 필요합니다!! (지원)
        if (firstCard.idx == secondCard.idx)
        {
            if (firstCard.idx < 2)
            {
                nameTxt.text = "김민우";
            }
            else if (firstCard.idx >= 2 && firstCard.idx < 4)
            {
                nameTxt.text = "김신우";
            }
            else if (firstCard.idx >= 4 && firstCard.idx < 6)
            {
                nameTxt.text = "정이현";
            }
            else
            {
                nameTxt.text = "최지원";
            }
            audioSource.PlayOneShot(clip);

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;

            // 매칭 성공 시 점수
            match_score += 2;

            if (cardCount == 0)
            {
                Time.timeScale = 0.0f;
                endPanel.SetActive(true);
                EndPanel();
                audioSource.PlayOneShot(finish);

            }
        }
        else
        {
            nameTxt.text = "실패!";
            audioSource.PlayOneShot(miss);

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
        MatchCount++;
    }


    void EndPanel()
    {
        // 시간 점수
        if (time >= 15.0f)
            time_score = 50;
        else if (time >= 12.0f)
            time_score = 45;
        else if (time >= 10.0f)
            time_score = 40;
        else if (time > 0.0f)
            time_score = 30;
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

        // 점수 계산
        Score = time_score + match_cnt_score + match_score;

        EndTimeTxt.text = time.ToString("N2");
        CountTxt.text = MatchCount.ToString();

        ScoreTxt.text = Score.ToString();


    }
}
