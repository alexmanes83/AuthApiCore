namespace AuthApiCore.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        // Construtor padrão necessário para deserialização
        public LoginModel() { 
        }
    }
}
