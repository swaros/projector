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
    class BookmarkManager : ISerializable
    {
        private List<QueryBookMark> bookMarks = new List<QueryBookMark>();

        // speed helper
        private string lastFoundName = "";
        private int lastFoundIndex = -1;

        /* Add a bookmark if not exists or replace the 
         * query if an bookmark with same name allready exists
         */ 
        public void addOrReplace(string name, string query)
        {
            int found = existsInIndex(name);
            if (found < 0)
            {
                QueryBookMark tmpBook = new QueryBookMark();
                tmpBook.name = name;
                tmpBook.query = query;
                bookMarks.Add(tmpBook);
            }
            else
            {
                bookMarks[found].query = query;
            }

        }

        
        public bool removeBookMark(string name)
        {
            int found = existsInIndex(name);
            if (found >= 0)
            {
                bookMarks.RemoveAt(found);
                return true;
            }
            return false;
        }  


        /**
         * get a bookmark by name
         */
        public QueryBookMark getByName(string name)
        {
            int found = existsInIndex(name);
            if (found < 0) return null;
            else return bookMarks[found];
        }


        /**
         * Returns a stringlist with all names from bookmark listing
         */ 
        public string[] listBookmarks()
        {
            string[] nameList = new string[bookMarks.Count];
            for (int i = 0; i < bookMarks.Count; i++)
            {
                nameList[i] = bookMarks[i].name;
            }
            return nameList;
        }

        /**
         * searchs a bookmark by name and returns the index if found
         * or -1 if not exists.
         */ 
        public int existsInIndex(string name)
        {
            if (lastFoundName == name)
            {
                return lastFoundIndex;
            }

            for (int i = 0; i < bookMarks.Count; i++)
            {
                if (bookMarks[i].name == name)
                {
                    lastFoundIndex = i;
                    lastFoundName = name;
                    return i;
                }
                
            }
            return -1;
        }


        public string getDefaultFilename()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + System.IO.Path.PathSeparator + "projector_bookmarks.def";
        }

        public BookmarkManager()
        {
        }

        public BookmarkManager(SerializationInfo info, StreamingContext ctxt)
        {
            this.bookMarks = (List<QueryBookMark>)info.GetValue("bookMarks", typeof(List<QueryBookMark>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("bookMarks", this.bookMarks);
        }
    }
}
