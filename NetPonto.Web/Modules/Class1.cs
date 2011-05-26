using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using System.Reflection;

namespace NetPonto.Web.Modules
{
    public class InfrastructureModule : Autofac.Module
    {
        private readonly Assembly _assemblyWithInfrastructure;
        private readonly string _connectionString;

        public InfrastructureModule(Assembly assemblyWithInfrastructure, string connectionString)
        {
            _assemblyWithInfrastructure = assemblyWithInfrastructure;
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(_assemblyWithInfrastructure).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(_assemblyWithInfrastructure).AsSelf();

        }
    }
}