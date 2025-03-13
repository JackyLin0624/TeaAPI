namespace TeaAPI.Models.Responses
{
    public class ResponseBase
    {
        public int ResultCode { get; set; }
        public List<string> Errors { get; set; }
    }
}
