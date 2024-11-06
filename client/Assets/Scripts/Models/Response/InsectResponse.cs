namespace Models.Insect.Response
{
    [System.Serializable]
    public class InsectInfoResponse
    {
        public string nickname;
        public string insectName;
        public string family;
        public string areaType;
        public int feedCnt;

        public DateTime lastEat;
        public int interactCnt;
        public DateTime livingDate;
        public int continuousDays;
        public List<EventInfo> events;
    }

    [System.Serializable]
    public class EventInfo
    {
        public string eventType;
        public bool isClear;
    }

}
