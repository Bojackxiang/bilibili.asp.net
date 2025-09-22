using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class BookModel
{
  [Required(ErrorMessage = "Book name is required ")]
  [MinLength(1, ErrorMessage = "User must provide at least one book name")]
  public List<string> bookNames { get; set; } = new();

}
