namespace Iriya.Libs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Attribute情報クラス
    /// </summary>
    public class AttributeInfoData
    {

        /// <summary>
        /// クラスの型を取得します。
        /// </summary>
        public Type Class
        {
            get;
            internal set;
        }

        /// <summary>
        /// メソッド情報を取得します。
        /// </summary>
        public MemberInfo Method
        {
            get;
            internal set;
        }

        /// <summary>
        /// 対象のメソッドのAttribute配列を取得します。
        /// </summary>
        public Attribute[] Attributes
        {
            get;
            internal set;
        }

    }
}
