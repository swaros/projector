using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace Projector
{
    public class RefScrObjectStorage
    {
        public string originObjectName;
        public List<String> methodMask = new List<string>();
        public List<String> methodNames = new List<string>();
        public List<MethodInfo> methods = new List<MethodInfo>();
        public Hashtable Strings = new Hashtable();
        public Hashtable Integers = new Hashtable();
        public Hashtable Booleans = new Hashtable();
        public Hashtable Objects = new Hashtable();

    }
}
