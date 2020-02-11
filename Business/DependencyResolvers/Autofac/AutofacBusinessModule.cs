using Autofac;
using Autofac.Core;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AuthManager>().As<IAuthService>();

            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<UserMongoRepository>().As<IUserDal>();
            builder.RegisterType<OperationClaimMongoRepository>().As<IOperationClaimDal>();
        }
    }
}
