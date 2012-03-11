﻿using System.Linq;
using System.Web.Http.Description;
using System.Web.Http.Dispatcher;
using Xunit;

namespace System.Web.Http.ApiExplorer
{
    public class ParameterSourceTest
    {
        [Fact]
        public void FromUriParameterSource_ShowUpCorrectlyOnDescription()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            DefaultHttpControllerFactory controllerFactory = ApiExplorerHelper.GetStrictControllerFactory(config, typeof(ParameterSourceController));
            config.ServiceResolver.SetService(typeof(IHttpControllerFactory), controllerFactory);
            IApiExplorer explorer = config.ServiceResolver.GetApiExplorer();

            ApiDescription description = explorer.ApiDescriptions.FirstOrDefault(desc => desc.ActionDescriptor.ActionName == "GetCompleTypeFromUri");
            Assert.NotNull(description);
            Assert.True(description.ParameterDescriptions.All(param => param.Source == ApiParameterSource.FromUri), "All parameters should come from URI.");

            description = explorer.ApiDescriptions.FirstOrDefault(desc => desc.ActionDescriptor.ActionName == "GetCustomFromUriAttribute");
            Assert.NotNull(description);
            Assert.True(description.ParameterDescriptions.Any(param => param.Source == ApiParameterSource.FromUri && param.Name == "value"), "The 'value' parameter should come from URI.");
            Assert.True(description.ParameterDescriptions.Any(param => param.Source == ApiParameterSource.FromBody && param.Name == "bodyValue"), "The 'bodyValue' parameter should come from body.");
        }

        [Fact]
        public void FromBodyParameterSource_ShowUpCorrectlyOnDescription()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            DefaultHttpControllerFactory controllerFactory = ApiExplorerHelper.GetStrictControllerFactory(config, typeof(ParameterSourceController));
            config.ServiceResolver.SetService(typeof(IHttpControllerFactory), controllerFactory);
            IApiExplorer explorer = config.ServiceResolver.GetApiExplorer();

            ApiDescription description = explorer.ApiDescriptions.FirstOrDefault(desc => desc.ActionDescriptor.ActionName == "PostSimpleTypeFromBody");
            Assert.NotNull(description);
            Assert.True(description.ParameterDescriptions.All(param => param.Source == ApiParameterSource.FromBody), "The parameter should come from Body.");
        }

        [Fact]
        public void UnknownParameterSource_ShowUpCorrectlyOnDescription()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            DefaultHttpControllerFactory controllerFactory = ApiExplorerHelper.GetStrictControllerFactory(config, typeof(ParameterSourceController));
            config.ServiceResolver.SetService(typeof(IHttpControllerFactory), controllerFactory);
            IApiExplorer explorer = config.ServiceResolver.GetApiExplorer();

            ApiDescription description = explorer.ApiDescriptions.FirstOrDefault(desc => desc.ActionDescriptor.ActionName == "GetFromHeaderAttribute");
            Assert.NotNull(description);
            Assert.True(description.ParameterDescriptions.All(param => param.Source == ApiParameterSource.Unknown), "The parameter source should be Unknown.");
        }
    }
}