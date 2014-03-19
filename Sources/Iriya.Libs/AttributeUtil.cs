namespace Iriya.Libs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Attributeユーティリティークラス
    /// </summary>
    public class AttributeUtil
    {
        /// <summary>
        /// アセンブリのファイルパスとクラスのAttribute名称を指定して、
        /// 該当するAttribute情報を取得します。
        /// </summary>
        /// <param name="assemblyPaths">検索対象のアセンブリファイルパス</param>
        /// <param name="attributeNames">検索対象のクラスのAttribute名称</param>
        /// <returns>Attribute情報</returns>
        public static IList<AttributeInfoData> GetAttributeInfoData(string[] assemblyPaths, string[] attributeNames)
        {
            if (assemblyPaths == null) throw new ArgumentNullException("assemblyPaths is null.");
            if (attributeNames == null) throw new ArgumentNullException("attributeNames is null.");

            IList<Type> attributeTypes = new List<Type>();
            
            foreach (string attrName in attributeNames)
            {
                Type attrType = Type.GetType(attrName);
                attributeTypes.Add(attrType);
            }

            return GetAttributeInfoData(assemblyPaths, attributeTypes.ToArray());
        }

        /// <summary>
        /// アセンブリのファイルパスとクラスのAttributeの型を指定して、
        /// 該当するAttribute情報を取得します。
        /// </summary>
        /// <param name="assemblyPaths">検索対象のアセンブリファイルパス</param>
        /// <param name="attributeNames">検索対象のクラスのAttributeの型</param>
        /// <returns>Attribute情報</returns>
        public static IList<AttributeInfoData> GetAttributeInfoData(string[] assemblyPaths, Type[] attributeTypes)
        {
            if (assemblyPaths == null) throw new ArgumentNullException("assemblyPaths is null.");
            if (attributeTypes == null) throw new ArgumentNullException("attributeTypes is null.");

            IList<AttributeInfoData> list = new List<AttributeInfoData>();

            foreach (string asmPath in assemblyPaths)
            {
                if (!File.Exists(asmPath)) throw new FileNotFoundException("file not found.", asmPath);

                Assembly asm = Assembly.LoadFrom(asmPath);

                foreach (Type t in asm.GetTypes())
                {
                    if (ContainAttribute(t, attributeTypes))
                    {
                        foreach (MethodInfo m in t.GetMethods())
                        {
                            if (m.GetCustomAttributes(true).Length > 0)
                            {
                                IList<Attribute> attrs = new List<Attribute>();
                                foreach (object o in m.GetCustomAttributes(true))
                                {
                                    if (o is Attribute)
                                        attrs.Add(o as Attribute);
                                }

                                AttributeInfoData data = new AttributeInfoData();
                                data.Class      = t;
                                data.Method     = m;
                                data.Attributes = attrs.ToArray();
                                list.Add(data);
                            }
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// クラスに対して該当するAttributeが含まれているか確認します。
        /// </summary>
        /// <param name="type">クラスの型</param>
        /// <param name="searchAttributes">検索対象のAttribute</param>
        /// <returns>
        /// 該当するAttributeが含まれている場合、true。
        /// 含まれていない場合、false。
        /// </returns>
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
