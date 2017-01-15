using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class StringComponent : NameComponent
        {
            private string content = "";

            public StringComponent(string content)
            {
                this.content = content;
            }

            public string Content
            {
                get{ return content; }
                set { this.content = ValidateName(value); }
            }

            public override void InitialiseNeededValues(object o) { }

            public override string ComponentToString(int index)
            {
                return content;
            }
        }
    }
}
