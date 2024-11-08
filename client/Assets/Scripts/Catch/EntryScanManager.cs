// Scripts/Catch/EntryScanManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용
using Models.Insect.Response;
using Models.Insect.Request;
using API.Catch;

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

    public TMP_InputField nicknameInputText;
    public Button nicknameSubmitButton;
    private string nickname;

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
        insectNameText.text = "곤충명 (한글): " + response.krName + "\n" + "곤충명 (영어): " + response.engName;
        familyText.text = "곤충 종류: " + response.family;
        areaText.text = "서식지: " + response.area;
        rejectedReasonText.text = response.rejectedReason != null ? "육성 불가능 사유: " + response.rejectedReason : "곤충 정보: " + response.info;
    }

    public void OnStartRaisingButtonClick()
    {
        NicknamePanel.SetActive(true);
        // StartCoroutine(catchApi.PostCatch((success) =>
        // {
        //     if (success)
        //     {
        //         NicknamePanel.SetActive(true); 
        //     }
        // }));
    }

    public void OnNicknameSubmit()
    {
        long userId = 1L; // 예시 userId
        long insectId = 6L; // 예시 insectId
        
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
