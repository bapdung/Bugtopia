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
}
