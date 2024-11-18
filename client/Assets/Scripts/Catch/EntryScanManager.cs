// Scripts/Catch/EntryScanManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용
using Models.Insect.Response;
using Models.Insect.Request;
using API.Catch;
using UnityEngine.Networking;
using System.Collections;

public class EntryScanManager : MonoBehaviour
{
    public CatchApi catchApi;

    public GameObject NicknamePanel; // NicknamePanel 객체 참조
    public TextMeshProUGUI insectNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI familyText;
    public TextMeshProUGUI areaText;
    public TextMeshProUGUI rejectedReasonText;
    public Button StartRaisingButton;
    public Image InsectCharaterImagePanel;
    public Image cameraImagePanel;

    public TMP_InputField nicknameInputText;
    public Button nicknameSubmitButton;
    private string nickname;
    private int canRaiseInsect;
    private long insectId;

    private void Awake()
    {
        if (catchApi == null)
        {
            GameObject catchApiObject = new GameObject("CatchApiObject");  // 새 GameObject 생성
            catchApi = catchApiObject.AddComponent<CatchApi>();  // catchApi 컴포넌트를 추가하여 할당
        }
    }
    private void Start()
    {
        if (NicknamePanel != null)
        {
            NicknamePanel.SetActive(false); 
        }

        if (StartRaisingButton != null)
        {
            StartRaisingButton.onClick.AddListener(OnStartRaisingButtonClick);
        }
    }

    public void UpdateInsectInfo(SearchInsectResponse response)
    {
        canRaiseInsect = response.canRaise;
        insectId = response.insectId;

        insectNameText.text = "곤충명 (한글): " + response.krName + "\n" + "곤충명 (영어): " + response.engName;
        familyText.text = "곤충 종류: " + response.family;
        areaText.text = "서식지: " + response.area;
        rejectedReasonText.text = response.rejectedReason != null ? "육성 불가능 사유: " + response.rejectedReason : "곤충 정보: " + response.info;
        Debug.Log(response);

        SetInsectImage(response.family);

        setCameraImage(response.imgUrl);

        if (canRaiseInsect == 1)
        {
            StartRaisingButton.interactable = false;
            StartRaisingButton.GetComponentInChildren<TextMeshProUGUI>().text = "입국거절";
        }
        else
        {
            StartRaisingButton.interactable = true;
            StartRaisingButton.GetComponentInChildren<TextMeshProUGUI>().text = "육성 시작";
        }
    }
    
    private void SetInsectImage(string family)
    {
        Sprite insectSprite = Resources.Load<Sprite>("InsectImages/" + family);
        if (insectSprite!= null)
        {
            InsectCharaterImagePanel.sprite = insectSprite;
        }
    }

    private void setCameraImage(string imgUrl)
    {
        StartCoroutine(DownloadImage(imgUrl));
    }    

    private IEnumerator DownloadImage(string imgUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imgUrl))
        {
            yield return request.SendWebRequest();

            Texture2D texture = DownloadHandlerTexture.GetContent(request);

            Sprite downloadedSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            cameraImagePanel.sprite = downloadedSprite;
            Destroy(texture);
        }
    }


    public void OnStartRaisingButtonClick()
    {
        NicknamePanel.SetActive(true);

        if (canRaiseInsect == 2)
        {
            // 슬롯 가득 찼음을 알리는 메시지 설정
            nicknameInputText.gameObject.SetActive(false); // 닉네임 입력 필드는 숨김
            nicknameSubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "메인으로 돌아가기";
            rejectedReasonText.text = "육성 슬롯이 가득 찼습니다. 더 이상 곤충을 키울 수 없습니다.";

            // 버튼 클릭 시 MainScene으로 이동
            nicknameSubmitButton.onClick.RemoveAllListeners();
            nicknameSubmitButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("InsectBook");
            });
        }
        else
        {
            // 육성 가능한 경우 닉네임 패널을 정상적으로 표시
            nicknameInputText.gameObject.SetActive(true);
            nicknameSubmitButton.GetComponentInChildren<TextMeshProUGUI>().text = "닉네임 등록";

            // 기존 닉네임 제출 기능을 연결
            nicknameSubmitButton.onClick.RemoveAllListeners();
            nicknameSubmitButton.onClick.AddListener(OnNicknameSubmit);
        }
    }

    public void OnNicknameSubmit()
    {
        long userId = UserStateManager.Instance.UserId; 
        nickname = nicknameInputText.text; 

        InsectNicknameRequest requestBody = new InsectNicknameRequest{
            nickname = nickname,
            insectId = insectId
        };

        if (!string.IsNullOrEmpty(nickname))
        {
            StartCoroutine(catchApi.PostInsectNickname(userId, requestBody, (response) =>
            {
                if (response != null)
                {
                    Debug.Log("닉네임이 성공적으로 전송되었습니다.");
                    Debug.Log(response.raisingInsectId);
                    
                    PlayerPrefs.SetInt("raisingInsectId", (int)response.raisingInsectId); 
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("InsectDetailScene");
                }
                else
                {
                    Debug.LogError("닉네임 전송에 실패했습니다.");
                }
            }));
        }
        else
        {
            Debug.LogWarning("닉네임이 비어 있습니다.");
        }
    }
}
