using System.Data.SqlClient;

namespace CSharp.CodeGenerator.Core
{
    public class GeneratorService
    {

        
        
        
        public void GetAllTable()
        {
            //获取所有表信息
            var sql = @"SELECT object_id TableId,name TableName,create_date CreateByDate FROM sys.objects WHERE type='U' ORDER BY CreateByDate DESC ";

        }
        
        public void Create()
        {
            var conn = new SqlConnection(ConnectionManager.ConnectionString);
            /* 1、查询表名称、字段*/

            var sql = @"SELECT obj.name,columns.COLUMN_NAME,columns.DATA_TYPE,columns.IS_NULLABLE,obj.name,prop.value
                        FROM INFORMATION_SCHEMA.COLUMNS columns
                        INNER JOIN sys.objects obj ON columns.TABLE_NAME=obj.name
                        LEFT JOIN sys.extended_properties prop ON prop.major_id=obj.object_id AND columns.ORDINAL_POSITION=prop.minor_id
                        WHERE obj.object_id IN @TableIds";

            //查询表描述
            var queryTableDescSql = @"SELECT obj.value FROM sys.extended_properties obj WHERE obj.object_id IN @TableIds AND prop.minor_id=0 ";
        }
    }
}