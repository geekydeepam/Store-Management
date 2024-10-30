using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public static class CustomRoles
    {
        public const string A = "Admin";
        public const string C = "Cloth Store";
        public const string GS = "General Store";

        public const string A_GS =A + "," +GS;
        public const string A_C=A + "," +C;
    }
}