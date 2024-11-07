using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Models.Insect.Response;
using API.Insect;

public class InsectListController : MonoBehaviour
{
    public Transform content;
    private InsectApi insectApi;

    private void Start()
    {
        if (insectApi == null)
        {
            GameObject insectApiObject = new GameObject("InsectApiObject");
            insectApi = insectApiObject.AddComponent<InsectApi>();
        }
    }

    public void LoadInsectList(string region)
    {
        StartCoroutine(GetInsectList(region));
    }

    private IEnumerator GetInsectList(string region)
    {
        yield return insectApi.GetInsectListWithRegion(region, response =>
        {
            Debug.Log(region + " 곤충 목록을 불러옵니다 개수: " + response.num);
            PopulateInsectList(response.insectList);
        },
        error =>
        {
            Debug.LogError("곤충 목록을 불러오는데 실패했습니다: " + error);
        });
    }

    private void PopulateInsectList(List<InsectInfo> insectList)
    {
        // 기존 리스트 클리어
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var insect in insectList)
        {
            Debug.Log(insect.nickname);

            // 새로운 TextMeshPro 텍스트 오브젝트 생성
            GameObject newTextObject = new GameObject("NicknameText");
            newTextObject.transform.SetParent(content);

            // TextMeshProUGUI 컴포넌트 추가
            TextMeshProUGUI nicknameText = newTextObject.AddComponent<TextMeshProUGUI>();
            nicknameText.text = insect.nickname;
            nicknameText.fontSize = 24;
            nicknameText.alignment = TextAlignmentOptions.Center;
            nicknameText.color = Color.black;

            // 위치 및 크기 조정
            RectTransform rectTransform = newTextObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.sizeDelta = new Vector2(200, 50); // 텍스트의 크기 설정
        }
    }
}
