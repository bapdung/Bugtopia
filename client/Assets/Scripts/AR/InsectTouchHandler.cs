// InsectTouchHandler.cs
using UnityEngine;
using API.Insect;
using Models.Insect.Request;
using Models.Insect.Response;

public class InsectTouchHandler : MonoBehaviour
{
    private InsectApi insectApi;
    private InsectInfoResponse insectInfoResponse;
    private IncreaseScoreResponse increaseScoreResponse;

    public void Initialize(InsectApi api, InsectInfoResponse infoResponse)
    {
        insectApi = api;
        insectInfoResponse = infoResponse;
    }

    private void Update()
    {
        // 터치 감지
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 시작되었을 때만 처리
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // 레이캐스트를 사용하여 터치된 오브젝트가 이 오브젝트인지 확인
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == this.transform)
                    {
                        OnInsectTouched(); // 터치된 경우 동작 수행
                    }
                }
            }
        }
    }

    // 터치되었을 때 발생할 동작
    private void OnInsectTouched()
    {
        Debug.Log($"지흔: {gameObject.name}이 터치되었습니다!");

        var increaseScoreRequest = new IncreaseScoreRequest
        {
            raisingInsectId = insectInfoResponse.raisingInsectId,
            category = 2
        };

        // 점수 증가 API 호출
        StartCoroutine(insectApi.PostIncreaseScore(increaseScoreRequest,
            onSuccess: (response) => {

                Debug.Log("점수 증가 성공");
            },
            onFailure: error => Debug.LogError("점수 증가 실패: " + error)
        ));
    }
}