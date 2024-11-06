using System;

namespace Models.Insect.Response
{
    [System.Serializable]
    public class InsectInfoResponse
    {
        // public long raisingInsectId;
        // public string nickname;
        // public string insectName;
        // public string family;
        // public string areaType;
        // public int feedCnt;

        // public DateTime lastEat;
        // public int interactCnt;
        // public DateTime livingDate;
        // public int continuousDays;
        public string areaType;
        public int canFeed;
        public string nickname;
        public string insectName;
        public long insectId;
        public string livingDate;
        public long clearEvent;

    }

    [System.Serializable]
    public class EventInfo
    {
        public string eventType;
        public bool isClear;
    }

}
