namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public interface IPathsAndAreasFactory
    {
        IPathsAndAreas Create(IProductPathAreaMap productPathAreaMap);
    }

    public class PathsAndAreasFactory : IPathsAndAreasFactory
    {
        public IPathsAndAreas Create(IProductPathAreaMap productPathAreaMap)
        {
            return new PathsAndAreas(productPathAreaMap);
        }
     }
}
