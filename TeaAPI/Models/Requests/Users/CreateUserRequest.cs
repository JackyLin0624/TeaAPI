namespace TeaAPI.Models.Requests.Users
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
