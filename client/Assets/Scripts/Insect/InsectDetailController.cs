using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using API.Insect;
using Models.Insect.Response;

public class InsectDetailController : MonoBehaviour
{
    private InsectApi insectApi; // InsectApi 인스턴스 참조

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
        long raisingInsectId = 7; // => 하드코딩 (추후수정)

        if (insectApi != null)
        {
            StartCoroutine(insectApi.GetInsectInfo(raisingInsectId, OnSuccess, OnFailure));
        }
        else
        {
            Debug.LogError("InsectApi 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnSuccess(InsectInfoResponse response)
    {
        insectNickname.text = response.nickname;
        insectName.text = response.insectName;

        feedCnt = response.feedCnt;
        feedContainerText.text = "먹이주기 ( " + feedCnt + "/2)";

        interactCnt = response.interactCnt;
        interactContainerText.text = "쓰다듬기 (" + interactCnt + "/10)";

        if(response.areaType == "FOREST") {
            area.text = "숲";
            areaContainer.color = new Color(41f / 255f, 157f / 255f, 89f / 255f);
        } else if (response.areaType == "WATER") {
            area.text = "연못";
            areaContainer.color = new Color(55f / 255f, 102f / 255f, 230f / 255f);
        } else if (response.areaType == "GARDEN") {
            area.text = "꽃밭";
            areaContainer.color = new Color(209f / 255f, 51f / 255f, 235f / 255f);
        }

        if (feedCnt >= 2) {
            feedInfo1.text = "오늘은 다 먹었어요";
            feedInfo2.text = "먹이는 하루에 두 번만 줘요";
            feedContainer.color = new Color(255f / 255f, 143f / 255f, 28f / 255f, 0.14f);
        } else {
            feedContainer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0.07f);
            feedInfo1.text = "오늘의 먹이 주기";
            feedInfo2.text = "지금 먹이를 줄 수 있어요!";
        }
    }

    private void OnFailure(string error)
    {
        // 실패 시 오류 메시지 출력
        Debug.LogError("Failed to fetch insect info: " + error);
    }
}