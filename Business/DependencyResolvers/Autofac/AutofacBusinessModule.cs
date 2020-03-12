using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Interceptors;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<RoomManager>().As<IRoomService>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<UserMongoRepository>().As<IUserDal>();
            builder.RegisterType<OperationClaimMongoRepository>().As<IOperationClaimDal>();
            builder.RegisterType<RoomMongoRepository>().As<IRoomDal>();

            //Get Executing Assembly for Fluent Validation Interception
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new ProxyGenerationOptions() { 
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
        }
    }
}
