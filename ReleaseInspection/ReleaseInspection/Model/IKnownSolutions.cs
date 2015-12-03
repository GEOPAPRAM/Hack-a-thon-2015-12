namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface IKnownSolutions
    {
        bool Contains(string name);
        Solution Get(string name);
    }
}