using System.Text.RegularExpressions;
namespace user_service.domain.ValueObject;

public class Email
{
    public string Value { get; private set; }
    public Email(string email)
    {
        string pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        Regex regex = new Regex(pattern);
        if (!regex.IsMatch(email))
        {
            throw new ArgumentException("Invalid email format");
        }
        else
        {
            Value = email;
        }
    }
}
    
    