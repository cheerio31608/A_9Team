using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Unity.VisualScripting;
public class Board : MonoBehaviour
{
    public GameObject card;
    float distance, Xmore, Ymore;
    int howManyCard;

    void Start()
    {
        howManyCard = 0;
        Xmore = 0;
        Ymore = 0;
        distance = 1.25f; // 요기 3개는 스테이지 3부터는 카드 스케일이 달라져서 거리 조절용 변수입니다!
        howManyCard = 5 + (2 * RetryButton.level);

        int[] arr = new int[12 + 4 * RetryButton.level];

        if (RetryButton.level >= 3)
        {
            Xmore = (RetryButton.level - 2) * 0.19f;
            distance = 1.25f - (RetryButton.level - 2) * 0.115f;
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }

        if (RetryButton.level >= 2)
            Ymore = 0.45f + (RetryButton.level - 1) * 0.25f;

        for (int i = 0; i < 12 + 4 * RetryButton.level; i++)
            arr[i] = i / 2;

        arr = arr.OrderBy(x => Random.Range(0f, howManyCard)).ToArray();
        GameManager.Instance.cardCount = arr.Length;
        StartCoroutine(CardCreate(arr));
       
    }
    IEnumerator CardCreate(int[] arr)
    {
        for (int i = 0; i < (howManyCard + 1) * 2; i++)
        {
            GameObject go = Instantiate(card, this.transform);
            float x = (i % 4) * distance - 1.875f + Xmore;
            float y = (i / 4) * distance - 2.85f - Ymore;
            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);
            Card.Instance.CardCreatAnim();
            yield return new WaitForSeconds(0.025f);
        }

        GameManager.Instance.nameTxt.text = "시작 !";
        GameManager.game_started = true;
        yield break;
    }
}