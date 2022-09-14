using Autofac;
using Autofac.Integration.Mvc;
using GigHubMVC.Models;
using GigHubMVC.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHubMVC.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. 
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            // Register DbContext
            builder.RegisterType<ApplicationDbContext>().InstancePerRequest();
            // Register UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}