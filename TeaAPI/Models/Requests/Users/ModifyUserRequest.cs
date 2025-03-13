namespace TeaAPI.Models.Requests.Users
{
    public class ModifyUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        //public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
