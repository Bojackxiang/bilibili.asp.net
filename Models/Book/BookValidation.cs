using System.ComponentModel.DataAnnotations;


public class BookValidation : ValidationAttribute
{
  public override bool IsValid(object? value)
  {

    bool userType = value is UserModel;

    return true;
  }
}
