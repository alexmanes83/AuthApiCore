namespace AuthApiCore.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // Construtor padrão
        public RegisterModel() { }

    }
}
