using System;
using System.Collections.Generic;

namespace Models.User.Response
{
    [System.Serializable]
    public class UserLoginResponse
    {
        public bool isJoined;
        public long? userId; // isJoined가 true일 때만 값이 설정됨
        public string nickname; // isJoined가 true일 때만 값이 설정됨
    }

    [System.Serializable]
    public class UserJoinResponse
    {
        public long userId;
        public string nickname;
    }

}
