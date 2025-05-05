using System;

namespace Repository.Helper.CustomExceptions
{
    
    public class CustomException : Exception
    {
        public int StatusCode { get; set; }
        public string CustomMessage { get; set; }

        public CustomException(string message, int statusCode) : base(message)
        {
            CustomMessage = message;
            StatusCode = statusCode;
        }
    }

  
    public class UserAlreadyExistsException : CustomException
    {
        public UserAlreadyExistsException()
            : base("User already exists with this email.", 409) 
        {
        }
    }

  
    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException()
            : base("User not found.", 404) 
        {
        }
    }

    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException()
            : base("Invalid email or password.", 401) 
        {
        }
    }

    public class PasswordMismatchException : CustomException
    {
        public PasswordMismatchException()
            : base("New password and confirm password do not match.", 400) 
        {
        }
    }
}
