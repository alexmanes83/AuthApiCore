namespace AuthApiCore.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public RegisterModel(string mail, string password)
        {

            Email = mail;
            Password = password;

        }
    }
}
