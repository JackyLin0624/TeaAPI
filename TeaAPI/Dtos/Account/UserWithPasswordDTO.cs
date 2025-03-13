namespace TeaAPI.Dtos.Account
{
    public class UserWithPasswordDTO : UserDTO
    {
        public string PasswordHash { get; set; }
    }
}
