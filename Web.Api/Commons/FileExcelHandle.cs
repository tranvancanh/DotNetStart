using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections;
using System.Data;
using WebApi.Models.Commons;

namespace WebApi.Commons
{
    public class FileExcelHandle
    {
        public static DataSet ReadFileExcelWithMutilesSheet(string fullPath, string[] sheetNames = null)
        {
            var dataSet = new DataSet();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);
                if (sheetNames is null)
                {
                    var dataTable = ReadFileExcelWithSingleSheet(fullPath);
                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                }
                for (int i = 0; i < sheetNames.Length; i++)
                {
                    var sheetName = sheetNames[i];
                    var dataTable = ReadFileExcelWithSingleSheet(fullPath, sheetName);
                    dataSet.Tables.Add(dataTable);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;

        }

        public static async Task<DataSet> ReadFileExcelWithMutilesSheetAsync(string fullPath, string[] sheetNames = null)
        {
            var dataSet = new DataSet();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);
                if (sheetNames is null)
                {
                    var dataTable = await ReadFileExcelWithSingleSheetAsync(fullPath);
                    dataSet.Tables.Add(dataTable);
                    return dataSet;
                }
                for (int i = 0; i < sheetNames.Length; i++)
                {
                    var sheetName = sheetNames[i];
                    var dataTable = await ReadFileExcelWithSingleSheetAsync(fullPath, sheetName);
                    dataSet.Tables.Add(dataTable);
                }

            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;

        }

        public static DataTable ReadFileExcelWithSingleSheetAndHeader(string fullPath, string[] headerTextContains, string sheetName = null, bool setIndex = false)
        {
            var dataTable = new DataTable();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);

