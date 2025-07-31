using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NLS.Office
{
    /// <summary>
    /// Excel解释生成类
    /// </summary>
    public sealed class Excel
    {
        /// <summary>
        /// 生成Excel
        /// </summary>
        /// <typeparam name="T">源类型</typeparam>
        /// <param name="source">List类型源数据</param>
        /// <param name="coloums">列名</param>
        /// <param name="action">Action方法，结果为与列名所对应的字段排列数组</param>
        /// <param name="sheetName">sheet名</param>
        /// <returns>byte[]</returns>
        public static byte[] ExcelGeneration<T>(List<T> source, string[] coloums, Func<T, object[]> action, string sheetName) where T : class, new()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                //头
                for (int i = 0; i < coloums.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = coloums[i];
                }

                int row = 2, col = 1;//行 , 列
                foreach (var item in source)
                {
                    var rowdata = action(item);
                    for (int i = 0; i < rowdata.Length; i++)
                    {
                        worksheet.Cells[row, col].Value = rowdata[i];
                        col += 1;
                    }
                    col = 1;
                    row += 1;
                }
                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                worksheet.Dispose();
                byte[] Export_Result = stream.ToArray();
                stream.Dispose();
                return Export_Result;
            }
        }

        /// <summary>
        /// 解析Excel为DataTable
        /// </summary>
        /// <param name="stream">Excel文件流</param>
        /// <param name="hasHead">第一行是否为列头,默认为True</param>
        /// <returns>解析后的DataTable数据</returns>
        public static DataTable ExcelAnalysis(Stream stream, bool hasHead = true)
        {
            DataTable dataTable = new DataTable();//实例化一个DataTable
            DataRow dataRow = null;
            try
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];//读取Sheet 1
                    int rowCount = worksheet.Dimension.Rows;//总行数
                    int colCount = worksheet.Dimension.Columns;//总列数

                    int row = 1;//行
                    //头部
                    if (hasHead)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataTable.Columns.Add(new DataColumn(worksheet.Cells[row, col].Value.ToString()));
                        }
                        row = 2;//标题时跳过第一行
                    }
                    else
                    {
                        //默认列头
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataTable.Columns.Add(new DataColumn($"Col{col}"));
                        }
                    }

                    //读取数据
                    for (; row <= rowCount; row++)
                    {
                        dataRow = dataTable.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            object value = worksheet.Cells[row, col].Value;
                            if (value != null)
                            {
                                dataRow[col - 1] = value.ToString();
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Dispose();
            }
        }
    }
}