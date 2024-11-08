using TMPro;
using UnityEngine;
using System.Collections;
using API.Insect;
using Models.Insect.Response;

public class InsectDetailController : MonoBehaviour
{
    private InsectApi insectApi; // InsectApi 인스턴스 참조

    private TextMeshProUGUI insectNickname; // 곤충 닉네임
    private TextMeshProUGUI insectName;     // 곤충명(한글)
    private TextMeshProUGUI areaType;       // 지역 종류

    private int feedCnt;        // 오늘 먹이를 먹은 횟수
    private int interactCnt;    // 오늘 상호작용한 횟수

    private bool canEat = false;        // 오늘 먹이를 추가로 줄 수 있는지 여부
    private bool canInteract = false;   // 오늘 상호작용을 더 할 수 있는지 여부

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

        // TextMeshPro 텍스트 요소 동적 생성
        insectNickname = CreateTextElement("Insect Nickname");
        insectName = CreateTextElement("Insect Name");
        areaType= CreateTextElement("Area Type");
        
        if (insectApi != null)
        {
            StartCoroutine(insectApi.GetInsectInfo(raisingInsectId, OnSuccess, OnFailure));
        }
        else
        {
            Debug.LogError("InsectApi 인스턴스를 찾을 수 없습니다.");
        }
    }

    // 텍스트 요소 동적 생성 함수
    private TextMeshProUGUI CreateTextElement(string elementName)
    {
        GameObject textObject = new GameObject(elementName); // 새로운 게임 오브젝트 생성
        textObject.transform.SetParent(this.transform, false); // 현재 오브젝트(Canvas)에 자식으로 설정
        TextMeshProUGUI textMeshPro = textObject.AddComponent<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 추가
        textMeshPro.fontSize = 24; // 기본 폰트 크기 설정 (필요에 따라 조절 가능)
        textMeshPro.alignment = TextAlignmentOptions.Center; // 기본 정렬 설정
        return textMeshPro;
    }

    private void OnSuccess(InsectInfoResponse response)
    {
        insectNickname.text = response.nickname;
        insectName.text = response.insectName;
        feedCnt = response.feedCnt;
        interactCnt = response.interactCnt;

        if(response.areaType == "FOREST") {
            areaType.text = "숲";
        } else if (response.areaType == "WATER") {
            areaType.text = "연못";
        } else if (response.areaType == "GARDEN") {
            areaType.text = "꽃밭";
        }

        if(feedCnt < 2) {
            canEat = true;
        }

        if(interactCnt < 10) {
            canInteract = true;
        }

        Debug.Log("canEat = " + canEat);
        Debug.Log("canInteract = " + canInteract);
    }

    private void OnFailure(string error)
    {
        // 실패 시 오류 메시지 출력
        Debug.LogError("Failed to fetch insect info: " + error);
    }
}