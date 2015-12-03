using System.Collections.Generic;
using System.Linq;

namespace NewVoiceMedia.Tools.ReleaseInspection.Model
{
    public class PathsAndAreas : IPathsAndAreas
    {
        private readonly IProductPathAreaMap _productPathAreaMap;
        public IDictionary<string, IList<string>> Areas { get; private set; }
        public IDictionary<string, IList<string>> TestAreas { get; private set; } 
        public IList<string> Paths { get; private set; }
        public IList<string> UnmappedPaths { get; private set; }

        public PathsAndAreas(IProductPathAreaMap productPathAreaMap)
        {
            _productPathAreaMap = productPathAreaMap;
            Areas = new SortedDictionary<string, IList<string>>();
            TestAreas = new SortedDictionary<string, IList<string>>();
            Paths = new List<string>();
            UnmappedPaths = new List<string>();
        }

        public void ExtractPathsAndAreas(SourceChange sourceChange)
        {
            foreach (var path in sourceChange.Paths.Where(path => !Paths.Contains(path)))
            {
                Paths.Add(path);

                if (_productPathAreaMap.IgnoredPaths.Contains(path))
                {
                    continue;
                }

                var area = GetArea(path);

                if (area == null)
                {
                    UpdateUnmappedPaths(path);
                }
                else
                {
                    UpdateAreas(area, path);                    
                }
            }
        }

        public IPathsAndAreas GetCleanCopy()
        {
            return new PathsAndAreas(_productPathAreaMap);
        }

        private string GetArea(string path)
        {
            foreach (var areaAndPaths in _productPathAreaMap.PathsByArea)
            {
                var lowerCasePaths = areaAndPaths.Value.Select(p => p.ToLowerInvariant());
                if (lowerCasePaths.Any(path.ToLowerInvariant().Contains))
                {
                    return areaAndPaths.Key;
                }
            }

            return null;
        }

        private void UpdateAreas(string area, string path)
        {
            if (IsTestArea(area))
            {
                if (!TestAreas.ContainsKey(area))
                {
                    TestAreas[area] = new List<string>();
                }

                TestAreas[area].Add(path);
            }
            else
            {
                if (!Areas.ContainsKey(area))
                {
                    Areas[area] = new List<string>();
                }

                Areas[area].Add(path);
            }
        }

        private static bool IsTestArea(string area)
        {
            return area.Contains("Test");
        }

        private void UpdateUnmappedPaths(string path)
        {
            if (!UnmappedPaths.Contains(path))
            {
                UnmappedPaths.Add(path);
            }
        }
    }
}
