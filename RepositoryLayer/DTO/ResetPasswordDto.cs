namespace BookStore.Models.DTO.User
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
