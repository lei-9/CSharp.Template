using System;

namespace CSharp.CodeGenerator.Models
{
    /// <summary>
    /// 表信息对象
    /// </summary>
    public class TableInfoModel
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
        /// 描述
        /// </summary>
        public string Prop { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateByDate { get; set; }
    }
}