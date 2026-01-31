using HabitTracker;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace HabitTrackerProgram;

internal class DataBase
{
    private readonly GenerateValues generateValues = new();

    private readonly string connectionString = @"Data Source=HabitTracker.db";

    private SqliteConnection GetConnection() => new(connectionString);

    internal void CreateTables()
    {
        using var connection = GetConnection();

        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS Habit(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Type TEXT NOT NULL CHECK(Type IN ('I','D'))
                    )";

        tableCmd.ExecuteNonQuery();

        tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS HabitValue(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    ValueInt INTEGER,
                    ValueDec REAL,
                    Date TEXT NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habit(Id)
                    )";

        tableCmd.ExecuteNonQuery();
    }

    internal void InsertHabit(string habitName, string typeValue)
    {
        using var connection = GetConnection();

        connection.Open();

        using var commnad = new SqliteCommand("INSERT INTO Habit(Name, Type) VALUES(@Name, @Type)", connection);

        commnad.Parameters.AddWithValue("@Name", habitName);
        commnad.Parameters.AddWithValue("@Type", typeValue);

        commnad.ExecuteNonQuery();
    }

    internal string GetHabitName(int habitId)
    {
        using var connection = GetConnection();

        connection.Open();

        using var command = new SqliteCommand("SELECT Name FROM Habit WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", habitId);

        var result = command.ExecuteScalar();

        return result == null ? "Habit not found" : Convert.ToString(result);
    }

    internal List<Habits> GetHabits()
    {
        List<Habits> result = [];

        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("SELECT * FROM Habit", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(new Habits()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Type = reader.GetString(2)
            });
        }

