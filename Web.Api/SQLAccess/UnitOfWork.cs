using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Tozan.Server.UnitOfWork
{

    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        void Begin();
        void Commit();
        void Rollback();
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DbConnection _connection;
        private DbTransaction _transaction;
        public UnitOfWork(DbConnection connection)
        {
            _connection = connection;
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }
        public IDbConnection Connection => _connection;

        public IDbTransaction Transaction => _transaction;

        public IDbTransaction GetTransaction()
        {
            return this._transaction;
        }

        public void Begin()
        {
            if(_connection is not null)
            {
                _transaction = _connection.BeginTransaction();
            }
            return;
        }

        public async Task BeginAsync()
        {
            if (_connection is not null)
            {
                _transaction = await _connection.BeginTransactionAsync();
            }
            return;
        }

        public void Commit()
        {
            try
            {
                if (_transaction is not null)
                {
                    _transaction.Commit();
                }
            }
            catch(Exception ex)
            {
                if (_transaction is not null)
                {
                    _transaction.Rollback();
                }
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return;
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_transaction is not null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch(Exception ex)
            {
                if (_transaction is not null)
                {
                    await _transaction.RollbackAsync();
                }
                this.ThrowExceptionMessage(ex);
                throw;
            }
            return;
        }

        public void Rollback()
        {
            if (_transaction is not null)
            {
                _transaction.Rollback();
            }
            return;
        }

        public async Task RollbackAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }
            return;
        }

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
                if (disposing)
                {
                    if (_transaction is not null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                }
                disposed = true;
            }
            return;
        }

        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            return;
        }
    }
}
