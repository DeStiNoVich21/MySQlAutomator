using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
namespace MySQlAutomator
{
    public class MySqlAutomator
    {   
        //Connection  line
        public string connection;
        //Table name
        public string tablename;


        public List<String> column = new List<String>();
        
        
        private void MySqlReadColumnNames()
        {

            MySqlConnection con = new MySqlConnection(connection);
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand($"Select * From {tablename}", con))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                var tableschema = reader.GetSchemaTable();
                foreach (DataRow row in tableschema.Rows)
                {
                    column.Add(row["ColumnName"].ToString());
                }
            }
            con.Close();
        }

        // Returns column names in table
        public string[] MySqlReadColumnNames(string tablename)
        {
            MySqlConnection con = new MySqlConnection(connection);
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand($"Select * From {tablename}", con))
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                var tableschema = reader.GetSchemaTable();
                foreach (DataRow row in tableschema.Rows)
                {
                    column.Add(row["ColumnName"].ToString());
                }
            }
            con.Close();
            return column.ToArray();
        }

        // Returns full data from selected row
        public string MySqlRowRead(string tablename, string columnname, string rowid)
        {
            column.Clear();
            MySqlReadColumnNames();
            MySqlConnection con = new MySqlConnection(connection);
            string rowdata = "";
            MySqlCommand cmd = new MySqlCommand($"SELECT * FROM {tablename} WHERE `{columnname}`='{rowid}'", con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                for (int i = 0; i < column.Count; i++)
                    rowdata += reader[column[i].ToString()] + ",";
            con.Close();
            return rowdata;
        }

        //adding the the row
        public void MySqlAdd(String[] a)
        {
            column.Clear();
            MySqlReadColumnNames();
            string cmdline = "";
            Random rnd = new Random();
            int ass = rnd.Next(1000000, 9999999);
            string addline = $"'{ass}'";
            MySqlConnection con = new MySqlConnection(connection);
            for (int i = 0; i < column.Count; i++)
            {
                if (i == 0)
                    cmdline += "`" + column[i] + "`";
                if (i > 0)
                    cmdline += "," + "`" + column[i] + "`";
            }

            for (int i = 0; i < a.Length; i++)
            {
                addline += "," + "'" + a[i].ToString() + "'";

            }
            con.Open();
            string d = $"INSERT INTO {tablename}({cmdline}) " + "VALUES (" + addline + ")";
            MySqlCommand cmd = new MySqlCommand(d, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //  Updating the selected piece of table
        public void MySqlUpdate(string id, string idvalue, string row, string value)
        {
            MySqlConnection con = new MySqlConnection(connection);
            con.Open();
            MySqlCommand statchange = new MySqlCommand($"UPDATE {tablename} SET `{row}` = '{value}' WHERE (`{id}`='{idvalue}' )", con);
            statchange.ExecuteNonQuery();
            con.Close();
        }

        // deleting the selected row
        public void MySqlDelete(string row, string rowid)
        {
            MySqlConnection con = new MySqlConnection(connection);
            con.Open();
            MySqlCommand statchange = new MySqlCommand($"DELETE  FROM {tablename} WHERE `{row}`='{rowid}' ", con);
            statchange.ExecuteNonQuery();
            con.Close();
        }

        // type is for the adding columns datatype
        public void MySqlColumnnAdd(string columnname, string type)
        {
            MySqlConnection con = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand($"ALTER TABLE {tablename} ADD COLUMN `{columnname}` {type} NULL ", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

    }
}
