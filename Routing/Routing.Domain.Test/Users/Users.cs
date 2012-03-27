using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Routing.Domain.Infrastructure;
using Routing.Domain.Services;
using Autofac;
using Routing.Domain.Dto.Command;

namespace Routing.Domain.Test.Accounts
{
    [TestClass]
    public class Accounts
    {
        [TestMethod]
        public void Register_New_User()
        {
            var container = Container.Init_Container();
            var service = container.Resolve<RegistrationService>();

            var cmd = new Register_New_User { DisplayName ="Valerio", Identity="123", Provider="Google" };
            service.Register_New_User(cmd);
        }
    }
}
