namespace TeaAPI.Models.Accounts
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }  
        public List<Permission> Permissions { get; set; } = new List<Permission>(); 
    }
}
