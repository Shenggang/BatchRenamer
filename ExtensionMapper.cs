using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class ExtensionMapper
        {
            private static char[] invalidChars = new char[] { '\\', '/', '<', '>', '|', ' ' };

            public static string ValidateExtension(string name)
            {
                string s = "";
                foreach (char c in name)
                {
                    if (!invalidChars.Contains(c))
                    {
                        s = s + c.ToString();
                    }
                }
                return s;
            }

            private String inputExtensions;
            private String outputExtension;

            public ExtensionMapper(String input, String output)
            {
                inputExtensions = input;
                outputExtension = output;
            }

            public ExtensionMapper() { }

            public String InputExtensions
            {
                get { return inputExtensions; }
                set { inputExtensions = ValidateExtension(value); }
            }

            public String OutputExtension
            {
                get { return outputExtension; }
                set
                {
                    String[] s = NameComponent.ValidateName(value).Split(' ');
                    outputExtension = "";
                    foreach (String str in s)
                    {
                        outputExtension = outputExtension + str;
                    }
                }
            }

            public String getExtensionIfMatch(String extension)
            {
                if (checkIfMatch(extension))
                {
                    return OutputExtension;
                }
                return null;
            }

            private bool checkIfMatch(String extension)
            {
                String[] extensions = inputExtensions.Split(':');
                foreach (string s in extensions)
                {
                    string epr = getCorrespondingRegex(s);
                    if (Regex.IsMatch(extension, epr)) { return true; }
                }
                return false;
            }

            private String getCorrespondingRegex(String s)
            {
                String epr = "";
                foreach(char c in s)
                {
                    switch(c)
                    {
                        case '*': epr += "\\w*";
                            break;
                        case '?': epr += "\\w";
                            break;
                        default: epr += c;
                            break;
                    }
                }
                return epr;
            }
        }
    }
}
