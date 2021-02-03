namespace GameLibrary
{
    public class ApiResponse
    {
        public ApiResponse(bool wasSuccessful, string errorMessage, int userId)
        {
            this.WasSuccessful = wasSuccessful;
            this.ErrorMessage = errorMessage;
            this.UserId = UserId;
        }

        public ApiResponse()
        {
        }

        public bool WasSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public int UserId { get; set; }
    }
}
