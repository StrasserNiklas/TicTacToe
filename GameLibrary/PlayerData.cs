namespace GameLibrary
{
    public class PlayerData
    {
        public PlayerData(string username, int wins)
        {
            this.Username = username;
            this.Wins = wins;
        }
        public PlayerData()
        {
        }

        public string Username { get; set; }

        public int Wins { get; set; }
    }
}
