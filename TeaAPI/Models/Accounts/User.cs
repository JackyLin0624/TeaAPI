namespace TeaAPI.Models.Accounts
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Account { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActived { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateUser { get; set; }
        public DateTime ModifyAt { get; set; }
        public string ModifyUser { get; set; }
        public DateTime? DeleteAt { get; set; }
        public string DeleteUser { get; set; }
    }
}
