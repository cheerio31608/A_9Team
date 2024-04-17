using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public SpriteRenderer frontImage;
    public SpriteRenderer background; // 카드의 뒷면 (색상 변경을 위해서)
    public GameObject front;
    public GameObject back;
    public AudioClip clip;
    public AudioSource audioSource;

    public Animator anim;

    bool matched_fail = false;

    private Coroutine myCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        background = back.GetComponentInChildren<SpriteRenderer>(); // 뒷면 색상 변경을 위해서

    }

    public void Setting(int number)
    {
        idx = number;
        string objectName = gameObject.name; // 현재 오브젝트 이름을 받아와서 확인할거에용
        //Debug.Log(objectName);

        if (objectName == "Card1 MW(Clone)") // 만약 오브젝트 이름이 MW 라면
        {
            //Debug.Log(" 민우님 카드 찾음");
            frontImage.sprite = Resources.Load<Sprite>($"MW/MW_{idx}"); //MW 폴더 내부에 있는 mw_idx 파일을 프론트로 설정
        }
        else if (objectName == "Card2 JW(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"JW/JW_{idx}");
        }
        else if (objectName == "Card3 SW(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"SW/SW_{idx}");
        }
        else if (objectName == "Card4 LH(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"LH/LH_{idx}");
        }
    }

    public void OpenCard()
    {
        if(GameManager.Instance.secondCard != null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);

        // Flip 애니메이션에서 SetActive 설정해서 따로 필요 없음
        //front.SetActive(true);
        //back.SetActive(false);

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
            GameManager.Instance.firstCard.StartCoroutineExample();
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.firstCard.StopCoroutineExample();
            GameManager.Instance.Matched();
        }
    }

    public void StartCoroutineExample()
    {
        myCoroutine = StartCoroutine(MyCoroutine());
    }

    public void StopCoroutineExample()
    {
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
            Debug.Log("코루틴 중지됨");
        }
    }

    IEnumerator MyCoroutine()
    {
        Debug.Log("Coroutine 시작");
        yield return new WaitForSeconds(5.0f);
        CloseCardInvoke();
        GameManager.Instance.firstCard = null;
        Debug.Log("Coroutine 종료");
    }
    public void DestroyCard()
    {
        Invoke("DestoryCardInvoke", 0.5f);
    }

    void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        if (!matched_fail)
        {
            matched_fail = true;
            background.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
        }

        // flip 애니메이션에서 SetActive 설정해서 따로 필요 없음
        // front.SetActive(false);
        // back.SetActive(true);
    }
}
