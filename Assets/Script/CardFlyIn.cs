using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardFlyIn : MonoBehaviour
{
    public Transform startPoint; // 시작점 (화면 밖)
    public Transform fixedEndPoint; // 고정된 EndPoint 위치 (화면 중앙)
    public float speed = 1f;

    private Transform endPoint; // 끝점 (화면 중앙)

    void Start()
    {
        // 초기 위치 설정
        transform.position = startPoint.position;

        // 고정된 EndPoint 위치로 설정
        endPoint = fixedEndPoint;

        // speef 값을 수정하여 움직임 속도 조절
        speed = 2.5f; // 움직임 속도를 2.5배로 빠르게 설정

        // GameObject 활성화
        startPoint.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);

        // 코루틴 시작
        StartCoroutine(FlyIn());
    }

    IEnumerator FlyIn()
    {
        float t = 0f;

        while (t <= 1f)
        {
            t += speed * Time.deltaTime;

            // Bezier Curve 계산 : 't' 값을 이용하여 나선형 움직임에 사용될 베지어 곡선을 계산
            Vector3 position = Mathf.Pow(1 - t, 2) * startPoint.position +
                               2 * (1 - t) * t * endPoint.position +
                               Mathf.Pow(t, 2) * endPoint.position;

            // 카드 위치 업데이트
            transform.position = position;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 움직임 완료 후 GameObject 비활성화
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
