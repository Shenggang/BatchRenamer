using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class ExtensionMapper
        {
            private String inputExtensions;
            private String outputExtension;

            public ExtensionMapper(String input, String output)
            {
                inputExtensions = input;
                outputExtension = output;
            }

            public String InputExtensions
            {
                set { inputExtensions = value; }
            }

            public String OutputExtension
            {
                get { return outputExtension; }
                set { outputExtension = value; }
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
                return inputExtensions.Contains(extension);
            }
        }
    }

}
