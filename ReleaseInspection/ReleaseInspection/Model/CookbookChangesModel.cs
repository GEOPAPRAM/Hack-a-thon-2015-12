namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class CookbookChangesModel
    {
        public CookbookChangesModel( string cloudName,string envCookbookName)
        {
            CloudName = cloudName;
            EnvCookbookName = envCookbookName;
        }

        public string CloudName { get; set; }
        public string Description { get; set; }
        public string EnvCookbookName { get; set; }

        public ReleaseModel Changes { get; set; }
    }
}
