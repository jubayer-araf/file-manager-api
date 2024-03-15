namespace FileManagement.Entities
{
    public class SettingsFile : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int FileTypeId { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string OwnerId { get; set; }

        public SettingsFile() 
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
