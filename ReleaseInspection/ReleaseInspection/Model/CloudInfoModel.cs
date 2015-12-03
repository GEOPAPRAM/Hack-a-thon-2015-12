namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class CloudInfoModel
    {
        public string CloudName { get; set; }
        public string CookbookName { get; set; }

        public CloudInfoModel(string cloudName, string cookbookName)
        {
            CloudName = cloudName;
            CookbookName = cookbookName;
        }
    }
}
