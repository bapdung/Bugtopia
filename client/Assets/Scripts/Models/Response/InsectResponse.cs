using System;
using System.Collections.Generic;

namespace Models.Insect.Response
{
    [System.Serializable]
    public class InsectArInfoResponse
    {
        public long raisingInsectId;
        public string nickname;
        public string family;
        public int feedCnt;
        public string lastEat;
        public int interactCnt;
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

    [System.Serializable]
    public class InsectDetailInfoResponse
    {
        public Info info;
        public LoveScore loveScore;
        public EventDetail eventDetail;
    }

    [System.Serializable]
    public class Info
    {
        public long raisingInsectId;
        public string nickname;
        public string insectName;
        public string family;
        public string areaType;
        public string livingDate;
    }

    [System.Serializable]
    public class LoveScore
    {
        public int total;
        public int feedCnt;
        public string lastEat;
        public int interactCnt;
    }

    [System.Serializable]
    public class EventDetail
    {
        public string endEvent;
        public bool isEvent;
        public string eventType;
    }
}
