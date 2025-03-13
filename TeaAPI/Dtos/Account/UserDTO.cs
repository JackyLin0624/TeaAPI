namespace TeaAPI.Dtos.Account
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Account { get; set; }
        public RoleDTO Role { get; set; }
    }
}
