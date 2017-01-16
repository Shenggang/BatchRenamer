
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

            public override void InitialiseRequiredFields(object o) { }

            public override string ComponentToString(int index)
            {
                return content;
            }
        }
    }
}
