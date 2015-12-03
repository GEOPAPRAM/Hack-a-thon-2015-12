using System.Collections.Generic;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.CallCentre;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.IVR;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WAF;
using NewVoiceMedia.Tools.ReleaseInspection.DeployableComponents.WFM;
using NewVoiceMedia.Tools.ReleaseInspection.Model;
using NewVoiceMedia.Tools.ReleaseInspection.Service;
using NewVoiceMedia.Tools.ReleaseInspection.WebPresentation.Modules;
using Ninject;

namespace NewVoiceMedia.Tools.ReleaseInspection.WebPresentation
{
    public class ReleaseNotesBootStrapper : NinjectNancyBootstrapper
    {
        internal List<string> AlreadyConfigured = new List<string>();

        protected override void RequestStartup(IKernel container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest += ctx =>
            {
                ctx.Response.ContentType = "text/html; charset=utf-8";
            };
        }

        protected override void ConfigureApplicationContainer(IKernel container)
        {
            base.ConfigureApplicationContainer(container);

            ConfigureCommonBindings(container);
            // Revision Modules :
            ConfigureModule<IVR, IvrVersionRetrieverService, IvrRevisionsModule>(container, IVR.Identifier);
            ConfigureModule<CallCentre, CallCentreVersionRetrieverService, CallCentreRevisionsModule>(container, CallCentre.Identifier);
            ConfigureModule<WFM, WfmVersionRetrieverService, WfmRevisionsModule>(container, WFM.Identifier);
            // Release Candidate Modules :
            ConfigureModule<IVR, IvrVersionRetrieverService, IvrReleaseCandidateModule>(container, IVR.Identifier);
            ConfigureModule<WAF, WafVersionRetrieverService, WafReleaseCandidateModule>(container, WAF.Identifier);
            ConfigureModule<CallCentre, CallCentreVersionRetrieverService, CallCentreReleaseCandidateModule>(container, CallCentre.Identifier);
            ConfigureModule<WFM, WfmVersionRetrieverService, WfmeReleaseCandidateModule>(container, WFM.Identifier);
        }

        private void ConfigureCommonBindings(IKernel container)
        {
            container.Bind<IStoryWorkFactory>().To<StoryWorkFactory>();
            container.Bind<IPathsAndAreasFactory>().To<PathsAndAreasFactory>();
            container.Bind<ISourceControl>().To<SourceControl>();
            container.Bind<IStoryRepositoryClient>().To<StoryRepositoryService>();
            container.Bind<IKnownSolutions>().To<KnownSolutions>();
            container.Bind<ICookbookService>().To<CookbookService>();
        }

        private void ConfigureModule<TDeployableComponent,TVersionRetrieverService,TNancyModule>(IKernel container, string identifier)
            where TDeployableComponent : DeployableComponentBase
            where TVersionRetrieverService : IVersionRetrieverService
            where TNancyModule : NancyModule
        {
            var item = typeof (TDeployableComponent).Name;
            if (!AlreadyConfigured.Contains(item))
            {
                container.Bind<IDeployableComponent>().To<TDeployableComponent>().Named(identifier);
                AlreadyConfigured.Add(item);
            }

            item = typeof (TVersionRetrieverService).Name;
            if (!AlreadyConfigured.Contains(item))
            {
                container.Bind<IVersionRetrieverService>()
                    .To<TVersionRetrieverService>()
                    .WhenInjectedExactlyInto<TDeployableComponent>();
                AlreadyConfigured.Add(item);
            }

            container.Bind<IReleaseService>()
                .To<ReleaseService>()
                .WhenInjectedInto<TNancyModule>()
                .WithConstructorArgument("deployableComponent", iC => iC.Kernel.Get<IDeployableComponent>(identifier));
        }
    }
}