namespace Certamen_3.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public Response()
        {
            Success = false;
        }

    }
}
