using System;
using System.Threading.Tasks;

namespace System.Data.SqlClient
{
    public static class SqlCommandAsyncExtensions
    {
        public static Task<int> ExecuteNonQueryAsync(this SqlCommand sqlCommand)
        {
            var source = new TaskCompletionSource<int>();
            try
            {
                Task<int>.Factory.FromAsync((callback, stateObject) => sqlCommand.BeginExecuteNonQuery(callback, stateObject), 
                    asyncResult => sqlCommand.EndExecuteNonQuery(asyncResult), null)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Exception e = t.Exception.InnerException;
                            source.SetException(e);
                        }
                        else
                        {
                            if (t.IsCanceled)
                            {
                                source.SetCanceled();
                            }
                            else
                            {
                                source.SetResult(t.Result);
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                source.SetException(ex);
            }
            return source.Task;
        }

        public static Task<SqlDataReader> ExecuteReaderAsync(this SqlCommand sqlCommand)
        {
            var source = new TaskCompletionSource<SqlDataReader>();
            try
            {
                Task<SqlDataReader>.Factory.FromAsync((callback, stateObject) => sqlCommand.BeginExecuteReader(callback, stateObject),
                    asyncResult => sqlCommand.EndExecuteReader(asyncResult), null)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Exception e = t.Exception.InnerException;
                            source.SetException(e);
                        }
                        else
                        {
                            if (t.IsCanceled)
                            {
                                source.SetCanceled();
                            }
                            else
                            {
                                source.SetResult(t.Result);
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                source.SetException(ex);
            }
            return source.Task;
        }
    }
}
