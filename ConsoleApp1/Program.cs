using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ConsoleApp1
{
    internal class Program
    {
        public static void Main()
        {
            DbProviderFactory factory = NpgsqlFactory.Instance;
            DbConnection connection = factory.CreateConnection()!;

            connection.ConnectionString = ""; ;
            connection.Open();

            string createTableSql =
                "CREATE TABLE IF NOT EXISTS test01 (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "last_name varchar NOT NULL, " +
                "first_name varchar NOT NULL);";

            using DbCommand commandCreate = factory.CreateCommand()!;

            commandCreate.Connection = connection;
            commandCreate.CommandText = createTableSql;
            commandCreate.ExecuteNonQuery();

            DbCommand commandInsert = factory.CreateCommand()!;

            commandInsert.Connection = connection;
            commandInsert.CommandText = "INSERT INTO test01(last_name, first_name) VALUES(@last_name, @first_name);";

            AddParameter(commandInsert, "@last_name", "Фамилия");
            AddParameter(commandInsert, "@first_name", "Имя");

            commandInsert.ExecuteNonQuery();

            DbCommand command = factory.CreateCommand()!;
            command.Connection = connection;
            command.CommandText = "SELECT id, last_name, first_name FROM test01";

            using DbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(
                    $"{reader.GetInt32(0),3} " +
                    $"{reader.GetString(1),-15} " +
                    $"{reader.GetString(2)}");
            }
        }

        static void AddParameter(DbCommand command, string parameterName, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            command.Parameters.Add(parameter);
        }
    }
}