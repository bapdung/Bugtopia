using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using API.Insect;
using Models.Insect.Response;

public class InsectDetailController : MonoBehaviour
{

    private Coroutine countdownCoroutine;

    private InsectApi insectApi; // InsectApi 인스턴스 참조
    private DateTime lastEatTime;

    [SerializeField] private Image areaContainer;
    [SerializeField] private TextMeshProUGUI area;

    [SerializeField] private Image interactInfoContainer;
    [SerializeField] private TextMeshProUGUI interactInfo;

    [SerializeField] private Image feedInfoContainer;
    [SerializeField] private TextMeshProUGUI feedInfo1;
    [SerializeField] private TextMeshProUGUI feedInfo2;

    [SerializeField] private Image insectInfoContainer;
    [SerializeField] private TextMeshProUGUI insectNickname;
    [SerializeField] private TextMeshProUGUI insectName;

    [SerializeField] private Button detailBtn;
    [SerializeField] private TextMeshProUGUI detailBtnText;

    [SerializeField] private Image todoContainer;
    [SerializeField] private Image interactContainer;
    [SerializeField] private TextMeshProUGUI interactContainerText;
    [SerializeField] private Image feedContainer;
    [SerializeField] private TextMeshProUGUI feedContainerText;

    [SerializeField] private Button ARBtn;

    private int feedCnt;        // 오늘 먹이를 먹은 횟수
    private int interactCnt;    // 오늘 상호작용한 횟수

    void Awake()
    {
        // insectApi가 할당되지 않았을 경우 코드 내에서 생성
        if (insectApi == null)
        {
            GameObject insectApiObject = new GameObject("InsectApiObject");  // 새 GameObject 생성
            insectApi = insectApiObject.AddComponent<InsectApi>();  // InsectApi 컴포넌트를 추가하여 할당
        }
    }

    private void Start()
    {
        long raisingInsectId = 5; // => 하드코딩 (추후수정)

        if (insectApi != null)
        {
            StartCoroutine(insectApi.GetInsectInfo(raisingInsectId, OnSuccess, OnFailure));
        }
        else
        {
            Debug.LogError("InsectApi 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnSuccess(InsectDetailInfoResponse response)
    {
        insectNickname.text = response.info.nickname;
        insectName.text = response.info.insectName;

        feedCnt = response.loveScore.feedCnt;
        feedContainerText.text = "먹이주기 ( " + feedCnt + "/2)";

        interactCnt = response.loveScore.interactCnt;
        interactContainerText.text = "쓰다듬기 (" + interactCnt + "/10)";

        if(response.info.areaType == "FOREST") {
            area.text = "숲";
            areaContainer.color = new Color(41f / 255f, 157f / 255f, 89f / 255f);
        } else if (response.info.areaType == "WATER") {
            area.text = "연못";
            areaContainer.color = new Color(55f / 255f, 102f / 255f, 230f / 255f);
        } else if (response.info.areaType == "GARDEN") {
            area.text = "꽃밭";
            areaContainer.color = new Color(209f / 255f, 51f / 255f, 235f / 255f);
        }

        if (feedCnt >= 2) {
            feedInfo1.text = "오늘은 다 먹었어요";
            feedInfo2.text = "먹이는 하루에 두 번만 줘요";
            feedContainer.color = new Color(255f / 255f, 143f / 255f, 28f / 255f, 0.14f);
        } else {
            feedContainer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0.07f);
            
            if (DateTime.TryParse(response.loveScore.lastEat, out lastEatTime))
            {
                StartCountdown();
            }
        }

        if(interactCnt >= 10) {
            interactContainer.color = new Color(255f / 255f, 143f / 255f, 28f / 255f, 0.14f);
        } else {
            interactContainer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0.07f);
        }
    }

    private void OnFailure(string error)
    {
        // 실패 시 오류 메시지 출력
        Debug.LogError("Failed to fetch insect info: " + error);
    }

    private void StartCountdown() {
        // 기존에 진행 중이던 카운트다운을 중지하고 새로 시작
        if (countdownCoroutine != null) {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(UpdateCountdown());
    }

     private IEnumerator UpdateCountdown()
    {
        while (true)
        {
            TimeSpan timeDifference = DateTime.Now - lastEatTime;
            TimeSpan remainingTime = TimeSpan.FromHours(6) - timeDifference;

            if (remainingTime.TotalSeconds <= 0)
            {
                feedInfo1.text = "오늘의 먹이 주기";
                feedInfo2.text = "지금 먹이를 줄 수 있어요!";
                break;
            }
            else
            {
                feedInfo1.text = "지금은 배가 불러요";
                feedInfo2.text = "다음 밥 시간 - " + string.Format("{0:D2}:{1:D2}:{2:D2}",
                    remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
            }

            yield return new WaitForSeconds(1);
        }
    }
}