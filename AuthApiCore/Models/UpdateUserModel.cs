namespace AuthApiCore.Models
{
    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string CurrentPassword { get; set; } 
        public string NewPassword { get; set; }
    }
}
