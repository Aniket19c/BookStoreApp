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

    public class IncorrectPasswordException : CustomException
    {
        public IncorrectPasswordException()
            : base("New password and confirm password do not match.", 400)
        {
        }
    }

    public class BookNotFoundException : CustomException
    {
        public int BookId { get; set; }

        public BookNotFoundException()
            : base("The specified book was not found.", 404)
        {
        }

        public BookNotFoundException(int bookId)
            : base($"Book with ID {bookId} was not found.", 404)
        {
            BookId = bookId;
        }
    }
}
