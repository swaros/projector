using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projector
{
    class MysqlCreateStructContainer
    {
        private List<MysqlCreateStruct> crList = new List<MysqlCreateStruct>();
        private List<MysqlIndexType> crIndizies = new List<MysqlIndexType>();
        private string tableName;
        private string engine;

        public Boolean temporary = false;



        public void setTablename(string tablename)
        {
            this.tableName = tablename;
        }

        public void setEngine(string setEngine)
        {
            this.engine = setEngine;
        }

        public bool addStruct(MysqlCreateStruct mStruct)
        {
            if (mStruct.name.Length > 0 && getStructByName(mStruct.name) == null)
            {
                this.crList.Add(mStruct);
                return true;
            }
            return false;
        }

        public string getCreationSql()
        {
            string crStr = "CREATE TABLE " + this.tableName + " (" + Environment.NewLine;

            List<String> primary = new List<string>();

            string add = "";
            for (int i = 0; i < this.crList.Count; i++)
            {
                crStr += add + this.crList[i].getSql();
                add = "," + Environment.NewLine;

                if (this.crList[i].isPrimary)
                {
                    primary.Add(this.crList[i].name);
                }

            }

            if (primary.Count > 0)
            {
                crStr += add + "PRIMARY KEY (";
                string dAdd = "";
                for (int i = 0; i < primary.Count; i++)
                {
                    crStr += dAdd + "`" + primary[i] + "`";
                    dAdd = ",";
                }
                crStr += ")";
            }

            if (crIndizies.Count > 0)
            {
                string dAdd = "";
                for (int a = 0; a < crIndizies.Count; a++)
                {
                    crStr += add + " KEY " + crIndizies[a].getName() + " (" + crIndizies[a].getFieldString() + ")";                                        
                    dAdd = "," + Environment.NewLine;                    
                }
            }


            crStr += Environment.NewLine + ")";
            crStr += Environment.NewLine + " ENGINE=" + this.engine;

            

            return crStr;
        }


        /**
         * add index by the name of keys
         */ 
        public void addIndex(string name, List<string> keys)
        {
            if (null == this.getIndexbyName(name))
            {
                MysqlIndexType newIndex = new MysqlIndexType(name);

                for (int i = 0; i < keys.Count; i++)
                {
                    MysqlCreateStruct getStruct = this.getStructByName(keys[i]);
                    if (getStruct != null)
                    {
                        newIndex.addRefererStruct(getStruct);
                    }
                    else
                    {
                        throw new Exception("key " + keys[i] + " not exists");
                    }

                    this.crIndizies.Add(newIndex);
                }
            }
            else
            {
                throw new Exception("Index with name " + name + " allready exists");
            }
        }

        /**
         * returns the index assigned to name (or retunrs null)
         */ 
        private MysqlIndexType getIndexbyName(string name)
        {
            for (int i = 0; i < this.crIndizies.Count; i++)
            {
                if (this.crIndizies[i].getName() == name) return this.crIndizies[i];
            }
            return null;
        }

        public void addFieldToIndex(string name, string fieldName)
        {
            MysqlIndexType editIndex = this.getIndexbyName(name);
            if (null != editIndex)
            {
                MysqlCreateStruct getStruct = this.getStructByName(fieldName);
                if (getStruct != null)
                {
                    editIndex.addRefererStruct(getStruct);
                }
                else
                {
                    throw new Exception("key " + fieldName + " not exists");
                }
            }
        }


        public void updateIndex(string name, List<string> keys)
        {
            MysqlIndexType editIndex = this.getIndexbyName(name);
            if (null != editIndex)
            {
                
                for (int i = 0; i < keys.Count; i++)
                {
                    MysqlCreateStruct getStruct = this.getStructByName(keys[i]);
                    if (getStruct != null)
                    {
                        editIndex.addRefererStruct(getStruct);
                    }
                    else
                    {
                        throw new Exception("key " + keys[i] + " not exists");
                    }

                    //this.crIndizies.Add(newIndex);
                }
            }
            else
            {
                throw new Exception("Index with name " + name + " allready exists");
            }
        }


        /**
         * find the struct by name and returns
         * the struct...otherwise it returns null
         */ 
        private MysqlCreateStruct getStructByName(string name)
        {
            for (int i = 0; i < this.crList.Count; i++)
            {
                if (crList[i].name == name) return crList[i];
            }
            return null;
        }

    }
}
