
using Microsoft.AspNetCore.Mvc;


[Route("api/home")]
public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }

  /**
  * Define different method for specific function
  */
  [HttpGet]
  [HttpGet("index")]
  public string Index()
  {
    // Log which route was used
    _logger.LogInformation("Index method called at {Timestamp}", DateTime.Now);
    _logger.LogInformation("Successfully processed Index request");
    return "Hello world";
  }

  /**
  * Define different route will fall in same route
  */
  [Route("contact")]
  [Route("contact-us")]
  public string Contact()
  {
    // Log which route was used
    return "Contact page";
  }

  /**
  * How to use the _logger
  */
  [HttpGet("test")]
  public IActionResult Test()
  {
    // Log with structured logging (recommended)
    _logger.LogInformation("Test endpoint called with UserId: {UserId}", "user123");

    // Different log levels
    _logger.LogTrace("This is trace level - most detailed");
    _logger.LogDebug("This is debug level - for debugging info");
    _logger.LogInformation("This is info level - general information");
    _logger.LogWarning("This is warning level - something unexpected but not error");

    // Log with scope for better context
    using (_logger.BeginScope("Processing test request for {RequestId}", Guid.NewGuid()))
    {
      _logger.LogInformation("Inside scoped operation");

      // Simulate some work
      if (DateTime.Now.Millisecond % 2 == 0)
      {
        _logger.LogWarning("Random warning occurred");
      }
    }

    return Ok(new { message = "Test completed", timestamp = DateTime.Now });
  }

  [HttpGet("person")]
  public IActionResult PersonAPI()
  {
    return Ok(new Person { id = "1" });
    // action result return response
    // return BadRequest(new { error = "Bad request example" });
    // return NotFound(new { error = "Not found example" });
    // return Unauthorized(new { error = "Unauthorized example" });
    // return Forbid();
    // return NoContent();
    // 下面的能够控制返回的 code
    // return StatusCode(500, new { error = "Internal server error example" });
  }
  // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  // public IActionResult Error()
  // {
  //   return View("Error!");
  // }

  public class Person
  {
    public string id { set; get; }
    public string? name { set; get; }
  }
}
