using System;
using System.Collections.Generic;

namespace Models.InsectBook.Response
{
    [System.Serializable]
    public class CatchListResponse
    {
        public int catchedInsectCnt;
        public int eggCnt;
        public CatchList[] catchList;
        public EggList[] eggList;
    }

    [System.Serializable]
    public class CatchList
    {
        public int catchedInsectId;
        public string insectName;
        public string photo;
        public string catchedDate;
    }

    [System.Serializable]
    public class EggList
    {
        public int eggId;
        public string eggName;
        public string receiveDate;
    }
}