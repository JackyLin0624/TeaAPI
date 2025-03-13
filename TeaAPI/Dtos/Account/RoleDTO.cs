namespace TeaAPI.Dtos.Account
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PermissionDTO> Permissions { get; set; }
    }
}
