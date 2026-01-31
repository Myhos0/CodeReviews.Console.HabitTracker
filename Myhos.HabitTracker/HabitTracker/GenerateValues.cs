namespace HabitTracker;

internal class GenerateValues
{
    private Random Random = new();

    public DateTime GenerateDate()
    {
        int year = Random.Next(2023, 2026);

        int month = Random.Next(1, 13);

        int daysInMonth = DateTime.DaysInMonth(year, month);

        int day = Random.Next(1, daysInMonth + 1);

        return new DateTime(year, month, day);
    }

    public int GenerateValue()
    {
        return Random.Next(1, 9) * Random.Next(1, 19);
    }
}
