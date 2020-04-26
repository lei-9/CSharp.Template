namespace CSharp.CodeGenerator.Models
{
    /// <summary>
    /// 生成代码对象
    /// </summary>
    public class GeneratorModel
    {
        /// <summary>
        /// 表id
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 要生成的持久对象名
        /// </summary>
        public string PersistentObjectName { get; set; }
        
        
    }
}