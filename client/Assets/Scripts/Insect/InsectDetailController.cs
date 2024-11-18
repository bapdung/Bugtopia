using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using API.Insect;
using Models.Insect.Response;

public class InsectDetailController : MonoBehaviour
{

  private Coroutine countdownCoroutine;

  private InsectApi insectApi; // InsectApi 인스턴스 참조
  private DateTime lastEatTime;

  [SerializeField] private Image areaContainer;
  [SerializeField] private TextMeshProUGUI area;

  [SerializeField] private TextMeshProUGUI feedInfo1;
  [SerializeField] private TextMeshProUGUI feedInfo2;

  [SerializeField] private TextMeshProUGUI insectNickname;
  [SerializeField] private TextMeshProUGUI insectName;

  [SerializeField] private Image interactContainer;
  [SerializeField] private TextMeshProUGUI interactContainerText;

  [SerializeField] private Image feedContainer;
  [SerializeField] private TextMeshProUGUI feedContainerText;

  [SerializeField] private TextMeshProUGUI LoveScoreText;

  [SerializeField] private Image LineStart;
  [SerializeField] private Image LineQ1toQ2;
  [SerializeField] private Image LineQ2toQ3;
  [SerializeField] private Image LineQ3toQ4;
  [SerializeField] private Image LineQ4toQ5;
  [SerializeField] private Image LineEnd;

  [SerializeField] private Image CircleQ1;
  [SerializeField] private Image CircleQ2;
  [SerializeField] private Image CircleQ3;
  [SerializeField] private Image CircleQ4;
  [SerializeField] private Image CircleQ5;

  [SerializeField] private TextMeshProUGUI eventInfoText;

  [SerializeField] private TextMeshProUGUI livingDate;
  [SerializeField] private TextMeshProUGUI ddayText;

  [SerializeField] private Button ARBtn;

  [SerializeField] private TextMeshProUGUI TextQ1;
  [SerializeField] private TextMeshProUGUI TextQ2;
  [SerializeField] private TextMeshProUGUI TextQ3;
  [SerializeField] private TextMeshProUGUI TextQ4;
  [SerializeField] private TextMeshProUGUI TextQ5;
  public Image insectImagePanel;
  private Color defaultTextColor; // 퀘스트 기본 텍스트 색상
  private int feedCnt;        // 오늘 먹이를 먹은 횟수
  private int interactCnt;    // 오늘 상호작용한 횟수
  private string family;
  private Image[] lineImages;
  private Image[] circleImages;
  private TextMeshProUGUI[] eventTexts;
  public Button returnButton;
  
  public void onClickReturnButton()
  {
    SceneManager.LoadScene("MainScene");
  }
  public void onClickArButton()
  {
    SceneManager.LoadScene("ARScene");
  }
  
  void Awake()
  {
    if (returnButton != null)
    {
        returnButton.onClick.AddListener(onClickReturnButton);
    }
    if (ARBtn != null)
    {
        ARBtn.onClick.AddListener(onClickArButton);
    }
    // insectApi가 할당되지 않았을 경우 코드 내에서 생성
    if (insectApi == null)
    {
      GameObject insectApiObject = new GameObject("InsectApiObject");  // 새 GameObject 생성
      insectApi = insectApiObject.AddComponent<InsectApi>();  // InsectApi 컴포넌트를 추가하여 할당
    }

    lineImages = new Image[] { LineStart, LineQ1toQ2, LineQ2toQ3, LineQ3toQ4, LineQ4toQ5, LineEnd };
    circleImages = new Image[] { CircleQ1, CircleQ2, CircleQ3, CircleQ4, CircleQ5 };
    eventTexts = new TextMeshProUGUI[] { TextQ1, TextQ2, TextQ3, TextQ4, TextQ5 };

  }

  private void Start()
  {
    int tempRaisingInsectId = PlayerPrefs.GetInt("raisingInsectId", 1);
    long raisingInsectId = (long)(tempRaisingInsectId);
    defaultTextColor = TextQ1.color;

    if (insectApi != null)
    {
      StartCoroutine(insectApi.GetInsectInfo(raisingInsectId, OnSuccess, OnFailure));
    }
    else
    {
      Debug.LogError("InsectApi 인스턴스를 찾을 수 없습니다.");
    }
  }

