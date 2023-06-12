using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    public JsonResult HandleError()
    {
        var errorMessage = "O recurso solicitado não está disponível.";

        return new JsonResult(new { errorMessage });
    }
}
