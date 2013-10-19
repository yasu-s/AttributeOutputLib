namespace Iriya.Libs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeOutputUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyPaths"></param>
        /// <param name="attributeNames"></param>
        /// <returns></returns>
        public static IList<AttributeData> GetAttributeData(string[] assemblyPaths, string[] attributeNames)
        {
            if (assemblyPaths == null) throw new ArgumentNullException("assemblyPaths is null.");
            if (attributeNames == null) throw new ArgumentNullException("attributeNames is null.");

            IList<Type> searchAttributes = new List<Type>();
            
            foreach (string attrName in attributeNames)
            {
                Type attrType = Type.GetType(attrName);
                searchAttributes.Add(attrType);
            }

            IList<AttributeData> list = new List<AttributeData>();

            foreach (string asmPath in attributeNames)
            {
                if (!File.Exists(asmPath)) throw new FileNotFoundException("file not found.", asmPath);

                Assembly asm = Assembly.LoadFrom(asmPath);

                foreach (Type t in asm.GetTypes())
                {
                    if (ContainAttribute(t, searchAttributes))
                    {
                        foreach (MethodInfo m in t.GetMethods())
                        {
                            if (m.GetCustomAttributes(true).Length > 0)
                            {
                                AttributeData data = new AttributeData();
                                data.Class = t;
                                data.Method = m;
                                data.Attributes = m.GetCustomAttributes(true) as Attribute[];
                                list.Add(data);
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="searchAttributes"></param>
        /// <returns></returns>
        private static bool ContainAttribute(Type type, IList<Type> searchAttributes)
        {
            foreach (Attribute attr in type.GetCustomAttributes(true))
            {
                foreach (Type attrType in searchAttributes)
                {
                    if (attr.GetType() == attrType)
                        return true;
                }
            }

            return false;
        }
    }
}
