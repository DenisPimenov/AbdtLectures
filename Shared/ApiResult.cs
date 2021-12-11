namespace Shared
{
    public class ApiResult
    {
        public bool Success { get; set; }

        public int Code { get; set; }

        public string Error { get; set; }

        public static ApiResult Ok()
        {
            return new() { Success = true };
        }
    }
}