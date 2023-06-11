using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Models
{
    public class CRUDResponse
    {
        public int Code { get; set; }
        public bool HasError { get; set; }
        public string Message { get; set; }
        public CRUDResponse Model { get; set; }
    }
}
