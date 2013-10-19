namespace Iriya.Libs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class AttributeData
    {

        public Type Class
        {
            get;
            internal set;
        }

        public MemberInfo Method
        {
            get;
            internal set;
        }

        public Attribute[] Attributes
        {
            get;
            internal set;
        }

    }
}
