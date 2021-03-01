using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.ต่อดาต้าเบส
{
    public class ConnectDB
    {

        public void OpenConection()
        {
            string ConnectionString = null;
            SqlConnection scc;
            ConnectionString = "Data Source=TEERAYUTSINBDEV/SQL2017;Initial Catalog=Develop_NutritionDB;User ID=sa;Password=P@ssw0rd ";
            scc = new SqlConnection(ConnectionString);
            try
            {
                scc.Open();


                scc.Close();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}