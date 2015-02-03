using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class ProfilProps
    {
        /// <summary>
        /// STRING wich style is used for plain forms
        /// </summary>
        public const string WINDOW_STYLE = "style_window";


        /// <summary>
        /// STRING wich style is used for MDI windows
        /// </summary>
        public const string MDI_STYLE = "style_mdi";

        /// <summary>
        /// STRING Username for database connection
        /// </summary>
        public const string DB_USERNAME = "db_username";

        /// <summary>
        /// password for database connection
        /// </summary>
        public const string DB_PASSWORD = "db_password";

        /// <summary>
        /// hostname for database connection
        /// </summary>
        public const string DB_HOST = "db_host";

        /// <summary>
        /// Name of the used database
        /// </summary>
        public const string DB_SCHEMA = "db_schema";

        /// <summary>
        /// name of the used database. same as DB_SCHEMA
        /// </summary>
        public const string DB_NAME = "db_schema";

        /// <summary>
        /// BOOL foreign key check enabled or not
        /// </summary>
        public const string DB_FOREIGN_KEY = "foreign_key_check";

        /// <summary>
        /// name of the external dif viewer
        /// </summary>
        public const string DIFF_CMD = "diff_command";

        /// <summary>
        /// paramaters for the external diff viewer
        /// </summary>
        public const string DIFF_PARAM = "diff_param";

        /// <summary>
        /// used color for this profil
        /// </summary>
        public const string STYLE_COLOR = "set_bgcolor";

        /// <summary>
        /// setting for database timeout
        /// </summary>
        public const string DB_TIMEOUT = "db_timeout";
        
      
    }
}
