using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Projector
{
    class Serializer
    {
        public Serializer()
        {
        }

        public void SerializeObject(string filename, TablePropHandler objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public TablePropHandler DeSerializeObject(string filename)
        {
            TablePropHandler objectToSerialize;
            Stream stream = null;
            stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            if (stream != null)
            {
                Object tmp = bFormatter.Deserialize(stream);
                objectToSerialize = (TablePropHandler) tmp;
                //objectToSerialize = (TablePropHandler)bFormatter.Deserialize(stream);
                stream.Close();
                return objectToSerialize;
            }
            else
            {
                return null;
            }
        }
    }

    class PlaceHolderSerializer
    {
        public PlaceHolderSerializer()
        {

        }

        public string getDefaultFilename(Profil profil)
        {
            string name = "default";
            if (null != profil)
            {
                name = profil.getProperty("db_username") + profil.getProperty("db_host") + profil.getProperty("db_shema");
                name = name.Replace(@"\", "_");
                name = name.Replace(".", "_");
                name = name.Replace(" ", "_");
                name = name.Replace(":", "_");
            }
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + "placeHolder_" + name + ".plc";
        }


        public void SerializeObject(string filename, PlacerHolder objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public PlacerHolder DeSerializeObject(string filename)
        {
            PlacerHolder objectToSerialize;
            Stream stream = null;
            stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            if (stream != null)
            {

                try
                {
                    Object tmp = bFormatter.Deserialize(stream);
                    objectToSerialize = (PlacerHolder)tmp;
                    //objectToSerialize = (TablePropHandler)bFormatter.Deserialize(stream);
                    stream.Close();
                    return objectToSerialize;
                }
                catch (Exception)
                {
                    return null;
                    //throw;
                }
            }
            else
            {
                return null;
            }
        }

    }

    class BookMarkSerializer
    {
        public BookMarkSerializer()
        {
        }

        public void SerializeObject(string filename, BookmarkManager objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public BookmarkManager DeSerializeObject(string filename)
        {
            BookmarkManager objectToSerialize;
            Stream stream = null;
            stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            if (stream != null)
            {

                try
                {
                    Object tmp = bFormatter.Deserialize(stream);
                    objectToSerialize = (BookmarkManager)tmp;
                    //objectToSerialize = (TablePropHandler)bFormatter.Deserialize(stream);
                    stream.Close();
                    return objectToSerialize;
                }
                catch (Exception)
                {
                    return null;
                    //throw;
                }
            }
            else
            {
                return null;
            }
        }
    }

}
