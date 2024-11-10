namespace Models.User.Request
{
    [System.Serializable]
    public class UserJoinRequest
    {
        public string deviceId;
        public string nickname;
    }

    [System.Serializable]
    public class UserLoginRequest
    {
        public string deviceId;
    }
}
