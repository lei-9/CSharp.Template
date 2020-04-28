namespace CSharp.Template.WebApi.Model
{
    public class BaseResponse
    {
        public BaseResponse(bool success, string msg, int statusCode)
        {
            Success = success;
            Msg = msg;
            StatusCode = statusCode;
        }

        public bool Success { get; set; }
        public int StatusCode { get; set; }

        public string Msg { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public BaseResponse(bool success, T data, string msg, int statusCode) : base(success, msg, statusCode)
        {
            Data = data;
        }
        public T Data { get; set; }
    }
}