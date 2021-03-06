﻿namespace JenkinsClient.Model
{
    public class CookbookInfo
    {
        public CookbookInfo(string cookbookName)
        {
            CookbookName = cookbookName;
        }

        public string CookbookName { get; private set; }
        public string Description { get; set; }
        public string EnvBuildVersion { get; set; }
        public string AppBuildVersion { get; set; }
        public ChangeSet Changes { get; set; }
    }
}
