using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace found.Models
{
    public class FirstFactory
    {
        //private List<Table> queryBySql(string sql) 
        //{
        //    SqlConnection con = new SqlConnection();
        //    con.ConnectionString = @"";//todo1
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand(sql,con);
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    List<Table> list = new List<Table>();
        //    while (reader.Read()) {
        //        Table x = new Table()
        //        {
        //            fId = (int)reader["fId"],
        //            fName = reader["fName"].ToString(),
        //            cId = (int)reader["cId"],
        //            fText = reader["fText"].ToString(),
        //            targetMoney = (int)reader["targetMoney"],
        //            accMoney = (int)reader["accMoney"],
        //            startTime = (DateTime)reader["startTime"],
        //            endTime = (DateTime)reader["endTime"],
        //            trueName = reader["trueName"].ToString(),
        //            fPhone = reader["fPhone"].ToString(),
        //            fEmail = reader["fEmail"].ToString(),
        //        };
        //        list.Add(x);
        //    }
        //    con.Close();
        //    return list;
        //}
       
        //public List<Table> queryAll()
        //{
        //    return queryBySql("select * from Table");
        //}
        //public 

    }
}