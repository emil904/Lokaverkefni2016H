using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LokaverkefniServer
{
    class dbClass
    {
        private string server;
        private string database;
        private string uid;
        private string password;

        string tengistrengur = null;
        string fyrirspurn = null;

        MySqlConnection sqltenging;
        MySqlCommand newSQLCommand;
        MySqlDataReader sqllesari = null;

        public void TengingVidDB()
        {
            server = "tsuts.tskoli.is";
            database = "2310952929_lokakverkefniusers";
            uid = "2310952929";
            password = "Branani.9041";

            tengistrengur = "server=" + server + ";userid=" + uid + ";password=" + password + ";database=" + database;
            sqltenging = new MySqlConnection(tengistrengur);
        }

        private bool OpenConnect() 
        {
            try
            {
                sqltenging.Open();
                return true;
            }
            catch (MySqlException e)
            {
                throw e;
            }
        }

        private bool CloseConnect()
        {
            try
            {
                sqltenging.Close();
                return true;
            }
            catch (MySqlException e)
            {
                
                throw e;
            }
        }

        public void userRegister(string email, string username, string password)
        {
            if (OpenConnect() == true)
            {
                fyrirspurn = "INSERT INTO users (USER_EMAIL, USERNAME, USER_PASSWORD) VALUES ('" + email + "','" + username + "','" + password + "')";

                newSQLCommand = new MySqlCommand(fyrirspurn, sqltenging);
                newSQLCommand.ExecuteNonQuery();
                CloseConnect();
            }
        }

        public string findUserToValidate(string email, string password)
        {
            string lina = null;
            if (OpenConnect() == true)
            {
                    fyrirspurn = "SELECT USER_EMAIL,USERNAME,USER_PASSWORD FROM users WHERE USER_EMAIL='" + email + "' AND USER_PASSWORD='" + password + "'";
                    newSQLCommand = new MySqlCommand(fyrirspurn, sqltenging);
                    sqllesari = newSQLCommand.ExecuteReader();
                    while (sqllesari.Read())
                    {
                        for (int i = 0; i < sqllesari.FieldCount; i++)
                        {
                            lina += (sqllesari.GetValue(i).ToString()) + ":";
                        }
                    }
                    if (lina == null)
                    {
                        lina = "U/P incorrect";
                    }

                CloseConnect();
            }
            return lina;
        }
    }
}
