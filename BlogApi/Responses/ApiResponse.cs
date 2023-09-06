namespace BlogApi.Responses
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResponse(object data = null, string errorMessage = null)
        {
            ErrorMessage = errorMessage;
            Data = data;
            Success = ErrorMessage is null;
        }

    }
}
