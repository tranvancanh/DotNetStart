
using Dapper;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Tozan.Server.ConnectionString;
using static Dapper.SqlMapper;

namespace Tozan.Server.SQLAccess
{
    // 参考： https://riptutorial.com/dapper/topic/3/basic-querying
    // https://pg-life.net/csharp/dapper/
    // https://j-levia.hatenablog.jp/entry/2017/04/17/213921


    public class SQLAccess : IDisposable
    {
        private static readonly string ConnectionString = GetConnectString.GetInstance().ConnectionString;

        private DbConnection _connection;
        public IDbConnection Connection => _connection;


        public SQLAccess()
        {
            _connection = new SqlConnection(new string(ConnectionString));
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public SQLAccess(string connectionString)
        {
            _connection = new SqlConnection(new string(connectionString));
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        //**************************************************************************************/
        //機能 　  : 現在のConnectionを取得
        //返り値   : 現在のConnection
        //引き数　 : なし
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public SqlConnection GetConnection()
        {
            return (SqlConnection)this._connection;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Insert文)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public long InsertT<T>(string T_SQL, T entityToInsert, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = _connection.Execute(T_SQL, entityToInsert, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Insert文, 非同期)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<long> InsertTAsync<T>(string T_SQL, T entityToInsert, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = await _connection.ExecuteAsync(T_SQL, entityToInsert, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(UpdateT文)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public long UpdateT<T>(string T_SQL, T entityToUpdate, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = _connection.Execute(T_SQL, entityToUpdate, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Update文)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<long> UpdateTAsync<T>(string T_SQL, T entityToUpdate, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = await _connection.ExecuteAsync(T_SQL, entityToUpdate, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Delete文)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public long DeleteT<T>(string T_SQL, T entityToDelete, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = _connection.Execute(T_SQL, entityToDelete, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Delete文、非同期)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<long> DeleteTAsync<T>(string T_SQL, T entityToDelete, IDbTransaction transaction = null) where T : class
        {
            long affectedRows = 0;
            try
            {
                affectedRows = await _connection.ExecuteAsync(T_SQL, entityToDelete, transaction);
            }
            catch (Exception ex)
            {
                affectedRows = -99;
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return affectedRows;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文)
        //返り値   : DataTable
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public DataTable ExecQueryDataTable(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            DataTable table = new DataTable();
            try
            {
                var reader = _connection.ExecuteReader(T_SQL, param: parametter, transaction, commandType: CommandType.Text);
                table.Load(reader);
            }
            catch(Exception ex)
            {
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return table;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文), 列をNULL にすることを許可する.
        //返り値   : DataTable
        //引き数　 : T_SQL, Param
        //作成日 　: 2023年01月
        //作成者   : システム
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public DataTable ExecQueryDataTableAllowColumnNull(string T_SQL, object parametter = null)
        {
            DataTable table = new DataTable();
            try
            {
                var reader = _connection.ExecuteReader(T_SQL, param: parametter, commandType: CommandType.Text);
                table.Load(reader);
                if (table is not null)
                {
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        var column = table.Columns[i];
                        column.AllowDBNull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return table;
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文非同期)
        //返り値   : DataTable
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<DataTable> ExecQueryDataTableAsync(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            DataTable table = new DataTable();
            try
            {
                var reader = await _connection.ExecuteReaderAsync(T_SQL, param: parametter, transaction, commandType: CommandType.Text);
                table.Load(reader);
            }
            catch(Exception ex)
            {
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return table;
        }

        //////////////////////////////////////////////////------------------ Procedure ---------- //////////////////////////////////////////////////////////////
        public DataTable ExecProcedureDataTable(string ProcedureName, object parametter = null)
        {
            DataTable table = new DataTable();
            try
            {
                var reader = _connection.ExecuteReader(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                table.Load(reader);
            }
            catch (Exception ex)
            {
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return table;
        }

        public async Task<DataTable> ExecProcedureDataTableAsync(string ProcedureName, object parametter = null)
        {
            DataTable table = new DataTable();
            try
            {
                var reader = await _connection.ExecuteReaderAsync(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
                table.Load(reader);
            }
            catch (Exception ex)
            {
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return table;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文)
        //返り値   : IEnumerable<T> object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public IEnumerable<T> ExecQueryData<T>(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return _connection.Query<T>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文非同期)
        //返り値   : T object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public T ExecQueryDataFistOrDefault<T>(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return _connection.QueryFirstOrDefault<T>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文)
        //返り値   : <IEnumerable<T> object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<IEnumerable<T>> ExecQueryDataAsync<T>(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return await _connection.QueryAsync<T>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文非同期)
        //返り値   : <IEnumerable<T> object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<T> ExecQueryDataFirstOrDefaultAsync<T>(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }
        //**************************************************************************************/
        //機能 　  : Executing Multiple SQL Statements(Select文同期)
        //返り値   : List<DataTable>
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年12月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public List<DataTable> ExecQueryMultipleDataTable(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            var listDataTable = new List<DataTable>();
            try
            {
                var gridReader = _connection.QueryMultiple(T_SQL, parametter, transaction, commandType: CommandType.Text);
                while (!gridReader.IsConsumed)
                {
                    var obj = gridReader.Read();
                    var objString = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    var data = JsonConvert.DeserializeObject<DataTable>(objString);
                    listDataTable.Add(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listDataTable;
        }

        //**************************************************************************************/
        //機能 　  : Executing Multiple SQL Statements(Select文非同期)
        //返り値   : Task<List<DataTable>>
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年12月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<List<DataTable>> ExecQueryMultipleDataTableAsync(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            var listDataTable = new List<DataTable>();
            try
            {
                var gridReader = await _connection.QueryMultipleAsync(T_SQL, parametter, transaction, commandType: CommandType.Text);
                while (!gridReader.IsConsumed)
                {
                    var obj = await gridReader.ReadAsync();
                    var objString = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    var data = JsonConvert.DeserializeObject<DataTable>(objString);
                    listDataTable.Add(data);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listDataTable;
        }

        //**************************************************************************************/
        //機能 　  : Executing Multiple SQL Statements(Select文同期)
        //返り値   : Task<GridReader>
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年12月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public GridReader ExecQueryMultipleQueriesData(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return _connection.QueryMultiple(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : Executing Multiple SQL Statements(Select文非同期)
        //返り値   : Task<GridReader>
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年12月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<GridReader> ExecQueryMultipleQueriesDataAsync(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return await _connection.QueryMultipleAsync(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(InsertまたはDeleteまたはUpdate文)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public int ExecQueryNonData(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return _connection.Execute(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(InsertまたはDeleteまたはUpdate文非同期)
        //返り値   : 影響を受ける行
        //引き数　 : T_SQL, Param, トランザクション
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<int> ExecQueryNonDataAsync(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteAsync(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }


        //////////////////////////////////////////////////////////////// Procedure Start //////////////////////////////////////////////////////////////////////
        public IEnumerable<T> ExecProcedureData<T>(string ProcedureName, object parametter = null)
        {
            return _connection.Query<T>(ProcedureName, param: parametter, commandType: CommandType.StoredProcedure);
        }

        public T ExecProcedureFistOrDefault<T>(string ProcedureName, object parametter = null)
        {
            return _connection.QueryFirstOrDefault<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> ExecProcedureAsync<T>(string ProcedureName, object parametter = null)
        {
            return await _connection.QueryAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }

        public async Task<T> ExecProcedureDataFirstOrDefaultAsync<T>(string ProcedureName, object parametter = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }

        public int ExecProcedureNonData(string ProcedureName, object parametter = null, IDbTransaction transaction = null)
        {
            //return affectedRows 
            return _connection.Execute(ProcedureName, parametter, transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExecProcedureNonDataAsync(string ProcedureName, object parametter = null)
        {
            //return affectedRows 
            return await _connection.ExecuteAsync(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }
        //////////////////////////////////////////////////////////////// Procedure Stop //////////////////////////////////////////////////////////////////////


        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文)
        //返り値   : object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public object ExecQuerySacalar(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return _connection.ExecuteScalar<object>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }

        //**************************************************************************************/
        //機能 　  : SQLの実行(Select文非同期)
        //返り値   : object
        //引き数　 : T_SQL, Param
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public async Task<object> ExecQuerySacalarAsync(string T_SQL, object parametter = null, IDbTransaction transaction = null)
        {
            return await _connection.ExecuteScalarAsync<object>(T_SQL, parametter, transaction, commandType: CommandType.Text);
        }


        //////////////////////////////////////////////////////////////// Procedure Start //////////////////////////////////////////////////////////////////////
        public object ExecProcedureSacalar(string ProcedureName, object parametter = null)
        {
            return _connection.ExecuteScalar<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }

        public async Task<object> ExecProcedureSacalarAsync(string ProcedureName, object parametter = null)
        {
            return await _connection.ExecuteScalarAsync<object>(ProcedureName, parametter, commandType: CommandType.StoredProcedure);
        }

        //////////////////////////////////////////////////////////////// Procedure Stop ///////////////////////////////////////////////////////////////////////

        /// <summary>
        /// log
        /// </summary>
        /// <param name="ex"></param>
        private void ThrowExceptionMessage(Exception ex)
        {
#if DEBUG
            Debug.WriteLine(ex.Message);
#endif
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            // Execute if resources have not already been disposed.
            if (!disposed)
            {
                // If the call is from Dispose, free managed resources.
                if (disposing && (_connection is not null))
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
                disposed = true;
            }
            return;
        }

        ~SQLAccess()
        {
            Dispose(disposing: false);
        }

        //**************************************************************************************/
        //機能 　  : Dispose文
        //返り値   : なし
        //引き数　 : なし
        //作成日 　: 2022年08月
        //作成者   : システム部
        //機能説明 : 
        //注意事項 : 
        //
        //**************************************************************************************/
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
