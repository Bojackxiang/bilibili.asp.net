using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FirstProject.Controllers
{
  [Route("[controller]")]
  public class BookController : ControllerBase
  {
    private readonly ILogger<BookController> _logger;

    public BookController(ILogger<BookController> logger)
    {
      _logger = logger;
    }

    [HttpPost("bookstore3/new")]
    public IActionResult Bookstore3([FromBody] BookModel newBook)
    {
      if (!ModelState.IsValid)
      {
        var errors = ModelState
                   .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                   .ToDictionary(
                       kvp => kvp.Key,
                       kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                   );

        _logger.LogWarning("BookStore3 - 模型验证失败: {Errors}", string.Join(", ", errors.Keys));
        return BadRequest(new { message = "验证失败", errors });
      }


      List<string> bookNames = newBook.bookNames;

      return Ok(bookNames);
    }
  }
}
