using System.Text.Json;

namespace WebAPI.Errors
{
    public class ApiError
    {
        public ApiError()
        {

        }
        public ApiError(int errorCode, string errorMessage, string errorDetails = null)
        {
            ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            ErrorDetails = errorDetails;
        }

        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDetails { get; set; }

        //serialize object in json
        public override string ToString()

            
        {

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            //2nd argument says all properties should be serialized in camel case
            return JsonSerializer.Serialize(this,options);
        }
    }
}
