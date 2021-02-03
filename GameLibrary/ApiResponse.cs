namespace GameLibrary
{
    public class ApiResponse
    {
        public ApiResponse(bool errorOccured, string errorMessage, int userId)
        {
            this.ErrorOccured = errorOccured;
            this.ErrorMessage = errorMessage;
            this.UserId = UserId;
        }

        public ApiResponse()
        {
        }

        public bool ErrorOccured { get; set; }

        public string ErrorMessage { get; set; }

        public int UserId { get; set; }
    }
}
