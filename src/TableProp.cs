using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Projector
{
    [Serializable()]
    class TableProp : ISerializable
    {
        public string assignedTable;
        public string primaryField;
        public List<string> whereFiels = new List<string>();

        public TableProp()
        {
        }

        public void getWhereFieldsFromCheckedListBox(CheckedListBox whereList)
        {
            whereFiels.Clear();
            for (int z = 0; z < whereList.CheckedItems.Count; z++)
            {
                String comStr = whereList.CheckedItems[z].ToString();
                whereFiels.Add(comStr);
            }
        }

        public CheckedListBox setCheckedListBox(CheckedListBox whereList)
        {
            for (int z = 0; z < whereList.Items.Count; z++)
            {
                String comStr = whereList.Items[z].ToString();
                if (whereExists(comStr)) whereList.SetItemChecked(z, true);
                else whereList.SetItemChecked(z, false);
            }
            return whereList;
        }

        private Boolean whereExists(string what)
        {
            for (int i = 0; i < whereFiels.Count; i++)
            {
                if (whereFiels[i] == what) return true;
            }
            return false;
        }

        public TableProp(SerializationInfo info, StreamingContext ctxt)
        {
            this.assignedTable = (string)info.GetValue("asTab", typeof(string));
            this.primaryField = (string)info.GetValue("primField", typeof(string));
            this.whereFiels = (List<string>)info.GetValue("wheres", typeof(List<string>));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("asTab", this.assignedTable);
            info.AddValue("primField", this.primaryField);
            info.AddValue("wheres", this.whereFiels);
        }


    }
}
