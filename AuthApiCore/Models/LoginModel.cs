﻿namespace AuthApiCore.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public LoginModel(string mail, string password)
        {

            Email = mail;
            Password = password;

        }
    }
}
