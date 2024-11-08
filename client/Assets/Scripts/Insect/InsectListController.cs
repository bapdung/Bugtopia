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

    private void Awake()
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

        // 총 3개의 슬롯을 만들기 위한 반복문
        for (int i = 0; i < 3; i++)
        {
            // 곤충 데이터를 불러오거나, 데이터가 없으면 null 처리
            InsectInfo insect = i < insectList.Count ? insectList[i] : null;

            // 곤충 항목 오브젝트 생성
            GameObject insectItem = new GameObject("InsectItem");
            insectItem.transform.SetParent(content, false); // content 하위에 설정

            // RectTransform 추가 및 설정
            RectTransform itemRectTransform = insectItem.AddComponent<RectTransform>();
            itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f); // 중앙에 Anchor 설정
            itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            itemRectTransform.pivot = new Vector2(0.5f, 0.5f); // Pivot을 중앙으로 설정
            itemRectTransform.anchoredPosition = new Vector2((i - 1) * 220, 0); // 슬롯 간 간격 설정 (220 픽셀 간격)

            // 곤충 닉네임 텍스트 생성
            GameObject nicknameObject = new GameObject("NicknameText");
            nicknameObject.transform.SetParent(insectItem.transform, false);
            TextMeshProUGUI nicknameText = nicknameObject.AddComponent<TextMeshProUGUI>();

            if (insect != null)
            {
                // 곤충 정보가 있을 때
                nicknameText.text = insect.nickname;
                nicknameText.fontSize = 24;
                nicknameText.alignment = TextAlignmentOptions.Center;
                nicknameText.color = Color.black;

                // family 값에 따라 프리팹 로드하여 배치
                string prefabPath = $"Prefabs/{insect.family}";
                GameObject insectPrefab = Resources.Load<GameObject>(prefabPath);

                if (insectPrefab != null)
                {
                    GameObject insectModel = Instantiate(insectPrefab, insectItem.transform);
                    insectModel.transform.localScale = Vector3.one * 0.2f; // 모델 크기 조정
                    insectModel.transform.localPosition = new Vector3(0, 50, 0); // 모델 위치 조정

                    Debug.Log($"{insect.nickname} 프리팹 로드 성공!");
                }
                else
                {
                    Debug.LogError($"{insect.family} 프리팹을 찾을 수 없습니다.");
                }
            }
            else
            {
                // 곤충 정보가 없을 때 "빈 슬롯"으로 표시
                nicknameText.text = "빈 슬롯";
                nicknameText.fontSize = 24;
                nicknameText.alignment = TextAlignmentOptions.Center;
                nicknameText.color = Color.gray;

                // 빈 슬롯의 아이콘 추가 (원하는 이미지나 아이콘을 사용해도 됩니다)
                GameObject emptyIcon = new GameObject("EmptyIcon");
                emptyIcon.transform.SetParent(insectItem.transform, false);
                TextMeshProUGUI iconText = emptyIcon.AddComponent<TextMeshProUGUI>();
                iconText.text = "+"; // 간단한 아이콘으로 '+' 표시
                iconText.fontSize = 48;
                iconText.alignment = TextAlignmentOptions.Center;
                iconText.color = Color.gray;

                // 빈 슬롯 아이콘 위치 설정
                RectTransform iconRect = emptyIcon.GetComponent<RectTransform>();
                iconRect.localScale = Vector3.one;
                iconRect.sizeDelta = new Vector2(50, 50); // 아이콘 크기 설정
                iconRect.anchoredPosition = new Vector2(0, 50); // 아이콘 위치 설정
            }

            // 닉네임 텍스트 RectTransform 설정
            RectTransform nicknameRect = nicknameObject.GetComponent<RectTransform>();
            nicknameRect.localScale = Vector3.one;
            nicknameRect.sizeDelta = new Vector2(200, 50); // 텍스트의 크기 설정
            nicknameRect.anchorMin = new Vector2(0.5f, 0.5f); // 중앙에 Anchor 설정
            nicknameRect.anchorMax = new Vector2(0.5f, 0.5f);
            nicknameRect.pivot = new Vector2(0.5f, 0.5f);
            nicknameRect.anchoredPosition = new Vector2(0, -50); // 닉네임 텍스트 위치 조정
        }
    }
}