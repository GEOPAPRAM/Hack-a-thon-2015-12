using System;

namespace NewVoiceMedia.Tools.ReleaseInspection.Service
{
    public interface IVersionRetrieverService
    {
        Version GetVersionInformation();
    }
}