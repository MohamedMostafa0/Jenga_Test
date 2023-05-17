namespace JengaTest.Models
{
    public class ErrorModel
    {
        public int Code { get; private set; }
        public string Error { get; private set; }
        public ErrorModel(int code, string error)
        {
            Code = code;
            Error = error;
        }
    }
}
