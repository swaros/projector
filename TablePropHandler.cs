using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Projector
{

    [Serializable()]
    class TablePropHandler : ISerializable
    {
        private List<TableProp> props = new List<TableProp>();

        public TablePropHandler()
        {
        }

        public TableProp getProp(string tableName)
        {
            for (int i = 0; i < this.props.Count; i++)
            {
                if (this.props[i].assignedTable == tableName) return this.props[i];
            }
            return null;
        }

        public Boolean setExitstsProp(TableProp property)
        {
            for (int i = 0; i < this.props.Count; i++)
            {
                if (this.props[i].assignedTable == property.assignedTable)
                {
                    this.props[i] = property;
                    return true;
                }
            }
            return false;
        }

        public void setTableProp(TableProp property)
        {
            TableProp tmpProp = this.getProp(property.assignedTable);
            if (tmpProp != null)
            {
                if (!this.setExitstsProp(property))
                {
                    throw new Exception("Failed to set property ");
                }
            }
            else
            {
                this.props.Add(property);
            }
        }

        public TablePropHandler(SerializationInfo info, StreamingContext ctxt)
        {
            this.props = (List<TableProp>)info.GetValue("Prop", typeof(List<TableProp>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Prop", this.props);
        }

    }
}
