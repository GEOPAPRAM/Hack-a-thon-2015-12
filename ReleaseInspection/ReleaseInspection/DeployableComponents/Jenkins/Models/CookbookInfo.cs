namespace NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.Jenkins.Models
{
    public class CookbookInfo : CookbookInfoBase
    {
        public CookbookInfo(string cookbookName) : base(cookbookName){ }

        public ChangeSet Changes { get; set; }
        public Cause AppCookbook { get; set; }
    }
}
