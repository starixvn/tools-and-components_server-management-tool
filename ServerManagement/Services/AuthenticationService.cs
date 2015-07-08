using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading;

namespace ServerManagement.Services
{
    public class AuthenticationService : BaseService
    {
        public bool ValidateUser(string userName, string password)
        {
            return RunCommand(pc => pc.ValidateCredentials(userName, password));
        }

        private bool InGroups(string groupNames, string accountName)
        {
            var groups = groupNames.Split(';');
            if (groupNames.Length == 0) return false;

            return groups.Any(g =>
            {
                var members = GetGroupMembers(g);
                return members != null && members.Contains(accountName, StringComparer.InvariantCultureIgnoreCase);
            });
        }

        private List<string> GetGroupMembers(string groupName)
        {
            var group = RunCommand(pc =>
            {
                using (var gp = GroupPrincipal.FindByIdentity(pc, groupName))
                {
                    return gp == null
                               ? new List<string>()
                               : gp.GetMembers().ToList().Select(mp => mp.SamAccountName).ToList();
                }
            });
            return group ?? new List<string>();
        }

        private T RunCommand<T>(Func<PrincipalContext, T> command, int retries = 3)
        {
            try
            {
                using (var pc = new PrincipalContext(ContextType.Domain))
                    return command(pc);
            }
            catch
            {
                if (retries > 0)
                {
                    Thread.Sleep(1000);
                    RunCommand(command, retries - 1);
                }
            }

            return default(T);
        }
    }
}