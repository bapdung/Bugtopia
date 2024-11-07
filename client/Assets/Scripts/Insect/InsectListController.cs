using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Models.Insect.Response;
using API.Insect;

public class InsectListController : MonoBehaviour
{
    public Transform content;
    public GameObject insectItemPrefab;
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
        Debug.Log( region + "곤충 목록을 불러옵니다 개수: " + response.num);
        foreach (InsectInfo child in response.insectList)
        {
            Debug.Log(child.family + " " + child.raisingInsectId + " " + child.nickname);
        }
            PopulateInsectList(response.insectList);
        },
        error =>
        {
            Debug.LogError("곤충 목록을 불러오는데 실패했습니다: " + error);
        });
    }

    // 리스트 아이템 생성
    private void PopulateInsectList(List<InsectInfo> insectList)
    {
        // 기존 리스트 클리어
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 새 리스트 아이템 생성
        foreach (var insect in insectList)
        {
            Debug.Log( insect.family);
            GameObject newItem = Instantiate(insectItemPrefab, content);
            newItem.transform.Find("Nickname").GetComponent<Text>().text = insect.nickname;

            // family 값에 따라 프리팹을 동적으로 로드하여 이미지 설정
            string prefabPath = $"Prefabs/{insect.family}";
            GameObject insectPrefab = Resources.Load<GameObject>(prefabPath);

            if (insectPrefab != null)
            {
                newItem.transform.Find("InsectImage").GetComponent<Image>().sprite = insectPrefab.GetComponent<SpriteRenderer>().sprite;
                Debug.Log($"{insect.nickname} 프리팹 로드 성공!");
            }
            else
            {
                Debug.LogError($"{insect.family} 프리팹을 찾을 수 없습니다.");
            }
        }
    }
}