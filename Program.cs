

using System.Data;
using System.Data.SQLite;

namespace Storage;

internal class Program
{
    private static string ConnectionString = "Data Source=StorageData.db";

    static public async Task AddStorageAsync(string ConnectionString, string ProductName, int ProductType, int ProviderId, int Count, decimal Cost, string Date)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "INSERT INTO \"main\".\"Storage\"(\"ProductName\",\"ProductType\",\"ProviderId\",\"Count\",\"Cost\",\"SupplyDate\") VALUES ($ProductName,$ProductType,$ProviderId,$Count,$Cost,$Date);\r\n";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ProductName", ProductName);
        command.Parameters.AddWithValue("$ProductType", ProductType);
        command.Parameters.AddWithValue("$ProviderId", ProviderId);
        command.Parameters.AddWithValue("$Count", Count);
        command.Parameters.AddWithValue("$Cost", Cost);
        command.Parameters.AddWithValue("$Date", Date);
        await command.ExecuteNonQueryAsync();
    }

    static public async Task AddProductTypeAsync(string ConnectionString, string TypeName)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = $"INSERT INTO \"main\".\"ProductTypes\"(\"TypeName\") VALUES ($TypeName);";

        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$TypeName", TypeName);
        await command.ExecuteNonQueryAsync();
    }

    static public async Task AddProviderAsync(string ConnectionString, string ProviderName)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = $"INSERT INTO \"main\".\"Providers\"(ProviderName) VALUES ($ProviderName);";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ProviderName", ProviderName);
        await command.ExecuteNonQueryAsync();
    }

    static public async Task UpdateStorageAsync(string ConnectionString, int ID, string ProductName, int ProductType, int ProviderId, int Count, decimal Cost, string Date)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "UPDATE Storage\r\nSET ProductName = $ProductName,\r\n    ProductType = $ProductType,\r\n    ProviderId = $ProviderId,\r\n    Count = $Count,\r\n    Cost = $Cost,\r\n    SupplyDate = $Date\r\nWHERE\r\n    ProductId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ProductName", ProductName);
        command.Parameters.AddWithValue("$ProductType", ProductType);
        command.Parameters.AddWithValue("$ProviderId", ProviderId);
        command.Parameters.AddWithValue("$Count", Count);
        command.Parameters.AddWithValue("$Cost", Cost);
        command.Parameters.AddWithValue("$Date", Date);
        command.Parameters.AddWithValue("$ID", ID);
        await command.ExecuteNonQueryAsync();
    }
    static public async Task UpdateProviderAsync(string ConnectionString, int ID, string ProviderName)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "UPDATE Providers\r\nSET ProviderName = $ProviderName\r\nWHERE\r\n    ProviderId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ProviderName", ProviderName);
        command.Parameters.AddWithValue("$ID", ID);
        await command.ExecuteNonQueryAsync();
    }
    static public async Task UpdateProductTypesAsync(string ConnectionString, int ID, string TypeName)
    {
        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "UPDATE ProductTypes SET TypeName = $typ WHERE TypeId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$typ", TypeName);
        command.Parameters.AddWithValue("$ID", ID);
        await command.ExecuteNonQueryAsync();
    }
    static public async Task DeleteStorageAsync(string ConnectionString, int ID)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "DELETE FROM \"main\".\"Storage\" WHERE ProductId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ID", ID);

        await command.ExecuteNonQueryAsync();
    }
    static public async Task DeleteProviderAsync(string ConnectionString, int ID)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "DELETE FROM \"main\".\"Providers\" WHERE ProductId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ID", ID);

        await command.ExecuteNonQueryAsync();
    }
    static public async Task DeleteProductTypesAsync(string ConnectionString, int ID)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "DELETE FROM \"main\".\"ProductTypes\" WHERE ProductId = $ID;";
        using var command = new SQLiteCommand(code, connection);
        command.Parameters.AddWithValue("$ID", ID);

        await command.ExecuteNonQueryAsync();
    }
    static public async Task SelectProviderMostProductAsync(string ConnectionString)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "select Storage.ProviderId,ProviderName,sum(Count) as num from Storage\r\njoin main.Providers P on P.ProviderId = Storage.ProviderId\r\ngroup by Storage.ProviderId\r\norder by sum(Count) desc\r\nLIMIT 1";
        using var command = new SQLiteCommand(code, connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                /*Console.WriteLine("{0} | {1} | {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));*/
                Console.WriteLine($"{reader["ProviderId"]} | {reader["ProviderName"]} | {reader["num"]}");
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        reader.Close();
    }
    static public async Task SelectProviderLessProductAsync(string ConnectionString)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "select Storage.ProviderId,ProviderName,sum(Count) as num from Storage\r\njoin main.Providers P on P.ProviderId = Storage.ProviderId\r\ngroup by Storage.ProviderId\r\norder by sum(Count) asc\r\nLIMIT 1";
        using var command = new SQLiteCommand(code, connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                /*Console.WriteLine("{0} | {1} | {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));*/
                Console.WriteLine($"{reader["ProviderId"]} | {reader["ProviderName"]} | {reader["num"]}");
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        reader.Close();
    }

    static public async Task SelectProductTypeMostProductAsync(string ConnectionString)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "select Storage.ProductType,TypeName,sum(Count) as num from Storage\r\njoin main.ProductTypes P on P.TypeId = Storage.ProductType\r\ngroup by Storage.ProductType\r\norder by sum(Count) desc\r\nlimit 1";
        using var command = new SQLiteCommand(code, connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                /*Console.WriteLine("{0} | {1} | {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));*/
                Console.WriteLine($"{reader["ProductType"]} | {reader["TypeName"]} | {reader["num"]}");
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        reader.Close();
    }
    static public async Task SelectProductTypeLessProductAsync(string ConnectionString)
    {

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "select Storage.ProductType,TypeName,sum(Count) as num from Storage\r\njoin main.ProductTypes P on P.TypeId = Storage.ProductType\r\ngroup by Storage.ProductType\r\norder by sum(Count) asc\r\nlimit 1";
        using var command = new SQLiteCommand(code, connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                /*Console.WriteLine("{0} | {1} | {2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2));*/
                Console.WriteLine($"{reader["ProductType"]} | {reader["TypeName"]} | {reader["num"]}");
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        reader.Close();
    }


    static public async Task SelectProductSetDaysAgoArrivedAsync(string ConnectionString, int days)
    {

        Dictionary<int, DateTime> dict = new Dictionary<int, DateTime> { };

        using var connection = new SQLiteConnection(ConnectionString);
        await connection.OpenAsync();
        string code = "select ProductId,Storage.SupplyDate from  Storage;";
        using var command = new SQLiteCommand(code, connection);
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                dict.Add(Convert.ToInt32(reader["ProductId"]), Convert.ToDateTime(reader["SupplyDate"]));
            }
        }
        else
        {
            Console.WriteLine("No rows found.");
        }
        reader.Close();
        foreach (var i in dict)
        {
            var buf = i.Value.AddDays(days);
            if (buf.Date == DateTime.Today)
            {
                Console.WriteLine($"{i.Key} | {i.Value}");
            }
        }

    }

    static async Task Main(string[] args)
    {
        using var connection = new SQLiteConnection(ConnectionString);

        try
        {

            await connection.OpenAsync();
            Console.WriteLine("Сonnection is successful\n");
            //
            //кароче я зробив всю дз і після цього побачив в коментарі, що вона має бути на від'єднаному режимі
            //а переробляти трохи впадло хоча впринципі я за нього шарю 
            string sql = "Select * From Storage";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            using (var writer = new StringWriter())
            {
                ds.WriteXml(writer);
                Console.WriteLine(writer.ToString());
            }
            //
        }
        catch (Exception ex)
        {
            await connection.CloseAsync();
            Console.WriteLine("\nConnection is failed");
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                await connection.CloseAsync();
                Console.WriteLine("\nConnection is disconnected");
            }
        }
    }
}