        return result;
    }

    internal List<HabitsValues> GetHabitsValues(int habitId)
    {
        List<HabitsValues> result = [];

        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("select * from HabitValue WHERE HabitId=@Id", connection);
        command.Parameters.AddWithValue("@Id", habitId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var habit = new HabitsValues
            {
                Id = reader.GetInt32(0),
                HabitId = reader.GetInt32(1),
                Date = DateTime.ParseExact(reader.GetString(4), "dd-MM-yy", new CultureInfo("en-US"))
            };

            habit.ValueInt = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
            habit.ValueDec = reader.IsDBNull(3) ? (double?)null : reader.GetDouble(3);

            result.Add(habit);
        }

        return result;
    }

    internal void Insert(int habitId, int? quantityI, double? quantityD, string date)
    {
        using var connection = GetConnection();
        connection.Open();

        using SqliteCommand command = new("INSERT INTO HabitValue(HabitId, ValueInt, ValueDec, Date) VALUES(@HabitId, @QuantityI, @QuantityD, @Date)", connection);

        command.Parameters.AddWithValue("@HabitId", habitId);
        command.Parameters.AddWithValue("@QuantityI", quantityI.HasValue ? (object)quantityI.Value : DBNull.Value);
        command.Parameters.AddWithValue("@QuantityD", quantityD.HasValue ? (object)quantityD.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Date", date);

        command.ExecuteNonQuery();
    }

    internal bool ExistValue(int id)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand(
            "SELECT EXISTS(SELECT 1 FROM HabitValue WHERE id = @Id)",
            connection);

        command.Parameters.AddWithValue("@Id", id);

        return Convert.ToInt32(command.ExecuteScalar()) == 1;
    }

    internal bool HabitExist(int id)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("SELECT EXISTS(SELECT 1 FROM Habit WHERE id = @Id)", connection);

        command.Parameters.AddWithValue("@Id", id);

        return Convert.ToInt32(command.ExecuteScalar()) == 1;
    }

    internal void Update(int id, string date, int? quantityI, double? quantityD)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("UPDATE HabitValue SET date = @Date, ValueInt = @QuantityI, ValueDec = @QuantityD  WHERE id = @Id", connection);

        command.Parameters.AddWithValue("@Date", date);
        command.Parameters.AddWithValue("@QuantityI", quantityI.HasValue ? (object)quantityI.Value : DBNull.Value);
        command.Parameters.AddWithValue("@QuantityD", quantityD.HasValue ? (object)quantityD.Value : DBNull.Value);
        command.Parameters.AddWithValue("@Id", id);

        command.ExecuteNonQuery();
    }

    internal bool Delete(int id)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("DELETE FROM HabitValue WHERE id = @Id", connection);

        command.Parameters.AddWithValue("@Id", id);

        return command.ExecuteNonQuery() > 0;
    }

    internal string GetHabitType(int id)
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SqliteCommand("SELECT Type FROM Habit WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        var result = command.ExecuteScalar();

        return result == null ? string.Empty : Convert.ToString(result);
    }

    internal double GetTotalQuantity(int habitId, string type)
    {
        using var connection = GetConnection();
        connection.Open();

        string column = type.Trim().ToUpper() switch
        {
            "I" => "ValueInt",
            "D" => "ValueDec",
            _ => throw new InvalidOperationException($"Invalid habit type: '{type}'")
        };

        string sql = $"SELECT SUM({column}) FROM HabitValue WHERE HabitId = @Id";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", habitId);

        object result = command.ExecuteScalar();
        double total = result != DBNull.Value ? Convert.ToDouble(result) : 0;

        return total;
    }

    internal double AveragePerYear(int habitId, string type, string year)
    {
        using var connection = GetConnection();
        connection.Open();

        string column = type.Trim().ToUpper() switch
        {
            "I" => "ValueInt",
            "D" => "ValueDec",
            _ => throw new InvalidOperationException($"Invalid habit type: '{type}'")
        };

        string sql = $@"SELECT AVG({column})FROM HabitValue WHERE HabitId = @Id AND substr(Date,7,2) = @Year";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", habitId);
        command.Parameters.AddWithValue("@Year", year);

        object result = command.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToDouble(result) : 0;
    }

    internal double AveragePerMonth(int habitId, string type, string month, string year)
    {
        using var connection = GetConnection();
        connection.Open();

        string column = type.Trim().ToUpper() switch
        {
            "I" => "ValueInt",
            "D" => "ValueDec",
            _ => throw new InvalidOperationException($"Invalid habit type: '{type}'")
        };

        string sql = $@"SELECT AVG({column}) FROM HabitValue WHERE HabitId = @Id AND substr(Date,4,2) = @Month AND substr(Date,7,2) = @Year";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", habitId);
        command.Parameters.AddWithValue("@Month", month.PadLeft(2, '0'));
        command.Parameters.AddWithValue("@Year", year);

        object result = command.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToDouble(result) : 0;
    }

    internal bool YearExists(int habitId, string year)
    {
        using var connection = GetConnection();
        connection.Open();

        string sql = @"SELECT EXISTS(SELECT 1 FROM HabitValue WHERE HabitId = @Id AND substr(Date,7,2) = @Year)";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", habitId);
        command.Parameters.AddWithValue("@Year", year);

        return Convert.ToInt32(command.ExecuteScalar()) == 1;
    }

    internal bool MonthExists(int habitId, string month, string year)
    {
        using var connection = GetConnection();
        connection.Open();

        string sql = @"SELECT EXISTS(SELECT 1 FROM HabitValue WHERE HabitId = @Id AND substr(Date,4,2) = @Month AND substr(Date,7,2) = @Year)";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", habitId);
        command.Parameters.AddWithValue("@Month", month);
        command.Parameters.AddWithValue("@Year", year);

        return Convert.ToInt32(command.ExecuteScalar()) == 1;
    }

    internal void CreateRecords()
    {
        using var connection = GetConnection();

        connection.Open();

        using var checkCommand = new SqliteCommand("SELECT COUNT(*) FROM HabitValue", connection);

        long count = (long)checkCommand.ExecuteScalar();

        if (count == 0)
        {
            using var insertHabit = connection.CreateCommand();
            insertHabit.CommandText = "INSERT INTO Habit (Name,Type) VALUES ('Kilometer run','I')";
            insertHabit.ExecuteNonQuery();
        }

        if (count > 0)
        {
            return;
        }

        for (int i = 0; i < 100; i++)
        {
            using var command = new SqliteCommand(@"INSERT INTO HabitValue (HabitId, ValueInt, ValueDec, Date) VALUES (@HabitId, @ValueInt, NULL, @Date)", connection);

            command.Parameters.AddWithValue("@HabitId", 1);
            command.Parameters.AddWithValue("@ValueInt", generateValues.GenerateValue());
            command.Parameters.AddWithValue("@Date", generateValues.GenerateDate().ToString("dd-MM-yy"));

            command.ExecuteNonQuery();
        }
    }

}
