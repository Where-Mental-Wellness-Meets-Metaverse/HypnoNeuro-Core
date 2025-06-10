using System;

namespace DataModel
{

    public class UserData
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarId { get; set; }
        private double tokens;
        public string Nfts { get; set; } // JSON string containing NFT details
        public string ProgressId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Action<double> OnUpdateToken;

        public double Tokens
        {
            get => tokens;
            set
            {
                tokens = value;
                OnUpdateToken?.Invoke(tokens);
            }
        }

        public UserData(string userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            Username = "Guest";
            Email = "guest@example.com";
            AvatarId = "default";
            Tokens = 1000;
            Nfts = "{}";
            ProgressId = "0";
            OnUpdateToken = null;
        }

       
    }
    [System.Serializable]
    public class Game
    {
        public string gameName;
        public SceneName sceneName;
        public double tokenPrice;
    }
}