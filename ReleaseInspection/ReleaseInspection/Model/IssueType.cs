namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class IssueType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static int TryParse(string name)
        {
            switch (name)
            {
                case "feature": 
                    return 10001;
                case "bug":
                    return 1;
                case "chore":
                    return 3;
                default:
                    return 0;
            }
        }
    }
}
