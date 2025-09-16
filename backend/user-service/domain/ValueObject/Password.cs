    namespace user_service.domain.ValueObject;

    using System.Text.RegularExpressions;
    public class Password
    {
        public string Value { get; private set; }
        public Password (string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$";;
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(password))
            {
                throw new ArgumentException("Invalid password format");
            }
            else
            {
                Value = password;
            }
        }
    }