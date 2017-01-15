using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    namespace RenameTools
    {
        public class CounterComponent : NameComponent
        {
            private int startNumber;
            private int step;
            private int minDigit;

            public CounterComponent(int startNumber, int step)
            {
                this.startNumber = startNumber;
                this.step = step;
            }

            public int StartNumber
            {
                get { return startNumber; }
                set { startNumber = value; }
            }

            public int Step
            {
                get { return step; }
                set { step = value; }
            }

            public override void InitialiseNeededValues(object o)
            {
                int count = (int)o;
                CalculateMinimumDigit(count);
            }

            public void CalculateMinimumDigit(int count)
            {
                minDigit = (int) Math.Ceiling(Math.Log10((startNumber + (count - 1) * step)));
            }

            public override string ComponentToString(int index)
            {
                int number = startNumber + index * step;
                string s = number.ToString();
                while (s.Length < minDigit)
                {
                    s = "0" + s;
                }
                return s;
            }
        }
    }
}
