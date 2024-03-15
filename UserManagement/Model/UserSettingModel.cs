namespace UserManagement.Model
{
    public class UserSettingModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long AllocatedSpace { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string IsDarkMode { get; set; }
        public string UserLogoId { get; set; }
    }
}
