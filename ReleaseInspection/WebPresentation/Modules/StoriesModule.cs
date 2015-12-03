using Nancy;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules
{
    public class StoriesModule : NancyModule
    {
        public StoriesModule() : base("/stories")
        {
            Get["/"] = parameters =>
                {
                    var startRevision = (string) Request.Query["start"];
                    var endRevision = (string) Request.Query["end"];
                    var newLocation = string.Format("/revisions/call-centre?start={0}&end={1}", startRevision, endRevision);
                     
                    return
                        Response.AsRedirect(newLocation);
                };
        }
    }
}