using UnityEngine;
using API.Insect;
using Models.Insect.Request;
using Models.Insect.Response;
using System;
using System.Collections;
using TMPro;

public class InsectTouchHandler : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    public GameObject heartPrefab;
    private InsectApi insectApi;
    private InsectArInfoResponse insectInfoResponse;
    private IncreaseScoreResponse increaseScoreResponse;
    private Animator insectAnimator;
   
    public void Initialize(InsectApi api, InsectArInfoResponse infoResponse, Animator animator)
    {
        insectApi = api;
        insectInfoResponse = infoResponse;
        heartPrefab = Resources.Load<GameObject>("AR/Heart");
        insectAnimator = animator;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == this.transform)
                    {
                        OnInsectTouched();
                    }
                }
            }
        }
    }

    private void OnInsectTouched()
    {
        Debug.Log($"지흔: {gameObject.name}이 터치되었습니다!");

        // 하트 표시 코루틴 시작
        StartCoroutine(ShowHeart());

        // 알림 텍스트 표시
        StartCoroutine(ShowNotificationText());

        if (insectAnimator != null)
        {
            insectAnimator.SetTrigger("hit");
            StartCoroutine(SetInsectTouch());
        }

        var increaseScoreRequest = new IncreaseScoreRequest
        {
            raisingInsectId = insectInfoResponse.raisingInsectId,
            category = 2
        };

        StartCoroutine(insectApi.PostIncreaseScore(increaseScoreRequest,
            onSuccess: (response) => {
                increaseScoreResponse = response;
                Debug.Log("점수 증가 성공");
            },
            onFailure: error => Debug.LogError("점수 증가 실패: " + error)
        ));
    }

    private IEnumerator ShowHeart()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("heartPrefab이 할당되지 않았습니다.");
            yield break;
        }

        GameObject heartInstance = Instantiate(heartPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity);
        heartInstance.transform.SetParent(this.transform);

        yield return new WaitForSeconds(2.0f); 

        Destroy(heartInstance);
    }

    private IEnumerator ShowNotificationText()
    {
        notificationText.text = $"{insectInfoResponse.nickname}을(를) 쓰다듬었어요!";
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3.0f);

        notificationText.gameObject.SetActive(false);
    }

    private IEnumerator SetInsectTouch()
    {
        yield return new WaitForSeconds(2.0f);
        insectAnimator.SetBool("hit", false);
        insectAnimator.SetBool("idle", true);
    }
}
