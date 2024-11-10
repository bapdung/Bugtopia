namespace Models.Insect.Request
{
    [System.Serializable]
    public class IncreaseScoreRequest
    {
        public long raisingInsectId;
        public int category;
    }

    [System.Serializable]
    public class SearchInsectRequest
    {
        public string photoUrl;
    }

    [System.Serializable]
    public class InsectNicknameRequest
    {
        public long insectId;
        public string nickname;
    }
}
