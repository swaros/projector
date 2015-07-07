using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Projector
{
    [Serializable()]
    class QueryBookMark : ISerializable
    {
        public QueryBookMark()
        {
        }

        public string name = "undefined";
        public string query = "";

        public QueryBookMark(SerializationInfo info, StreamingContext ctxt)
        {
            this.name = (string)info.GetValue("name", typeof(string));
            this.query = (string)info.GetValue("query", typeof(string));
           

        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("name", this.name);
            info.AddValue("query", this.query);
            
        }


    }
}
