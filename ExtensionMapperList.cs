/**************************************************************
 * Copyright (c) 2017. Shenggang Hu.
 * All rights reserved.
 **************************************************************/

using System;
using System.Collections.Generic;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class ExtensionMapperList
        {
            private List<ExtensionMapper> list = new List<ExtensionMapper>();

            public ExtensionMapperList(List<ExtensionMapper> list)
            {
                this.list = list;
            }

            public ExtensionMapperList() { }

            public List<ExtensionMapper> getList()
            {
                return list;
            }

            public void add(ExtensionMapper em)
            {
                list.Add(em);
            }

            public void removeTheLast()
            {
                int count = list.Count;
                if (list.Count !=0)
                {
                    list.RemoveAt(count - 1);
                }
            }

            public String getResultExtensoin(string extension)
            {
                String s = extension;
                if (list.Count != 0)
                {
                    foreach (ExtensionMapper em in list)
                    {
                        string result = em.getExtensionIfMatch(extension);
                        if (!result.Equals("ExtensionNotMatched"))
                        {
                            s = result;
                        }
                    }
                }
                return s;
            }
        }
    }
}
