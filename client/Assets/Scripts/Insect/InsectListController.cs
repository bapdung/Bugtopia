using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
      InsectInfo insect = i < insectList.Count ? insectList[i] : null;

      GameObject insectItem = new GameObject("InsectItem");
      insectItem.transform.SetParent(content, false);

      // RectTransform 설정
      RectTransform itemRectTransform = insectItem.AddComponent<RectTransform>();
      itemRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
      itemRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
      itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
      itemRectTransform.anchoredPosition = new Vector2((i - 1) * 220, 0);

      // 닉네임 텍스트 생성
      GameObject nicknameObject = new GameObject("NicknameText");
      nicknameObject.transform.SetParent(insectItem.transform, false);
      TextMeshProUGUI nicknameText = nicknameObject.AddComponent<TextMeshProUGUI>();

      if (insect != null)
      {
        // 곤충 정보가 있을 때 닉네임 설정
        nicknameText.text = insect.nickname;
        nicknameText.fontSize = 24;
        nicknameText.alignment = TextAlignmentOptions.Center;
        nicknameText.color = Color.black;

        // PNG 이미지 로드 및 표시
        string imagePath = $"InsectImages/{insect.family}";
        Sprite insectSprite = Resources.Load<Sprite>(imagePath);

        if (insectSprite != null)
        {
          GameObject imageObject = new GameObject("InsectImage");
          imageObject.transform.SetParent(insectItem.transform, false);
          Image imageComponent = imageObject.AddComponent<Image>();
          imageComponent.sprite = insectSprite;

          // RectTransform을 통해 크기와 위치 설정
          RectTransform imageRect = imageObject.GetComponent<RectTransform>();
          imageRect.sizeDelta = new Vector2(100, 100); // 1:1 비율 고정 크기
          imageRect.anchoredPosition = new Vector2(0, 8); //위치 조절함수

          Debug.Log($"{insect.nickname} PNG 로드 성공!");
        }
        else
        {
          Debug.LogError($"{insect.family} PNG 이미지를 찾을 수 없습니다.");
        }
      }
      else
      {
        // 곤충 정보가 없을 때 "빈 슬롯"으로 표시
        nicknameText.text = "빈 슬롯";
        nicknameText.fontSize = 24;
        nicknameText.alignment = TextAlignmentOptions.Center;
        nicknameText.color = Color.gray;

        GameObject emptyIcon = new GameObject("EmptyIcon");
        emptyIcon.transform.SetParent(insectItem.transform, false);
        TextMeshProUGUI iconText = emptyIcon.AddComponent<TextMeshProUGUI>();
        iconText.text = "+";
        iconText.fontSize = 48;
        iconText.alignment = TextAlignmentOptions.Center;
        iconText.color = Color.gray;

        RectTransform iconRect = emptyIcon.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(50, 50);
        iconRect.anchoredPosition = new Vector2(0, 8);
      }

      // 닉네임 텍스트 RectTransform 설정
      RectTransform nicknameRect = nicknameObject.GetComponent<RectTransform>();
      nicknameRect.sizeDelta = new Vector2(200, 50);
      nicknameRect.anchorMin = new Vector2(0.5f, 0.5f);
      nicknameRect.anchorMax = new Vector2(0.5f, 0.5f);
      nicknameRect.pivot = new Vector2(0.5f, 0.5f);
      nicknameRect.anchoredPosition = new Vector2(0, -50);
    }
  }
}