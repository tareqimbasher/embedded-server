using System.Net;

namespace EmbeddedServer
{
    public abstract class Controller
    {
        public HttpContext HttpContext { get; set; }

        public IActionResult NoContent() => new ActionResult(HttpStatusCode.NoContent);

        public IActionResult Ok() => new ActionResult(HttpStatusCode.NoContent);
        public IActionResult Ok(object data) => new ActionResult(HttpStatusCode.OK, data);

        public IActionResult BadRequest() => new ActionResult(HttpStatusCode.BadRequest);
        public IActionResult BadRequest(string message) 
            => new ActionResult(HttpStatusCode.BadRequest, new ErrorData(message));

        public IActionResult NotFound() => new ActionResult(HttpStatusCode.NotFound);
        public IActionResult NotFound(string message)
            => new ActionResult(HttpStatusCode.NotFound, new ErrorData(message));

        public IActionResult Forbidden() => new ActionResult(HttpStatusCode.Forbidden);
        public IActionResult Forbidden(string message)
            => new ActionResult(HttpStatusCode.Forbidden, new ErrorData(message));

        public IActionResult InternalServerError() => new ActionResult(HttpStatusCode.InternalServerError);
        public IActionResult InternalServerError(string message)
            => new ActionResult(HttpStatusCode.InternalServerError, new ErrorData(message));

        public IActionResult File(string filePath) => new FileResult(filePath);
        public IActionResult File(string fileName, byte[] fileData) => new FileResult(fileName, fileData);
        public IActionResult File(string fileName, byte[] fileData, string contentType)
            => new FileResult(fileName, fileData, contentType);
    }
}