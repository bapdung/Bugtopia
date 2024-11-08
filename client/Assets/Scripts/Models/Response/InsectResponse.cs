using System;
using System.Collections.Generic;

namespace Models.Insect.Response
{
    [System.Serializable]
    public class InsectInfoResponse
    {
        public long raisingInsectId;
        public string nickname;
        public string insectName;
        public string family;
        public string areaType;
        public int feedCnt;
        public string lastEat;
        public int interactCnt;
        public DateTime livingDate;
        public int continuousDays;
        public int loveScore;
        public bool isEvent;
        public string eventType;
    }

    [System.Serializable]
    public class EventInfo
    {
        public string eventType;
        public bool isClear;
    }

    [System.Serializable]
    public class IncreaseScoreResponse
    {
        public long loveScore;
        public bool isEvent;
        public string eventType;
    }

    [System.Serializable]
    public class InsectListWithRegionResponse
    {
        public int num;
        public List<InsectInfo> insectList;
    }

    [System.Serializable]
    public class InsectInfo
    {
        public string family;
        public long raisingInsectId;
        public string nickname;
    }

    [System.Serializable]
    public class SearchUnsectResponse
    {
        public int insectId;
        public string krName;
        public string engName;
        public string info;
        public int canRaise;
        public string family;
        public string area;
        public string rejectedReason;
    }
    [System.Serializable]
    public class S3Response
    {
        public S3Data data;
    }
    [System.Serializable]
    public class S3Data
    {
        public string path;
    }
}
