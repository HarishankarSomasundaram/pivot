using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Pivot.Models
{
    public class NorthwindDataContextExt: Pivot.Models.NorthwindDataContext
    {
        static string ConnectionString
        {
            get
            {
                string sqlExpressString = ConfigurationManager.ConnectionStrings["NWindConnectionString"].ConnectionString;
                return sqlExpressString;
            }
        }
        public NorthwindDataContextExt() : base(ConnectionString) { }
    }
}