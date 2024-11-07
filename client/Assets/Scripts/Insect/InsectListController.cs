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
            Debug.Log(region + " 곤충 목록을 불러옵니다 개수: " + response.num);
            PopulateInsectList(response.insectListList);
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

        // 새 리스트 아이템 생성 (nickname 텍스트만 표시)
        foreach (var insect in insectList)
        {
            Debug.Log(insect.nickname);
            GameObject newItem = Instantiate(insectItemPrefab, content);
            newItem.transform.Find("Nickname").GetComponent<Text>().text = insect.nickname;
        }
    }
}
