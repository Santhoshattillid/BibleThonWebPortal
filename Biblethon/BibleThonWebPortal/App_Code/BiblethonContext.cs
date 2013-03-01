using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Biblethon.Controller
{
    public class BiblethonContext
    {
        public string GetConnectionString()
        {
            string connection = ConfigurationManager.ConnectionStrings["GPConnectionString"].ToString();
            return connection;
        }
    }
}