namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class CookbookChangesModel
    {
        public CookbookChangesModel(string cloudName)
        {
            CloudName = cloudName;
        }

        public string CloudName { get; set; }
        public string Description { get; set; }

        public ReleaseModel Changes { get; set; }
    }
}
