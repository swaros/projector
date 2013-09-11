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
    class PlacerHolder : ISerializable
    {
        Hashtable placeHolder = new Hashtable();
        Boolean allProfiles = false;
        List<string> forProfiles = new List<string>();

        public int Count()
        {
            return placeHolder.Count;
        }

        public void clear()
        {
            placeHolder.Clear();
        }

        public List<string> getKeys()
        {
            List<string> result = new List<string>();
            foreach (DictionaryEntry de in this.placeHolder)
            {
                string keyname = de.Key.ToString();                                
                result.Add(keyname);
            }
            return result;
        }

        public string value(string name)
        {
            if (placeHolder.ContainsKey(name))
            {
                return (string) placeHolder[name];
            }
            else
            {
                throw new Exception("Key allready not exists. use PlayeHolder.exists(<keyname> ToString check it first");
            }
        }

        public void addPlaceHolder(string name, string value){
            if (!placeHolder.ContainsKey(name)){
                placeHolder.Add(name, value);
                
            } else {
                throw new Exception("Key allready exists. use PlayeHolder.exists(<keyname> ToString check it first");
            }
        }

        public void remove(string name)
        {
            if (placeHolder.ContainsKey(name))
            {
                placeHolder.Remove(name);
            }
            else
            {
                throw new Exception("Key allready not exists. use PlayeHolder.exists(<keyname> ToString check it first");
            }
        }

        public void update(string name, string value)
        {
            if (placeHolder.ContainsKey(name))
            {
                placeHolder[name] = value;
            }
            else
            {
                throw new Exception("Key allready not exists. use PlayeHolder.exists(<keyname> ToString check it first");
            }
        }

        public Boolean exists(string name)
        {
            return placeHolder.ContainsKey(name);
        }
              
        public PlacerHolder(SerializationInfo info, StreamingContext ctxt)
        {
            this.placeHolder = (Hashtable)info.GetValue("placeHolder", typeof(Hashtable));
            this.forProfiles = (List<string>)info.GetValue("profiles", typeof(List<string>)); 
        }

        public PlacerHolder()
        {
            // TODO: Complete member initialization
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("placeHolder", this.placeHolder);
            info.AddValue("profiles", this.forProfiles);
            
            
        }

    }
}
