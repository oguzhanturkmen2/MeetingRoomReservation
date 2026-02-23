namespace MeetingRoomReservation.Api.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();

        public static ApiResponse Ok(object? data, string message = "İşlem başarılı")
        {
            return new ApiResponse
            {
                Success = true,
                Data = data,
                Message = message,
                Errors = new List<string>()
            };
        }

        public static ApiResponse Error(string message, List<string>? errors = null)
        {
            return new ApiResponse
            {
                Success = false,
                Data = null,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }
    }


}
