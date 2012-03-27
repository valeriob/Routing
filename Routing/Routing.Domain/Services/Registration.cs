using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Routing.Domain.Dto.Command;
using Raven.Client;
using Routing.Domain.Aggregates;

namespace Routing.Domain.Services
{
    public class RegistrationService
    {
        IDocumentSession DocumentSession;
     

        public RegistrationService(IDocumentSession documentSession)
        {
            DocumentSession = documentSession;

        }

        public void Register_New_User(Register_New_User cmd)
        {
            var identity_already_exists = DocumentSession.Query<User>().Any(u=> u.Identities.Any(i=> i.Provider == cmd.Provider && i.Id == cmd.Identity));
            if(identity_already_exists)
                throw new Exception("");

            var user = new User { Name = cmd.DisplayName };
            user.Identify(cmd.Provider, cmd.Identity, cmd.DisplayName);

            DocumentSession.Store(user);
        }
    }
}
