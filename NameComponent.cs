using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public abstract class NameComponent
        {
            private static char[] invalidChars = new char[] { '\\', '/', '<', '>', ':', '*', '?', '|' };

            public abstract string ComponentToString(int index);

            public abstract void InitialiseRequiredFields(object o);

            public static char[] InvalidCharacters
            {
                get { return invalidChars; }
            }

            public static string ValidateName(string name)
            {
                string s = "";
                foreach(char c in name)
                {
                    if (!invalidChars.Contains(c))
                    {
                        s = s + c.ToString();
                    }
                }
                return s;
            }
        }
    }
}
