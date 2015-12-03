namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class StoryWorkFactory : IStoryWorkFactory
    {
        private readonly IPathsAndAreasFactory _pathsAndAreasFactory;

        public StoryWorkFactory(IPathsAndAreasFactory pathsAndAreasFactory)
        {
            _pathsAndAreasFactory = pathsAndAreasFactory;
        }

        public UntrackedStoryWork Create(IProductPathAreaMap productPathAreaMap)
        {
            return new UntrackedStoryWork(_pathsAndAreasFactory.Create(productPathAreaMap));
        }

        public StoryWork Create(Story story, IProductPathAreaMap productPathAreaMap)
        {
            return new StoryWork(story, _pathsAndAreasFactory.Create(productPathAreaMap));
        }
    }

    public interface IStoryWorkFactory
    {
        UntrackedStoryWork Create(IProductPathAreaMap productPathAreaMap);
        StoryWork Create(Story story, IProductPathAreaMap productPathAreaMap);
    }
}