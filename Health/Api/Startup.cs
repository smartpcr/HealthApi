// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Microsoft Corporation">
//   Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using Api;
using Microsoft.Owin;

[assembly:OwinStartup(typeof(Startup))]
namespace Api
{
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWebApi(WebApiConfig.Register());
        }
    }
}