                var originalData = ReadFileExcelWithSingleSheet(fullPath, sheetName);
                var data = DataTableFilter(originalData, headerTextContains, setIndex);
                if (data.IsConvert)
                {
                    dataTable = data.NewDataTable;
                }
                else
                {
                    dataTable = new DataTable();
                }

            }
            catch (Exception) 
            {
                throw;
            }
            return dataTable;
        }

        public static async Task<DataTable> ReadFileExcelWithSingleSheetAndHeaderAsync(string fullPath, string[] headerTextContains, string sheetName = null, bool setIndex = false)
        {
            var dataTable = new DataTable();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);

                var originalData = await ReadFileExcelWithSingleSheetAsync(fullPath, sheetName);
                var data = DataTableFilter(originalData, headerTextContains, setIndex);
                if (data.IsConvert)
                {
                    dataTable = data.NewDataTable;
                }
                else
                {
                    dataTable = new DataTable();
                }

            }
            catch (Exception)
            {
                throw;
            }
            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ReadFileExcelWithSingleSheet(string fullPath, string sheetName = null)
        {
            var dataTable = new DataTable();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // ファイル情報を読み込む
                var inputExcelFile = new FileInfo(fullPath);
                using (var inputFile = new ExcelPackage(inputExcelFile))
                {
                    ExcelWorksheet worksheet;
                    // シート名が「inputSheet」
                    if (sheetName is null)
                    {
                        worksheet = inputFile.Workbook.Worksheets[0];
                    }
                    else
                    {
                        worksheet = inputFile.Workbook.Worksheets.Where(s => s.Name == sheetName).FirstOrDefault();
                        if (worksheet is null)
                        {
                            throw new Exception(sheetName + "のシート名が存在しません!");
                        }
                    }
                    dataTable.TableName = worksheet.Name;
                    // get number of rows and columns in the sheet
                    int rows = worksheet.Dimension.Rows; // 20
                    int columns = worksheet.Dimension.Columns; // 7

                    //クラム作成
                    for (int column = 0; column < columns; column++)
                    {
                        var columnName = GetExcelColumnName(column + 1);
                        dataTable.Columns.Add(columnName, typeof(string));
                    }

                    // 以下に、シート内のセルの読み込み処理を記述する
                    for (int i = 0; i < rows; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        for (int j = 0; j < columns; j++)
                        {
                            var cellVal = worksheet.Cells[i + 1, j + 1].Value;
                            row[j] = cellVal;
                        }
                        dataTable.Rows.Add(row);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dataTable;
        }

        public static async Task<DataTable> ReadFileExcelWithSingleSheetAsync(string fullPath, string sheetName = null)
        {
            var dataTable = new DataTable();
            try
            {
                //パスをチェック
                CheckFullPathExcelXLSX(fullPath);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                // ファイル情報を読み込む
                var inputExcelFile = new FileInfo(fullPath);
                using (var inputFile = new ExcelPackage(inputExcelFile))
                {
                    ExcelWorksheet worksheet;
                    // シート名が「inputSheet」
                    if (sheetName is null)
                    {
                        worksheet = inputFile.Workbook.Worksheets[0];
                    }
                    else
                    {
                        worksheet = inputFile.Workbook.Worksheets.Where(s => s.Name == sheetName).FirstOrDefault();
                        if (worksheet is null)
                        {
                            throw new Exception(sheetName + "のシート名が存在しません!");
                        }
                    }
                    dataTable.TableName = worksheet.Name;
                    // get number of rows and columns in the sheet
                    int rows = worksheet.Dimension.Rows; // 20
                    int columns = worksheet.Dimension.Columns; // 7

                    await Task.Run(() =>
                    {
                        //クラム作成
                        for (int column = 0; column < columns; column++)
                        {
                            var columnName = GetExcelColumnName(column + 1);
                            dataTable.Columns.Add(columnName, typeof(string));
                        }

                        // 以下に、シート内のセルの読み込み処理を記述する
                        for (int i = 0; i < rows; i++)
                        {
                            DataRow row = dataTable.NewRow();
                            for (int j = 0; j < columns; j++)
                            {
                                var cellVal = worksheet.Cells[i + 1, j + 1].Value;
                                row[j] = cellVal;
                            }
                            dataTable.Rows.Add(row);
                        }
                    });
                    
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dataTable;
        }


        /// <summary>
        /// 列名でデータをフィルタリングする
        /// </summary>
        /// <param name="originalTable"></param>
        /// <param name="headerTextContains"></param>
        /// <param name="setIndex"></param>
        /// <returns></returns>
        public static DataTableFilter DataTableFilter(DataTable originalTable, string[] headerTextContains, bool setIndex = false)
        {
            var convert = new DataTableFilter();
            try
            {
                DataRow dataRow = null;
                var isStartReadBody = false;
                DataTable newDataTable = null;
                var indexStart = 0;
                for (int i = 0; i < originalTable.Rows.Count; i++)
                {
                    dataRow = originalTable.Rows[i];
                    var dataRowItemArray = dataRow.ItemArray;
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(dataRowItemArray);
                    var arrayList = (ArrayList)JsonConvert.DeserializeObject(jsonString, typeof(ArrayList));
                    if (!isStartReadBody)
                    {
                        var total = 0;
                        foreach (var headerKey in headerTextContains)
                        {
                            var check = (from string array in arrayList
                                         where (array is not null) && array.Contains(headerKey)
                                         select array).ToList().Take(1).ToList();
                            total += check.Count;
                        }
                        if (total >= headerTextContains.Length)
                        {
                            newDataTable = new DataTable();
                            newDataTable.Columns.Clear();
                            DataColumn dtColumn;
                            var headers = (from string array in arrayList
                                           where string.IsNullOrWhiteSpace(array) == false
                                           select array).ToList();
                            indexStart = arrayList.IndexOf(headers[0]);
                            foreach (string columnName in headers)
                            {
                                dtColumn = new DataColumn();
                                dtColumn.ColumnName = columnName;
                                newDataTable.Columns.Add(dtColumn);
                            }
                            isStartReadBody = true;
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    //データを取得
                    if (isStartReadBody)
                    {
                        DataRow drow = newDataTable.NewRow();
                        for (int col = 0; col < newDataTable.Columns.Count; col++)
                        {
                            drow[col] = dataRowItemArray[col + indexStart];
                        }
                        newDataTable.Rows.Add(drow);
                    }
                }

                if (newDataTable is not null)
                {
                    if (newDataTable.Rows.Count > 0)
                    {
                        //空のデータを取得する
                        var blankDataRow = new List<DataRow>();
                        foreach (DataRow row in newDataTable.Rows)
                        {
                            var isCheck = row.ItemArray.All(x => string.IsNullOrWhiteSpace(Convert.ToString(x)));
                            if (isCheck)
                            {
                                blankDataRow.Add(row);
                            }
                        }
                        //空のデータを削除する
                        foreach (var row in blankDataRow.ToList())
                        {
                            row.Delete();
                        }
                        //Indexを追加
                        if (setIndex)
                        {
                            DataColumn Col = newDataTable.Columns.Add("Index");
                            Col.SetOrdinal(0);// to put the column in position 0;
                            for (int index = 0; index < newDataTable.Rows.Count; index++)
                            {
                                newDataTable.Rows[index]["Index"] = index + 1;
                            }
                        }
                        convert = new DataTableFilter() { IsConvert = true, NewDataTable = newDataTable };
                    }
                    else
                    {
                        convert = new DataTableFilter() { IsConvert = true, NewDataTable = newDataTable };
                    }
                }
                else
                {
                    convert = new DataTableFilter() { IsConvert = false, NewDataTable = null };
                }
            }
            catch (Exception)
            {
                throw;
            }

            return convert;
        }

        /// <summary>
        /// ファイル名をチェック
        /// </summary>
        /// <param name="fullPath"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="System.Exception"></exception>
        private static void CheckFullPathExcelXLSX(string fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new Exception(Constants.FilePathAnonymous);
            }
            var folderPath = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                throw new Exception(Constants.FilePathAnonymous);
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new Exception(Constants.FileNameAnonymous);
            }
            if (!File.Exists(fullPath))
            {
                throw new Exception(Constants.FileNameAnonymous);
            }
            var extension = Path.GetExtension(fullPath).ToLower();
            if (!extension.Equals(".xlsx"))
            {
                throw new System.Exception(Constants.FileExtensionsNotSupport);
            }
            return;
        }


        private static string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static (bool isExport, string filePath) ExportFileExcel(System.Data.DataTable data, string[] headers, string fileFullPath, string sheetName = "Sheet1", int startX = 0, int startY = 0)
        {
            var result = false;
            if (fileFullPath == null) { throw new System.Exception("ファイル名がおかしいです！"); }
            if (startX < 0) { throw new System.Exception("開始位置がおかしいです！"); }
            if (startY < 0) { throw new System.Exception("開始位置がおかしいです！"); }
            if (headers.Length != data.Columns.Count) { return (isExport: result, filePath: null); }
            string extension = Path.GetExtension(fileFullPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fileFullPath = fileFullPath + ".xlsx";
            }

            extension = Path.GetExtension(fileFullPath).ToLower();
            if (!extension.Equals(".xlsx"))
            {
                throw new System.Exception("拡張子が正しいくない！");
            }

            FileInfo fileInfoCheck = new FileInfo(fileFullPath);
            // 既にファイルが存在している場合は削除する
            if (fileInfoCheck.Exists)
            {
                // ファイルを削除
                fileInfoCheck.Delete();
                Thread.Sleep(1000);
            }
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(fileFullPath);
                using (var package = new ExcelPackage(fileInfo))
                {
                    // シート追加
                    package.Workbook.Worksheets.Add(sheetName);
                    // シート取得
                    using (ExcelWorksheet sheet = package.Workbook.Worksheets[sheetName])
                    {
                        //ヘッダーセット
                        for (int i = 0; i < headers.Length; i++)
                        {
                            sheet.Cells[startY + 1, startX + 1 + i].Value = headers[i];
                        }

                        var printHeader = false;
                        // データセット
                        sheet.Cells[startY + 1 + 1, startX + 1].LoadFromDataTable(data, printHeader);

                        // 保管
                        package.SaveAs(fileInfo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            result = true;

            return (isExport: result, filePath: fileFullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="headers"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="sheetName"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static async Task<(bool isExport, string filePath)> ExportFileExcelAsync(System.Data.DataTable data, string[] headers, string fileFullPath, string sheetName = "Sheet1", int startX = 0, int startY = 0)
        {
            var result = false;
            if (fileFullPath == null) { throw new System.Exception("ファイル名がおかしいです！"); }
            if (startX < 0) { throw new System.Exception("開始位置がおかしいです！"); }
            if (startY < 0) { throw new System.Exception("開始位置がおかしいです！"); }
            if (headers.Length != data.Columns.Count) { return (isExport: result, filePath: null); }
            string extension = Path.GetExtension(fileFullPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                fileFullPath = fileFullPath + ".xlsx";
            }

            extension = Path.GetExtension(fileFullPath).ToLower();
            if (!extension.Equals(".xlsx"))
            {
                throw new System.Exception("拡張子が正しいくない！");
            }

            // 既にファイルが存在している場合は削除する
            FileInfo fileInfoCheck = new FileInfo(fileFullPath);
            // 既にファイルが存在している場合は削除する
            if (fileInfoCheck.Exists)
            {
                // ファイルを削除
                fileInfoCheck.Delete();
                await Task.Delay(1000);
            }
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(fileFullPath);
                using (var package = new ExcelPackage(fileInfo))
                {
                    // シート追加
                    package.Workbook.Worksheets.Add(sheetName);
                    // シート取得
                    using (ExcelWorksheet sheet = package.Workbook.Worksheets[sheetName])
                    {
                        //ヘッダーセット
                        for (int i = 0; i < headers.Length; i++)
                        {
                            sheet.Cells[startY + 1, startX + 1 + i].Value = headers[i];
                        }

                        var printHeader = false;
                        // データセット
                        sheet.Cells[startY + 1 + 1, startX + 1].LoadFromDataTable(data, printHeader);

                        // 保管
                        await package.SaveAsAsync(fileInfo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            result = true;

            return (isExport: result, filePath: fileFullPath);
        }
    }
}
