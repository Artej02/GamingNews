using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Attributes
{
    public class JoinAlias : Attribute
    {
        public string Value { get; set; }
    }
}
