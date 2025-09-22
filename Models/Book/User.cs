using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class UserModel
{
  [FromRoute(Name = "userId")]
  [Range(1, int.MaxValue, ErrorMessage = "用户ID必须大于0")]
  public int UserId { get; set; }

  [FromRoute(Name = "username")]
  [Required(ErrorMessage = "用户名不能为空")]
  [StringLength(20, MinimumLength = 3, ErrorMessage = "用户名长度必须在3-20字符之间")]
  public string Username { get; set; } = string.Empty;

}
