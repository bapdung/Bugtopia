using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using API.Catch;
using Models.InsectBook.Response;

public class BookDetailController : MonoBehaviour
{
    private CatchApi catchApi;

    [SerializeField] private Image catchImage;

    [SerializeField] private Image canRaise;
    [SerializeField] private TextMeshProUGUI canRaiseText;

    [SerializeField] private TextMeshProUGUI krName;
    [SerializeField] private TextMeshProUGUI engName;
    
    [SerializeField] private TextMeshProUGUI info;

    [SerializeField] private GameObject possibleArea;
    [SerializeField] private GameObject impossibleArea;

    [SerializeField] private Image insectModelImg;
    [SerializeField] private Image area;
    [SerializeField] private TextMeshProUGUI areaName;

    [SerializeField] private TextMeshProUGUI rejectedReasonText;

    [SerializeField] private Image raisBtn;
    [SerializeField] private TextMeshProUGUI raiseBtnText;
    [SerializeField] private TextMeshProUGUI deleteBtnText;

    void Awake()
    {
        if (catchApi == null)
        {
            GameObject catchApiObject = new GameObject("CatchApiObject");
            catchApi = catchApiObject.AddComponent<CatchApi>();
        }
    }

    private void Start()
    {
        int insectId = 1;       // => 하드 코딩 (추후수정)

        if (catchApi != null)
        {
            StartCoroutine(catchApi.GetBookDetail(insectId, OnSuccess, OnFailure));
        }
        else
        {
            Debug.LogError("CatchApi 인스턴스를 찾을 수 없습니다.");
        }
    }

    private void OnSuccess(BookDetailResponse response)
    {
        StartCoroutine(LoadImageFromURL(response.imgUrl, catchImage));
        krName.text = response.krName;
        engName.text = response.engName;
        info.text = response.info;
        
        if(response.canRaise == 1) {
            canRaise.color = new Color(242f / 255f, 107f / 255f, 118f / 255f);
            canRaiseText.text = "입국 거절";
            possibleArea.SetActive(false);
            impossibleArea.SetActive(true);
            rejectedReasonText.text = response.rejectedReason;

            // 육성 버튼
            raisBtn.color = new Color(252f / 255f, 195f / 255f, 136f / 255f);
            raiseBtnText.text = "입국이 거절되면 키울 수 없어요";

            deleteBtnText.text = "다른 곤충 촬영하기";
        } else {
            canRaise.color = new Color(83f / 255f, 190f / 255f, 106f / 255f);
            canRaiseText.text = "입국 승인";
            possibleArea.SetActive(true);
            impossibleArea.SetActive(false);

            // 육성 버튼
            raisBtn.color = new Color(255f / 255f, 143f / 255f, 28f / 255f);
            raiseBtnText.text = "육성 시작하기";
        }

        if(response.area == "FOREST") {
            areaName.text = "숲";
            area.color = new Color(41f / 255f, 157f / 255f, 89f / 255f);
        } else if (response.area == "WATER") {
            areaName.text = "연못";
            area.color = new Color(55f / 255f, 102f / 255f, 230f / 255f);
        } else if (response.area == "GARDEN") {
            areaName.text = "꽃밭";
            area.color = new Color(209f / 255f, 51f / 255f, 235f / 255f);
        }

        if(response.family == "Beetle") {
            Sprite sprite = Resources.Load<Sprite>("InsectImages/Beetle");
            insectModelImg.sprite = sprite;
        } else if (response.family == "Mantis") {
            Sprite sprite = Resources.Load<Sprite>("InsectImages/Mantis");
            insectModelImg.sprite = sprite;
        } else if (response.family == "Stagbeetle") {
            Sprite sprite = Resources.Load<Sprite>("InsectImages/Stagbeetle");
            insectModelImg.sprite = sprite;
        } else if (response.family == "Tarantula") {
            Sprite sprite = Resources.Load<Sprite>("InsectImages/Tarantula");
            insectModelImg.sprite = sprite;
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