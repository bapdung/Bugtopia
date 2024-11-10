using UnityEngine;
using API.Insect;
using Models.Insect.Request;
using Models.Insect.Response;
using System;

public class InsectTouchHandler : MonoBehaviour
{
    private InsectApi insectApi;
    private InsectArInfoResponse insectInfoResponse;
    private IncreaseScoreResponse increaseScoreResponse;
    private Action<string, float> ShowNotification; // 알림 표시 콜백

    public void Initialize(InsectApi api, InsectArInfoResponse infoResponse)
    {
        insectApi = api;
        insectInfoResponse = infoResponse;
    }

    public void SetNotificationHandler(Action<string, float> showNotification)
    {
        ShowNotification = showNotification;
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

        var increaseScoreRequest = new IncreaseScoreRequest
        {
            raisingInsectId = insectInfoResponse.raisingInsectId,
            category = 2
        };

        StartCoroutine(insectApi.PostIncreaseScore(increaseScoreRequest,
            onSuccess: (response) => {
                increaseScoreResponse = response;
                Debug.Log("점수 증가 성공");
                
                ShowNotification?.Invoke(insectInfoResponse.nickname + "을(를) 쓰다듬었어요!", 3f);
            },
            onFailure: error => Debug.LogError("점수 증가 실패: " + error)
        ));
    }
}