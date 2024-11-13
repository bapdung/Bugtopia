using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using API.Catch;
using Models.InsectBook.Response;

public class InsectBookController : MonoBehaviour
{
    private EnvironmentConfig environmentConfig;

    private CatchApi catchApi;

    public GameObject insectItemPrefab;
    public Transform catchPanel;

    private void Awake()
    {
        if (catchApi == null)
        {
            GameObject catchApiObject = new GameObject("CatchApiObject");
            catchApi = catchApiObject.AddComponent<CatchApi>();
        }

        if (environmentConfig == null)
        {
            environmentConfig = Resources.Load<EnvironmentConfig>("EnvironmentConfig");
        }
    }

    private void Start()
    {
        if (catchApi != null)
        {
            StartCoroutine(catchApi.GetCatchInsectList(OnSuccess, OnFailure));
        }
        else
        {
            Debug.LogError("CatchApi 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnSuccess(CatchListResponse response)
    {
        // CatchList에 있는 항목들을 Prefab에 표시
        foreach (var catchItem in response.catchList)
        {
            GameObject insectItem = Instantiate(insectItemPrefab, catchPanel);
            
            // 이미지와 텍스트 컴포넌트에 데이터 바인딩
            TextMeshProUGUI insectNameText = insectItem.transform.Find("InsectNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI catchedDateText = insectItem.transform.Find("CatchedDateText").GetComponent<TextMeshProUGUI>();
            Image insectImage = insectItem.transform.Find("InsectImage").GetComponent<Image>();

            insectNameText.text = catchItem.insectName;
            catchedDateText.text = catchItem.catchedDate.Split(' ')[0].Replace("-",". ");
            StartCoroutine(LoadImageFromURL(catchItem.photo, insectImage));
        }

        // EggList에 있는 항목들도 Prefab에 표시
        foreach (var eggItem in response.eggList)
        {
            GameObject eggItemObj = Instantiate(insectItemPrefab, catchPanel);
            
            // 이미지와 텍스트 컴포넌트에 데이터 바인딩
            TextMeshProUGUI eggNameText = eggItemObj.transform.Find("InsectNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI receiveDateText = eggItemObj.transform.Find("CatchedDateText").GetComponent<TextMeshProUGUI>();
            Image eggImage = eggItemObj.transform.Find("InsectImage").GetComponent<Image>();

            eggNameText.text = eggItem.eggName;
            receiveDateText.text = eggItem.receiveDate.Split(' ')[0].Replace("-",". ");
            StartCoroutine(LoadImageFromURL($"{environmentConfig.s3Url}/admin/eggImg.png", eggImage));
        }
    }

    private IEnumerator LoadImageFromURL(string url, Image imageComponent)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            }
            else
            {
                Debug.LogError($"Failed to load image from URL: {url}");
            }
        }
    }

    private void OnFailure(string error)
    {
        Debug.LogError("Failed to fetch insect info: " + error);
    }
}