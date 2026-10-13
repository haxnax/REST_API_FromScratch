using System.Data;

public class ConnectionPool
{
    private readonly Queue<IDbConnection> _pool = new Queue<IDbConnection>();
    private readonly int _maxSize;
    private readonly string _connectionString;
    private readonly object _lock= new object();

    public ConnectionPool(string connectonString, int maxSize=10)
    {
        _connectionString= connectonString;
        _maxSize=maxSize;
    }

    public IDbConnection GetConnection()
    {
        lock(_lock)
        {
            if(_pool.Count>0)
            {
                return _pool.Dequeue();
            }
            return CreateConnection();
        }
    }
    public void ReturnConnection(IDbConnection connection)
    {
        lock(_lock)
        {
            if(_pool.Count<_maxSize && connection.State==ConnectionState.Open)
            {
                _pool.Enqueue(connection);
            }
            else
            {
                connection.Dispose();
            }
        }
    }

    private IDbConnection CreateConnection()
    {
        throw new NotImplementedException("Implement based on your database provider");
    }
}