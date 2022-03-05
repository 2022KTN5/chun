using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Auth.Models
{
    public class Class1_factory
    {
        public Class1 queryLogIn(string charityname)
        {
            List<Class1> list = queryBySql("select * from Charity_member1 where c_name='" + charityname + "'");
            if (list.Count == 0)
                return null;
            return list[0];
        }
        public Class1 queryBycid(int cId)
        {
            List<Class1> list = queryBySql("select * from Charity_member1 where cId=" + cId.ToString());
            if (list.Count == 0)
                return null;
            return list[0];
        }
        public List<Class1> queryAll()
        {
            return queryBySql("select * from Charity_member1");
        }
        private List<Class1> queryBySql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=shopping;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Class1> list = new List<Class1>();
            while (reader.Read())
            {
                Class1 x = new Class1()
                {
                    //fId = (int)reader["fId"],
                    //fName = reader["fName"].ToString(),
                    //fPhone = reader["fPhone"].ToString(),
                    //fEmail = reader["fEmail"].ToString(),
                    //fAddress = reader["fAddress"].ToString(),
                    //fPassword = reader["fPassword"].ToString(),
                    cId = (int)reader["cId"],
                    c_name = reader["c_name"].ToString(),
                    c_address = reader["c_address"].ToString(),
                    c_account = reader["c_account"].ToString(),
                    c_password = reader["c_password"].ToString(),
                    c_phone = reader["c_phone"].ToString(),
                    c_email = reader["c_email"].ToString(),
                    c_pname = reader["c_pname"].ToString(),
                };
                list.Add(x);
            }
            con.Close();
            return list;
        }
        public void create(Class1 p)
        {
            string sql = "insert into Charity_member1(";
            //sql += "fId,";
            sql += "c_name,";
            sql += "c_address,";
            sql += "c_account,";
            sql += "c_password,";
            sql += "c_phone,";
            sql += "c_email,";
            sql += "c_pname";
            sql += ")VALUES(";
            sql += "'" + p.c_name + "',";
            sql += "'" + p.c_address + "',";
            sql += "'" + p.c_account + "',";
            sql += "'" + p.c_password + "',";
            sql += "'" + p.c_phone + "',";
            sql += "'" + p.c_email + "',";
            sql += "'" + p.c_pname + "')";

            executeSql(sql);

        }
        public void delete(int cId)
        {
            string sql = "delete from Charity_member1 where cId=" + cId.ToString();
            executeSql(sql);
        }
        public void update(Class1 p)
        {
            string sql = "update Charity_member1 set ";
            sql += "c_name='" + p.c_name + "',";
            sql += "c_address='" + p.c_address + "',";
            sql += "c_account='" + p.c_account + "',";
            sql += "c_password='" + p.c_password + "',";
            sql += "c_phone='" + p.c_phone + "',";
            sql += "c_email='" + p.c_email + "',";
            sql += "c_pname='" + p.c_pname + "'";
            sql += "where cId=" + p.cId.ToString();
            executeSql(sql);
        }

        private void executeSql(string sql)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=shopping;Integrated Security=True";
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}