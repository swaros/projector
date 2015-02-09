using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Projector
{
    class ObjectStorage
    {
        private static Hashtable Storage = new Hashtable();

        static public Boolean StoreObject(string token, string name, Object obj){
            if (!ObjectStorage.Storage.ContainsKey(token))
            {
                Hashtable tokenObjects = new Hashtable();
                tokenObjects.Add(name, obj);
                ObjectStorage.Storage.Add(token, tokenObjects);
                return true;
            }
            else
            {
                Hashtable Stored = (Hashtable)ObjectStorage.Storage[token];
                if (Stored.ContainsKey(name))
                {
                    return false;
                }

                Stored.Add(name, obj);
                ObjectStorage.Storage[token] = Stored;
                return true;
            }
        }

        public static Object getObject(string token, string name){
            if (ObjectStorage.Storage.ContainsKey(token))
            {
                Hashtable Stored = (Hashtable)ObjectStorage.Storage[token];
                if (Stored.ContainsKey(name))
                {
                    return Stored[name];
                }
            }
            return null;
        }

        public static Boolean isStored(string token, string name)
        {
            Object tmp = ObjectStorage.getObject(token, name);
            return (tmp != null);
        }

        public static Boolean removeObject(string token, string name)
        {
            if (ObjectStorage.Storage.ContainsKey(token))
            {
                Hashtable Stored = (Hashtable)ObjectStorage.Storage[token];
                if (Stored.ContainsKey(name))
                {
                    Stored.Remove(name);
                    GC.Collect();
                    return true;
                }
            }
            return false;
        }

        public static Boolean removeToken(string token)
        {
            if (ObjectStorage.Storage.ContainsKey(token))
            {
                ObjectStorage.Storage.Remove(token);
                GC.Collect();
                return true;
            }
            return false;
        }

    }
}
