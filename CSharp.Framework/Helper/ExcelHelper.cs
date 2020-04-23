﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CSharp.Framework.Extensions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CSharp.Framework.Helper
{
    /// <summary>
    /// Excel帮助类 - 依赖NPOI
    /// </summary>
    public class ExcelHelper
    {
        private static readonly int _defaultHeaderNum = 0;
        private static readonly object _excelFileReadLocker = new object();

        private static readonly Dictionary<string, PropertyInfo[]> ModelMap = new Dictionary<string, PropertyInfo[]>();
        private static readonly Dictionary<string, string> ModelEnumMap = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> FieldDescMap = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> FieldDescRelationMapper = new Dictionary<string, string>();
        private static Dictionary<string, PropertyInfo> FieldPropertyInfoMap = new Dictionary<string, PropertyInfo>();

        private static string _dateTimeFormat = "yyyy-MM-dd hh:mm:ss";

        public static void SetDateTimeFormat(string dateTimeFormat)
        {
            _dateTimeFormat = dateTimeFormat;
        }


        #region read

        /// <summary>
        /// 根据excel路径读取 - 位置关联生成动态数据集合
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="dynamicFieldMapper"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="containsHeader"></param>
        /// <returns></returns>
        public static List<dynamic> Read(string excelPath, Dictionary<int, string> dynamicFieldMapper = null, int sheetIndex = 0, bool containsHeader = true)
        {
            var stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            return Read(stream, dynamicFieldMapper, sheetIndex, containsHeader);
        }

        /// <summary>
        /// 根据excel流读取 - 位置关联生成动态数据集合
        /// </summary>
        /// <param name="excelStream"></param>
        /// <param name="dynamicFieldMapper"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="containsHeader"></param>
        /// <returns></returns>
        public static List<dynamic> Read(Stream excelStream, Dictionary<int, string> dynamicFieldMapper = null, int sheetIndex = 0, bool containsHeader = true)
        {
            var workbook = CreateWorkBook(excelStream);

            var sheet = workbook.GetSheetAt(sheetIndex);

            dynamicFieldMapper = GetDynamicReadMapper(dynamicFieldMapper, sheet.GetRow(0)?.PhysicalNumberOfCells ?? 0);
            var startIndex = containsHeader ? _defaultHeaderNum + 1 : 0;
            var result = new List<dynamic>();
            for (var i = startIndex; i < sheet.PhysicalNumberOfRows; i++)
            {
                var readRow = sheet.GetRow(i);

                if (readRow == null) continue;

                var model = new ExcelDynamicObject();
                for (var j = 0; j < readRow.PhysicalNumberOfCells; j++)
                {
                    //字典存在映射关系才进行赋值
                    if (!dynamicFieldMapper.ContainsKey(j)) continue;
                    var cellValue = GetCellValue(readRow.GetCell(j));

                    model.AddProperty(dynamicFieldMapper[j], cellValue);
                }

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 根据excel路径读取 - 表头（标题）关联生成动态数据集合
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="dynamicFieldMapper"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        public static List<dynamic> Read(string excelPath, Dictionary<string, string> dynamicFieldMapper, int sheetIndex = 0)
        {
            var stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            return Read(stream, dynamicFieldMapper, sheetIndex);
        }

        /// <summary>
        /// 根据excel流读取 - 表头（标题）关联生成动态数据集合
        /// </summary>
        /// <param name="excelStream"></param>
        /// <param name="dynamicFieldMapper"></param>
        /// <param name="sheetIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<dynamic> Read(Stream excelStream, Dictionary<string, string> dynamicFieldMapper, int sheetIndex = 0)
        {
            if (!(dynamicFieldMapper?.Any() ?? false)) throw new Exception("表头对应关系不能为空！可以尝试不传该参数。");

            var workbook = CreateWorkBook(excelStream);

            var sheet = workbook.GetSheetAt(sheetIndex);

            var headerMap = new Dictionary<int, string>();
            var headerRow = sheet.GetRow(_defaultHeaderNum);
            if (headerRow != null)
            {
                for (var i = 0; i < headerRow.PhysicalNumberOfCells; i++)
                {
                    headerMap.Add(i, headerRow.GetCell(i)?.StringCellValue);
                }
            }

            var startIndex = _defaultHeaderNum + 1;
            var result = new List<dynamic>();
            for (var i = startIndex; i < sheet.PhysicalNumberOfRows; i++)
            {
                var readRow = sheet.GetRow(i);
                if (readRow != null)
                {
                    var model = new ExcelDynamicObject();
                    for (var j = 0; j < readRow.PhysicalNumberOfCells; j++)
                    {
                        //字典存在映射关系才进行赋值
                        if (!headerMap.ContainsKey(j)) continue;

                        var curHeaderName = headerMap[j];
                        if (string.IsNullOrEmpty(curHeaderName) || !dynamicFieldMapper.ContainsKey(curHeaderName)) continue;

                        var cellValue = GetCellValue(readRow.GetCell(j));

                        model.AddProperty(dynamicFieldMapper[curHeaderName], cellValue);
                    }

                    result.Add(model);
                }
            }

            return result;
        }


        /// <summary>
        /// 根据excel路径读取 - 返回泛型集合
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="fieldMapperList"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="headerIndex"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Read<T>(string excelPath, Dictionary<int, string> fieldMapperList = null, int sheetIndex = 0, int headerIndex = 0) where T : class, new()
        {
            var stream = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            return Read<T>(stream, fieldMapperList, sheetIndex, headerIndex);
        }

        /// <summary>
        /// 根据excel流读取 - 返回泛型集合
        /// </summary>
        /// <param name="excelStream"></param>
        /// <param name="fieldMapperList"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="headerIndex"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Read<T>(Stream excelStream, Dictionary<int, string> fieldMapperList = null, int sheetIndex = 0, int headerIndex = 0) where T : class, new()
        {
            var workbook = CreateWorkBook(excelStream);

            var mapperModel = new T();
            fieldMapperList = GetReadMapperList(mapperModel, fieldMapperList);
            var result = new List<T>();

            var sheet = workbook.GetSheetAt(sheetIndex);

            for (var i = headerIndex + 1; i < sheet.PhysicalNumberOfRows; i++)
            {
                var readRow = sheet.GetRow(i);
                if (readRow != null)
                {
                    var model = new T();
                    for (var j = 0; j < readRow.PhysicalNumberOfCells; j++)
                    {
                        //字典存在映射关系才进行赋值
                        if (!fieldMapperList.ContainsKey(j)) continue;

                        var cellValue = GetCellValue(readRow.GetCell(j));

                        SetFieldValue(model, fieldMapperList[j], cellValue);
                    }

                    result.Add(model);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取单元格的值
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            var cellValue = string.Empty;
            if (cell == null) return cellValue;

            if (cell.CellType == CellType.Numeric)
            {
                var format = cell.CellStyle.DataFormat;
                cellValue = format > 0 ? cell.DateCellValue.ToString(_dateTimeFormat) : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                cell.SetCellType(CellType.String);
                cellValue = cell.StringCellValue;
            }

            return cellValue;
        }


        /// <summary>
        /// 创建Excel WorkBook
        /// </summary>
        /// <param name="excelStream"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static IWorkbook CreateWorkBook(Stream excelStream)
        {
            IWorkbook workbook;
            try
            {
                workbook = new XSSFWorkbook(excelStream);
            }
            catch
            {
                try
                {
                    workbook = new HSSFWorkbook(excelStream);
                }
                catch
                {
                    throw new Exception("excel格式错误！请检查excel后缀是否为xls或xlsx。");
                }
            }

            return workbook;
        }

        #endregion

        #region export

        //todo 多sheet 导出

        /// <summary>
        /// 根据实体集合生成excel 流
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="fieldMapperList"></param>
        /// <param name="sheetName"></param>
        /// <param name="containsHeader"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Stream Export<T>(List<T> excelData, Dictionary<string, string> fieldMapperList = null, string sheetName = "sheet1",
            bool containsHeader = true) where T : class, new()
        {
            if (!excelData?.Any() ?? true) throw new Exception("export empty!");
            //  with read at compare add exists excel file template mapper
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);

            fieldMapperList = GetMapperList(excelData[0], fieldMapperList);

            //sheet当前写入行位置
            var rowNum = 0;
            if (containsHeader)
            {
                var headerRow = sheet.CreateRow(rowNum);

                rowNum++;
                //表头单元格位置
                var headerCellNum = 0;
                foreach (var fieldMapper in fieldMapperList)
                {
                    headerRow
                        .CreateCell(headerCellNum)
                        .SetCellValue(fieldMapper.Value);
                    headerCellNum++;
                }
            }

            //写入内容
            foreach (var rowData in excelData)
            {
                var row = sheet.CreateRow(rowNum);
                var curCellNum = 0;
                foreach (var fieldMapper in fieldMapperList)
                {
                    row
                        .CreateCell(curCellNum)
                        .SetCellValue(GetFieldValue(rowData, fieldMapper.Key));

                    curCellNum++;
                }

                rowNum++;
            }

            var stream = new MemoryStream();
            workbook.Write(stream, true);

            return stream;
        }


        /// <summary>
        /// 根据已有excel模板导出
        /// </summary>
        /// <param name="excelData"></param>
        /// <param name="templatePath">模板相对路径</param>
        /// <param name="templateRelationRowIndex">模板语法所在行</param>
        /// <param name="sheetIndex">读取的sheet</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Stream Export<T>(List<T> excelData, [NotNull] string templatePath, int templateRelationRowIndex = 1, int sheetIndex = 0) where T : class, new()
        {
            //读取模版关联
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? "", templatePath);
            if (!File.Exists(path))
                throw new Exception($"Excel模板文件不存在！");

            IWorkbook workbook;

            var buffer = ReadFileBuffer(path); //File.ReadAllBytes(path);
            var stream = new MemoryStream(buffer.ToArray());
            var fileExtension = Path.GetExtension(path).ToLower();
            workbook = fileExtension switch
            {
                ".xlsx" => new XSSFWorkbook(stream),
                ".xls" => new HSSFWorkbook(stream),
                _ => new XSSFWorkbook(stream),
            };

            var sheet = workbook.GetSheetAt(sheetIndex);

            var fieldMapperList = new Dictionary<int, string>();

            var relationRow = sheet.GetRow(templateRelationRowIndex);
            if (relationRow == null) throw new Exception("模板未生成字段关联，具体语法为'$FieldName'(PS：不需要单引号)。");

            for (var i = 0; i < relationRow.PhysicalNumberOfCells; i++)
            {
                var relationCode = relationRow.GetCell(i)
                    ?.StringCellValue;
                if (string.IsNullOrEmpty(relationCode)) throw new Exception($"第{i + 1}行模板语法为空！");

                var splitRelationCode = relationCode.Split('$', StringSplitOptions.RemoveEmptyEntries);
                if (splitRelationCode.Length != 1) throw new Exception($"第{i + 1}行模板语法错误！");
                var relation = splitRelationCode[0].Trim();
                fieldMapperList.Add(i, relation);
            }


            //写入
            var writeRowIndex = templateRelationRowIndex;
            foreach (var rowData in excelData)
            {
                foreach (var (key, value) in fieldMapperList)
                {
                    SetAndCreateCellValue(rowData, sheet, writeRowIndex, key, value);
                }

                writeRowIndex++;
            }

            var result = new MemoryStream();
            if (workbook is XSSFWorkbook xssfWorkbook)
                xssfWorkbook.Write(result, true);
            else
                workbook.Write(result);
            
            return result;
        }

        #endregion

        #region Utils

        private static void SetAndCreateCellValue<T>(T model, ISheet sheet, int rowIndex, int cellIndex, string fieldName)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            (row.GetCell(cellIndex) ?? row.CreateCell(cellIndex))
                .SetCellValue(GetFieldValue(model, fieldName));
        }

        private static byte[] ReadFileBuffer(string path)
        {
            lock (_excelFileReadLocker)
            {
                return File.ReadAllBytes(path);
            }
        }

        private static Dictionary<int, string> GetDynamicReadMapper(Dictionary<int, string> dynamicFieldMapper, int cellNumber)
        {
            if (dynamicFieldMapper?.Any() ?? false) return dynamicFieldMapper;

            dynamicFieldMapper = new Dictionary<int, string>();
            for (int i = 0; i < cellNumber; i++)
            {
                dynamicFieldMapper.Add(i, $"item{i}");
            }

            return dynamicFieldMapper;
        }

        /// <summary>
        /// 根据description 获取对应的字段名
        /// </summary>
        /// <param name="model"></param>
        /// <param name="descName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetFieldNameByDesc<T>(T model, string descName)
        {
            var modelType = model.GetType().ToString();
            var key = $"{modelType}.{descName}";
            if (FieldDescRelationMapper.ContainsKey(key)) return FieldDescRelationMapper[key];

            var propertyInfos = GetModelMap(model);
            foreach (var propertyInfo in propertyInfos)
            {
                var desc = GetFieldDesc(model, propertyInfo.Name);
                if (!string.IsNullOrEmpty(desc))
                {
                    key = $"{modelType}.{desc}";
                    FieldDescRelationMapper.Add(key, propertyInfo.Name);
                    if (desc == descName) descName = propertyInfo.Name;
                }
            }

            return descName;
        }

        /// <summary>
        /// 设置反射对象的字段值
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        private static void SetFieldValue<T>(T model, string fieldName, string value)
        {
            fieldName = GetFieldNameByDesc(model, fieldName);

            object val = null;
            var propertyInfo = GetFieldPropertyInfo(model, fieldName);

            if (propertyInfo == null) return;

            if (!propertyInfo.PropertyType.IsGenericType)
            {
                val = string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, propertyInfo.PropertyType);
            }
            else if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                val = string.IsNullOrEmpty(value) ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(propertyInfo.PropertyType));
            }

            GetModelMap(model)
                .FirstOrDefault(f => f.Name == fieldName)
                ?.SetValue(model, val);
        }

        /// <summary>
        /// 获取实体字段信息Mapper
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static PropertyInfo[] GetModelMap<T>(T model)
        {
            var key = model.GetType().ToString();
            if (ModelMap.ContainsKey(key)) return ModelMap[key];

            var propertyInfos = model.GetType().GetProperties();
            ModelMap.Add(key, propertyInfos);
            return propertyInfos;
        }


        private static PropertyInfo GetFieldPropertyInfo<T>(T model, string fieldName)
        {
            var key = $"{model.GetType().ToString()}.{fieldName}";
            if (FieldPropertyInfoMap.ContainsKey(key)) return FieldPropertyInfoMap[key];

            var fieldPropertyInfo = GetModelMap(model).FirstOrDefault(f => f.Name == fieldName);
            FieldPropertyInfoMap.Add(key, fieldPropertyInfo);
            return fieldPropertyInfo;
        }

        /// <summary>
        /// 获取实体对应字段description
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetFieldDesc<T>(T model, string fieldName)
        {
            var key = $"{model.GetType().ToString()}.{fieldName}";
            if (FieldDescMap.ContainsKey(key)) return FieldDescMap[key];

            var descAttr = GetModelMap(model).FirstOrDefault(f => f.Name == fieldName)
                ?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            var desc = descAttr?.Any() ?? false ? ((DescriptionAttribute) descAttr[0]).Description : null;
            FieldDescMap.Add(key, desc);
            return desc;
        }

        /// <summary>
        /// 获取实体对应字段的值
        /// </summary>
        /// <param name="rowData"></param>
        /// <param name="fieldName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetFieldValue<T>(T rowData, string fieldName)
        {
            var fieldPropertyInfo = GetModelMap(rowData).FirstOrDefault(f => f.Name == fieldName);

            if (fieldPropertyInfo == null)
            {
                // 返回ExcelDynamicObject FieldValue
                return GetDynamicObjectValue(rowData, fieldName);
            }

            var fieldValue = fieldPropertyInfo.GetValue(rowData, null);

            if (fieldValue == null) return null;

            return fieldPropertyInfo.PropertyType.IsEnum ? GetEnumValue(fieldValue) : GetFieldStringValue(fieldValue);
        }

        private static string GetDynamicObjectValue<T>(T rowData, string key)
        {
            var fieldValue = string.Empty;
            if (rowData is ExcelDynamicObject model)
            {
                fieldValue = GetFieldStringValue(model.GetValue(key));
            }

            return fieldValue;
        }

        private static string GetFieldStringValue(object fieldValue)
        {
            //处理枚举
            if (fieldValue.GetType().IsEnum)
            {
                var enumValue = GetEnumValue(fieldValue);
                if (!string.IsNullOrEmpty(enumValue)) return enumValue;
            }

            //其余为日期和字符串
            var fieldType = fieldValue.GetType().ToString();
            return fieldType switch
            {
                "System.DateTime" => Convert.ToDateTime(fieldValue).ToString(_dateTimeFormat),
                _ => fieldValue?.ToString()
            };
        }


        /// <summary>
        /// 获取枚举值
        /// 优先级：
        /// description
        /// ToString()
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        private static string GetEnumValue(object fieldValue)
        {
            var type = fieldValue.GetType();
            var enumValue = fieldValue?.ToString();
            var key = $"{type.FullName}.{enumValue}";
            if (ModelEnumMap.ContainsKey(key)) return ModelEnumMap[key];

            if (string.IsNullOrEmpty(enumValue)) return null;

            var val = (Enum) fieldValue;
            var fieldInfo = val.GetType().GetField(enumValue);
            if (fieldInfo == null)
            {
                ModelEnumMap.Add(key, null);
                return null;
            }

            var descAttr = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            var result = descAttr.Any() ? ((DescriptionAttribute) descAttr[0]).Description : enumValue;
            ModelEnumMap.Add(key, result);
            return result;
        }

        /// <summary>
        /// 获取需要导出的实体字段对应关系
        /// 优先级：
        /// 1、根据传来的mapper
        /// 2、根据实体字段的description属性
        /// 3、根据字段名
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldMapperList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Dictionary<string, string> GetMapperList<T>(T model, Dictionary<string, string> fieldMapperList)
        {
            //1、from fieldMapper
            if (fieldMapperList?.Any() ?? false)
                return fieldMapperList;

            fieldMapperList = new Dictionary<string, string>();
            //2、from field description attribute 
            var fieldNameList = new List<string>();
            var propertyInfos = GetModelMap(model);
            foreach (var propertyInfo in propertyInfos)
            {
                var desc = GetFieldDesc(model, propertyInfo.Name);
                if (!string.IsNullOrEmpty(desc)) fieldMapperList.Add(propertyInfo.Name, desc);
                fieldNameList.Add(propertyInfo.Name);
            }

            if (fieldMapperList.Any()) return fieldMapperList;
            //3、from field name
            foreach (var fieldName in fieldNameList)
            {
                fieldMapperList.Add(fieldName, fieldName);
            }


            if (fieldMapperList.Any()) return fieldMapperList;

            //4、from DynamicObjectExtension 
            if (model is ExcelDynamicObject dynamicObject)
            {
                fieldMapperList = dynamicObject.GetAllKeys()
                    .ToDictionary(key => key, value => value);
            }

            return fieldMapperList;
        }


        /// <summary>
        /// 获取需要导出的实体字段对应关系
        /// 优先级：
        /// 1、根据传来的mapper
        /// 2、根据实体字段的description属性
        /// 3、根据字段名
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldMapperList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Dictionary<int, string> GetReadMapperList<T>(T model, Dictionary<int, string> fieldMapperList)
        {
            //1、from fieldMapper
            if (fieldMapperList?.Any() ?? false)
                return fieldMapperList;

            fieldMapperList = new Dictionary<int, string>();
            //2、from field description attribute 
            var fieldIndex = 0;
            var fieldNameList = new List<string>();
            var propertyInfos = GetModelMap(model);
            foreach (var propertyInfo in propertyInfos)
            {
                var desc = GetFieldDesc(model, propertyInfo.Name);
                if (!string.IsNullOrEmpty(desc))
                {
                    fieldMapperList.Add(fieldIndex, desc);
                    fieldIndex++;
                }

                fieldNameList.Add(propertyInfo.Name);
            }

            if (fieldMapperList.Any()) return fieldMapperList;
            //3、from field name
            fieldIndex = 0;
            foreach (var fieldName in fieldNameList)
            {
                fieldMapperList.Add(fieldIndex, fieldName);
                fieldIndex++;
            }

            return fieldMapperList;
        }

        #endregion
    }
}