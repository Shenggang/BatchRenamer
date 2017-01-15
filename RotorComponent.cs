using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class RotorComponent : NameComponent
        {
            private String[] stringSet;

            public RotorComponent(String[] set)
            {
                stringSet = set;
            }

            public String StringSet
            {
                get
                {
                    String s = "";
                    foreach(string a in stringSet)
                    {
                        s = s + a + ":";
                    }
                    s = s.Remove(s.Length - 1);
                    return s;
                }
                set
                {
                    stringSet = value.ToString().Split(':');
                    for (int i = 0; i < stringSet.Length; i++)
                    {
                        stringSet[i] = ValidateName(stringSet[i]);
                    }
                }
            }

            public int count
            {
                get { return stringSet.Length; }
            }

            public override void InitialiseNeededValues(object o) { }

            public override string ComponentToString(int index)
            {
                int length = stringSet.Length;
                return stringSet[index % length];
            }
        }
    }
}
