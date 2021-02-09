namespace GameLibrary
{
    public class ApiResponse
    {
        public ApiResponse(bool wasSuccessful, string errorMessage, int userId, string token = "")
        {
            this.WasSuccessful = wasSuccessful;
            this.ErrorMessage = errorMessage;
            this.UserId = userId;
            this.JwToken = token;
        }

        public ApiResponse()
        {
        }

        public bool WasSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public int UserId { get; set; }

        public string JwToken { get; set; }
    }
}
