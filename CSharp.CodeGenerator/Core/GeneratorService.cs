using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CSharp.CodeGenerator.Models;
using Dapper;

namespace CSharp.CodeGenerator.Core
{
    public class GeneratorService
    {
        /// <summary>
        /// 获取所有表信息
        /// </summary>
        /// <returns></returns>
        public List<TableInfoModel> GetAllTable()
        {
            //获取所有表信息
            var sql = @"SELECT t.object_id TableId,t.name TableName,prop.Value Prop,t.create_date CreateByDate FROM sys.tables t
                        LEFT JOIN sys.extended_properties prop ON t.object_id=prop.major_id AND prop.minor_id=0
                        ORDER BY CreateByDate DESC ";
            using var conn = new SqlConnection(ConnectionManager.ConnectionString);
            return conn.Query<TableInfoModel>(sql).ToList();
        }

        public void Create(List<string> tableIds)
        {
            if (!tableIds.Any()) return;

            //查询所有字段、字段描述、字段类型
            var queryFieldSql = @"SELECT t.object_id TableId,c.TABLE_NAME TableName,c.COLUMN_NAME FieldName, 
                                    (CASE c.IS_NULLABLE WHEN 'YES' THEN 1 ELSE 0 END) IsNullable
                                    ,c.DATA_TYPE DataType,prop.value Prop,c.ORDINAL_POSITION Position FROM INFORMATION_SCHEMA.COLUMNS c
                                    INNER JOIN sys.tables t ON c.TABLE_NAME=t.name
                                    INNER JOIN sys.extended_properties prop ON prop.minor_id=c.ORDINAL_POSITION
                                    WHERE t.object_id IN @TableIds
                                    ORDER BY c.ORDINAL_POSITION";

            //查询表描述 关联起来
            var queryTableSql = @"SELECT t.object_id TableId,t.name TableName,prop.Value Prop,t.create_date CreateByDate FROM sys.tables t
                                    LEFT JOIN sys.extended_properties prop ON t.object_id=prop.major_id AND prop.minor_id=0
                                    where t.object_id in @TableIds";
            var fieldInfos = new List<FieldInfoModel>();
            var tableInfos = new List<TableInfoModel>();
            using var conn = new SqlConnection(ConnectionManager.ConnectionString);
            fieldInfos = conn.Query<FieldInfoModel>(queryFieldSql, new
            {
                TableIds = tableIds
            }).ToList();

            tableInfos = conn.Query<TableInfoModel>(queryTableSql, new
            {
                TableIds = tableIds
            }).ToList();
            
        }
    }
}