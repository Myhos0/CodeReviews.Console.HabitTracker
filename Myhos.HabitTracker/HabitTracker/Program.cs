using HabitTrackerProgram;

class Program 
{
    public static void Main() 
    {
        HabitTrackerProgram.DataBase db = new();

        db.CreateTables();
        db.CreateRecords();

        HabitTrackerProgram.HabitTracker hb = new();

        hb.Menu();
    }
}