using Oracle.ManagedDataAccess.Client;

public class LogRepository
{
    private readonly string _connectionString;

    public LogRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertLog(string operation, string message)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Log (Operation, Message) VALUES (:operation, :message)";

            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(new OracleParameter("operation", operation));
                command.Parameters.Add(new OracleParameter("message", message));

                command.ExecuteNonQuery();
            }
        }
    }
}
