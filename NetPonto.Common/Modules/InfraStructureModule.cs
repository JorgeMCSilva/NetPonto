using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace NetPonto.Common.Modules
{
    public class InfrastructureModule : Module
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


            //colocar aqui o EF 
            //builder.RegisterType<ConfigureNHibernate>()
            //    .WithParameters(new[]
            //                        {
            //                            new NamedParameter("connectionString",
            //                                               _connectionString),
            //                            new NamedParameter("assembliesWithEntities",
            //                                               new[] {typeof (Event).Assembly})
            //                        })
            //    .SingleInstance();

            //builder.RegisterAdapter<ConfigureNHibernate, NHibernate.Cfg.Configuration>(configure => configure.CreateConfiguration())
            //    .SingleInstance();

            //builder.RegisterAdapter<NHibernate.Cfg.Configuration, ISessionFactory>(configuration => configuration.BuildSessionFactory())
            //    .SingleInstance();

            //builder.RegisterType<SchemaUpdate>();

            //// TODO: create transaction when opening, abort on error, commit on success
            //builder.RegisterAdapter<ISessionFactory, ISession>(factory => TransactionPerRequest.SetSessionAndStartTransaction(factory.OpenSession()))
            //    .HttpRequestScoped();


            //builder.RegisterGeneric(typeof(NHibernateRepository<>))
            //    .As(typeof(IRepository<>))
            //    .HttpRequestScoped();


        }
    }
}
