namespace AuthApiCore.Models
{
    public class CustomUser
    {
        public int Id { get; set; } // ID do usuário
        public string Login { get; set; } // Nome de login
        public string Password { get; set; } // Senha

        public CustomUser()
        {
        }
    }
}
