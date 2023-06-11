using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Models
{
    public enum AuthorizationType
    {
        ReadWrite = 1,
        Read = 2,
        Block = 3
    }

    public enum RoleAccessLevels
    {
        AccessDenied = 0,
        ViewAccess = 1,
        FullAccess = 2
    }

    public enum Role
    {
        Administrator = 1,
        StandardUser = 2,
        Advisor = 3
    }

    public enum CRUDOperation
    {
        Create = 1,
        Update = 2,
        Delete = 3
    }
    public enum ChangeLogAction
    {
        Insert = 1,
        Update = 2,
        Delete = 3,
        Login = 4,
        Logout = 5
    }
    public enum ChangeLogTable
    {
        
    }
}