  private void OnSuccess(InsectDetailInfoResponse response)
  {
    insectNickname.text = response.info.nickname;
    insectName.text = response.info.insectName;
    LoveScoreText.text = $"{response.loveScore.total}";
    family = response.info.family;

    string dateString = response.info.livingDate.Split("T")[0];
    livingDate.text = "만난 날짜 : " + dateString.Replace("-", ". ");

    DateTime createdDate = DateTime.ParseExact(dateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    TimeSpan dateDifference = DateTime.Now - createdDate;
    int daysDifference = dateDifference.Days;
    ddayText.text = "당신을 만난지 " + daysDifference + "일째";

    feedCnt = response.loveScore.feedCnt;
    feedContainerText.text = "젤리주기 ( " + feedCnt + "/5)";

    interactCnt = response.loveScore.interactCnt;
    interactContainerText.text = "쓰다듬기 (" + interactCnt + "/10)";

    SetEventTextColor(response.nextEventInfo.nextEvent);

    if (response.info.areaType == "FOREST")
    {
      area.text = "숲";
      areaContainer.color = new Color(41f / 255f, 157f / 255f, 89f / 255f);
    }
    else if (response.info.areaType == "WATER")
    {
      area.text = "연못";
      areaContainer.color = new Color(55f / 255f, 102f / 255f, 230f / 255f);
    }
    else if (response.info.areaType == "GARDEN")
    {
      area.text = "꽃밭";
      areaContainer.color = new Color(209f / 255f, 51f / 255f, 235f / 255f);
    }

    if (feedCnt >= 5)
    {
      feedInfo1.text = "오늘은 다 먹었어요";
      feedInfo2.text = "먹이는 하루에 5번만 줘요";
      feedContainer.color = new Color(255f / 255f, 143f / 255f, 28f / 255f, 0.14f);
    }
    else
    {
      feedContainer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0.07f);

      if (DateTime.TryParse(response.loveScore.lastEat, out lastEatTime))
      {
        StartCountdown();
      }
    }

    if (interactCnt >= 10)
    {
      interactContainer.color = new Color(255f / 255f, 143f / 255f, 28f / 255f, 0.14f);
    }
    else
    {
      interactContainer.color = new Color(0f / 255f, 0f / 255f, 0f / 255f, 0.07f);
    }

    string nextEvent = response.nextEventInfo.nextEvent;
    if (nextEvent == "END")
    {
      eventInfoText.text = "축하합니다! 모든 이벤트를 클리어 했습니다!";
    }
    else if (nextEvent == "MARRY")
    {
      eventInfoText.text = "최종 전투 이벤트가 열렸습니다!";
    }
    else
    {
      int remainScore = response.nextEventInfo.remainScore;
      if (remainScore <= 0)
      {
        eventInfoText.text = "다음 필요 애정도를 보려면 전투 클리어가 필요합니다";
      }
      else
      {
        eventInfoText.text = "다음 이벤트까지 애정도 " + remainScore + "이 더 필요합니다";
      }
    }

    int endPoint = 5;
    if (nextEvent == "FOOD_C1")
    {
      endPoint = 0;
    }
    else if (nextEvent == "TERITORY_C1")
    {
      endPoint = 1;
    }
    else if (nextEvent == "FOOD_C2")
    {
      endPoint = 2;
    }
    else if (nextEvent == "TERITORY_C2")
    {
      endPoint = 3;
    }
    else if (nextEvent == "MARRY")
    {
      endPoint = 4;
    }
    else if (nextEvent == "END")
    {
      endPoint = 5;
    }

    for (int i = 0; i <= endPoint; i++)
    {
      lineImages[i].color = new Color(255f / 255f, 143f / 255f, 28f / 255f);

      if (i != 0)
      {
        circleImages[i - 1].color = new Color(255f / 255f, 143f / 255f, 28f / 255f);
      }
    }

    SetInsectImage(family);
  }

  private void SetInsectImage(string family)
  {
      Sprite insectSprite = Resources.Load<Sprite>("InsectImages/" +  family.Replace(" ", ""));
      if (insectSprite!= null)
      {
          insectImagePanel.sprite = insectSprite;
      }
  }
  
  private void SetEventTextColor(string nextEvent)
  {
    foreach (var text in eventTexts)
    {
      text.color = defaultTextColor;
    }

    switch (nextEvent)
    {
      case "FOOD_C1":
        TextQ1.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        break;
      case "TERITORY_C1":
        TextQ2.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        break;
      case "FOOD_C2":
        TextQ3.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        break;
      case "TERITORY_C2":
        TextQ4.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        break;
      case "MARRY":
        TextQ5.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        break;
    }
  }
  private void OnFailure(string error)
  {
    // 실패 시 오류 메시지 출력
    Debug.LogError("Failed to fetch insect info: " + error);
  }

  private void StartCountdown()
  {
    // 기존에 진행 중이던 카운트다운을 중지하고 새로 시작
    if (countdownCoroutine != null)
    {
      StopCoroutine(countdownCoroutine);
    }
    countdownCoroutine = StartCoroutine(UpdateCountdown());
  }

  private IEnumerator UpdateCountdown()
  {
    while (true)
    {
      TimeSpan timeDifference = DateTime.Now - lastEatTime;
      TimeSpan remainingTime = TimeSpan.FromHours(6) - timeDifference;

      if (remainingTime.TotalSeconds <= 0)
      {
        feedInfo1.text = "오늘의 먹이 주기";
        feedInfo2.text = "지금 먹이를 줄 수 있어요!";
        break;
      }
      else
      {
        feedInfo1.text = "지금은 배가 불러요";
        feedInfo2.text = "다음 밥 시간 - " + string.Format("{0:D2}:{1:D2}:{2:D2}",
            remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
      }

      yield return new WaitForSeconds(1);
    }
  }

}