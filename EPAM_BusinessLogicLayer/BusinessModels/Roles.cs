using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EPAM_BusinessLogicLayer.BusinessModels
{
    public class Roles
    {
        public const string Administrator = "Administrator";
        public const string Owner = "Owner";
        public const string Moderator = "Moderator";
        public const string Premium = "Premium";
        public const string Plus = "Plus";
        public const string User = "User";

        public static IEnumerable<string> GetRoles()
        {
            return new[] {Administrator, Owner, Moderator, Premium, Plus, User};
        }

    }
}
