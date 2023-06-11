using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Models
{
    public class ListResponse<T>
    {
        public IEnumerable<T> Data;
        public int Total { get; set; }
        public bool HasError = false;
        public string ErrorMessage = string.Empty;
    }
}
