using UnityEngine;

public class UserStateManager : MonoBehaviour
{
    private static UserStateManager _instance;
    public static UserStateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserStateManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<UserStateManager>();
                    singletonObject.name = typeof(UserStateManager).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    public long UserId { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        // userId를 하드코딩하여 초기화
        UserId = 1;
    }
}