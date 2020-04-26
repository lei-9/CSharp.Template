using System;

namespace CSharp.CodeGenerator.Models
{
    /// <summary>
    /// 表字段信息对象
    /// </summary>
    public class FieldInfoModel
    {
        /// <summary>
        /// 表id
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string Prop { get; set; }
    }
}