namespace UserManagement.Entities
{
    public class UserSetting : BaseEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public long AllocatedSpace { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string IsDarkMode { get; set; }
        public string UserLogoId { get; set; }

        public UserSetting()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
