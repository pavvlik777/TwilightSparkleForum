namespace TwilightSparkle.Forum.DataContracts
{
    public class ApiErrorDataContract
    {
        public string ErrorCode { get; }


        public ApiErrorDataContract(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}