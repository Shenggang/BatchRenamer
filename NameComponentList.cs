/**************************************************************
 * Copyright (c) 2017. Shenggang Hu.
 * All rights reserved.
 **************************************************************/

using System.Collections.Generic;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class NameComponentList
        {
            private List<NameComponent> list = new List<NameComponent>();

            public NameComponentList() { }

            public NameComponentList(List<NameComponent> list)
            {
                this.list = list;
            }

            public int Count
            {
                get { return list.Count; }
            }

            public List<NameComponent> getList()
            {
                return list;
            }

            public void Add(NameComponent c)
            {
                list.Add(c);
            }

            public void removeTheLast()
            {
                int count = list.Count;
                if (list.Count != 0)
                {
                    list.RemoveAt(count - 1);
                }
            }

            public void InitialistRequiredFields(object o)
            {
                foreach(NameComponent c in list)
                {
                    c.InitialiseRequiredFields(o);
                }
            }

            public string ComposeNewName(int index)
            {
                string name = "";
                foreach(NameComponent c in list)
                {
                    name = name + c.ComponentToString(index);
                }
                return name;
            }
        }
    }
}
