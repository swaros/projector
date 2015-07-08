using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class MysqlStoredProc
    {
        private MysqlHandler usedhandler;

        public MysqlStoredProc(MysqlHandler useThis)
        {
            this.usedhandler = useThis;
        }

        public void call(string name, Hashtable parameters)
        {            
            MySqlCommand cmd = new MySqlCommand(name, this.usedhandler.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (DictionaryEntry param in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + param.Key.ToString(), param.Value.ToString());
                }
            }

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine(rdr[0] + " --- " + rdr[1]);
            }
            rdr.Close();
        }

    }
}
