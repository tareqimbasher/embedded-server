using EmbeddedServer.Hosting;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedServer
{
    public class ActionResult : IActionResult
    {
        public async Task WriteToResponseAsync(HttpListenerResponse response, IWebHostConfiguration configuration)
        {
            response.StatusCode = (int)HttpStatusCode;
            response.StatusDescription = HttpStatusCode.ToString();
            response.Headers.Add(HttpResponseHeader.ContentType, "application/json");

            string json = configuration.JsonSerializer.Serialize(this);

            var bytes = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = bytes.Length;
            response.OutputStream.Write(bytes, 0, bytes.Length);
        }


        public ActionResult()
        {
        }

        public ActionResult(HttpStatusCode statusCode)
        {
            HttpStatusCode = statusCode;
        }

        public ActionResult(HttpStatusCode status, object data) : this(status)
        {
            Data = data;
        }

        public ActionResult(HttpStatusCode status, ErrorData error) : this(status)
        {
            Error = error;
        }


        public HttpStatusCode HttpStatusCode { get; set; }
        public object Data { get; set; }
        public ErrorData Error { get; set; }

        public bool IsSuccessful => (int)HttpStatusCode >= 200 && (int)HttpStatusCode < 300;
        public bool IsNotSuccessful => !IsSuccessful;



        // Static Methods
        public static ActionResult Ok(object data = null)
            => new ActionResult(HttpStatusCode.OK, data);
        public static ActionResult Failed(ErrorData error = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ActionResult(statusCode, error) { HttpStatusCode = statusCode };
        public static ActionResult Failed(string errorMessage = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ActionResult(statusCode, new ErrorData(errorMessage));
        public static ActionResult NotFound(string errorMessage = null)
            => new ActionResult(HttpStatusCode.NotFound, new ErrorData(errorMessage));
        public static ActionResult Unauthorized(string errorMessage = null)
            => new ActionResult(HttpStatusCode.Unauthorized, new ErrorData(errorMessage));
        public static ActionResult Forbidden(string errorMessage = null)
            => new ActionResult(HttpStatusCode.Forbidden, new ErrorData(errorMessage));
    }

    //public class ResultDto<TData> : ResultDto
    //{
    //    public ResultDto()
    //    {
    //    }

    //    public ResultDto(ResultStatus status) : base(status)
    //    {
    //    }

    //    public ResultDto(ResultStatus status, TData data) : this(status)
    //    {
    //        Data = data;
    //    }

    //    public ResultDto(ResultStatus status, ErrorData error) : this(status)
    //    {
    //        Error = error;
    //    }

    //    public TData Data { get; set; }


    //    // Static Methods
    //    public static ResultDto<TData> Ok(TData data = default) => new ResultDto<TData>(ResultStatus.Ok, data) { HttpStatusCode = data == null ? HttpStatusCode.NoContent : HttpStatusCode.OK };
    //    public static ResultDto<TData> Failed(ErrorData error = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ResultDto<TData>(ResultStatus.Failed, error) { HttpStatusCode = statusCode };
    //    public static ResultDto<TData> Failed(string errorMessage = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest) => new ResultDto<TData>(ResultStatus.Failed, new ErrorData(errorMessage)) { HttpStatusCode = statusCode };
    //    public static ResultDto<TData> NotFound(ErrorData error = null) => new ResultDto<TData>(ResultStatus.NotFound, error) { HttpStatusCode = HttpStatusCode.NotFound };
    //    public static ResultDto<TData> Unauthorized(ErrorData error = null) => new ResultDto<TData>(ResultStatus.Unauthorized, error) { HttpStatusCode = HttpStatusCode.Unauthorized };
    //    public static ResultDto<TData> Forbidden(ErrorData error = null) => new ResultDto<TData>(ResultStatus.Forbidden, error) { HttpStatusCode = HttpStatusCode.Forbidden };
    //}
}