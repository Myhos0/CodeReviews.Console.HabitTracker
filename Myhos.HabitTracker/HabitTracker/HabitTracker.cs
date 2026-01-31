using HabitTrackerProgram.Enums;
using Spectre.Console;
using System.Globalization;

namespace HabitTrackerProgram;

internal class HabitTracker
{
    private readonly DataBase DataBase = new();

    private readonly Habits Habit = new();

    public void Menu()
    {
        bool open = true;
        while (open)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenu>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<MainMenu>())
                );

            switch (menuOption)
            {
                case MainMenu.ConsultHabits:
                    ViewHabits();
                    break;
                case MainMenu.CreateNewHabit:
                    CreateHabit();
                    break;
                case MainMenu.Exit:
                    open = false;
                    break;
            }
        }
    }

    private void OperationMenu()
    {
        bool open = true;

        while (open)
        {
            Console.Clear();

            AnsiConsole.MarkupLine($"Habit: [#FCCA46]{Habit.Name}[/]");

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOperation>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<MenuOperation>())
                );

            switch (menuOption)
            {
                case MenuOperation.Insert:
                    InsertValuesMenu();
                    break;
                case MenuOperation.Update:
                    UpdateValuesMenu();
                    break;
                case MenuOperation.View:
                    ViewHabitsValues();
                    break;
                case MenuOperation.Delete:
                    DeleteValuesMenu();
                    break;
                case MenuOperation.MoreConsults:
                    ConsultsMenu();
                    break;
                case MenuOperation.Exit:
                    open = false;
                    break;
            }
        }
    }

    private void ConsultsMenu()
    {
        bool open = true;

        while (open)
        {
            Console.Clear();

            AnsiConsole.MarkupLine($"Habit: [#FCCA46]{Habit.Name}[/]");

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<Consults>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<Consults>())
                );

            switch (menuOption)
            {
                case Consults.Total:
                    Total();
                    break;
                case Consults.AveragePerMonth:
                    AverageMonth();
                    break;
                case Consults.AveragePerYear:
                    AverageYear();
                    break;
                case Consults.Exit:
                    open = false;
                    break;
            }
        }
    }

    private void InsertValuesMenu()
    {
        string date = GetDateInput("Insert date [#619B8A](dd-MM-yy)[/], [#A76D60]T[/] to insert today's date or [#9A031E]0[/] to cancel:");
        if (date == "0") return;

        AnsiConsole.MarkupLine($"Date entered: [#A76D60]{date}[/]");

        int? quantityI = null;
        double? quantityD = null;

        if (Habit.Type == "I")
        {
            quantityI = GetIntInput("Insert the quantity [#619B8A](No Decimals)[/] or type [#9A031E]0[/] to return:");
            if (quantityI == 0) return;

            DataBase.Insert(Habit.Id, quantityI, null, date);
        }
        else if (Habit.Type == "D")
        {
            quantityD = GetDecimalInput("Insert the quantity [#619B8A](Decimals)[/] or type [#9A031E]0[/] to return:");
            if (quantityD == 0) return;

            DataBase.Insert(Habit.Id, null, quantityD, date);
        }
        else
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Error[/]");
            Console.ReadKey();
        }
    }

    private void UpdateValuesMenu()
    {
        ViewHabitsValues();

        int id = GetIntInput("Insert the [#619B8A]ID[/] to update or type [#9A031E]0[/] to return:");
        if (id == 0) return;

        if (!DataBase.ExistValue(id))
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Record not found.[/]");
            Console.ReadKey();
            return;
        }

        string date = GetDateInput("Insert date [#619B8A](dd-MM-yy)[/], [#75C9C8]T[/] to insert today's date or  [#9A031E]0[/] to cancel:");
        if (date == "0") return;

        int? quantityI = null;
        double? quantityD = null;

        if (Habit.Type == "I")
        {
            quantityI = GetIntInput("Insert the quantity [#619B8A](No Decimals)[/] or type [#9A031E]0[/] to return:");
            if (quantityI == 0) return;

            DataBase.Update(id, date, quantityI, null);
        }
        else if (Habit.Type == "D")
        {
            quantityD = GetDecimalInput("Insert the quantity [#619B8A](Decimals)[/] or type [#9A031E]0[/] to return:");
            if (quantityD == 0) return;

            DataBase.Update(id, date, null, quantityD);
        }
        else
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Error.[/]");
            Console.ReadKey();
        }
    }

    private void DeleteValuesMenu()
    {
        ViewHabitsValues();

        int id = GetIntInput("Insert [#619B8A]ID[/] to delete or type [#9A031E]0[/] to exit:");
        if (id == 0) return;

        if (DataBase.Delete(id))
        {
            AnsiConsole.MarkupLine("[#4CA97C]Record deleted[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Record not found[/]");
        }

        Console.ReadKey();
    }

    private void CreateHabit()
    {
        AnsiConsole.MarkupLine("Insert the name of the new habit [#619B8A](No numbers)[/]");
        string HabitName = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(HabitName) || HabitName.Any(char.IsDigit))
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Please insert a valid name[/]");
            HabitName = Console.ReadLine();
        }

        AnsiConsole.MarkupLine("Choose type: I [#619B8A](integer)[/] or D [#619B8A](double)[/]");

        string typeValueInput = Console.ReadLine().ToUpper();

        while (typeValueInput != "I" && typeValueInput != "D")
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Invalid option.[/] Enter [#619B8A]I[/] or [#619B8A]D[/]:");
            typeValueInput = Console.ReadLine().ToUpper();
        }


        DataBase.InsertHabit(HabitName, typeValueInput);

        AnsiConsole.MarkupLine("[#4CA97C]Habit Create successfully[/]");
        Console.ReadKey();
    }

    private void ViewHabits()
    {
        List<Habits> habits = DataBase.GetHabits();

        if (habits.Count == 0)
        {
            AnsiConsole.MarkupLine("Empty List");
            Console.ReadKey();

            return;
        }

        var table = new Table();
        table.AddColumns("[#DBB957]ID[/]", "[#DBB957]Habit[/]", "[#DBB957]Type[/]");

        foreach (var r in habits)
        {
            table.AddRow(
                $"[#A9FBD7]{r.Id}[/]",
                r.Name,
                r.Type
                );
        }

        table.Expand();
        AnsiConsole.Write(table);

        int habitId = GetIntInput("Enter the habit [#619B8A]ID[/]");

        while (!(DataBase.HabitExist(habitId) || habitId == 0))
        {
            habitId = GetIntInput("Select an existing habit or enter [#9A031E]0[/] to return.");
        }

        if (habitId == 0)
        {
            Menu();
        }

        Habit.Id = habitId;
        Habit.Name = DataBase.GetHabitName(habitId);
        Habit.Type = DataBase.GetHabitType(habitId);

        OperationMenu();
    }

    private void ViewHabitsValues()
    {
        Console.Clear();

        AnsiConsole.MarkupLine($"Habit: [#FCCA46]{Habit.Name}[/]");

        List<HabitsValues> records = DataBase.GetHabitsValues(Habit.Id);

        if (records.Count == 0)
        {
            AnsiConsole.MarkupLine("Empty List");
            Console.ReadKey();

            return;
        }

        var table = new Table();
        table.AddColumns("[#DBB957]Id[/]", "[#DBB957]Date[/]", "[#DBB957]Quantity[/]");

        foreach (var r in records)
        {
            string value = r.ValueInt is null ? r.ValueDec.Value.ToString("0.##", CultureInfo.InvariantCulture) : r.ValueInt.ToString();

            table.AddRow(
                $"[#A9FBD7]{r.Id}[/]",
                r.Date.ToString("dd-MM-yyyy"),
                value
            );
        }

        table.Expand();
        AnsiConsole.Write(table);
        Console.ReadKey();
    }

    internal string GetDateInput(string message)
    {
        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.Equals(input, "T", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.ToString("dd-MM-yy");
            }

            if (input == "0")
            {
                return "0";
            }

            if (DateTime.TryParseExact(input, "dd-MM-yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                return parsed.ToString("dd-MM-yy");
            }

            message = "[#FF0A0A]Invalid date.[/] Try again:";
        }
    }

    internal int GetIntInput(string message)
    {
        AnsiConsole.MarkupLine(message);

        string input = Console.ReadLine();

        while ((!int.TryParse(input, out int n) || n < 0) && input != "0")
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Invalid number.[/] Try again:");
            input = Console.ReadLine();
        }

        return int.Parse(input);
    }

    internal double GetDecimalInput(string message)
    {
        AnsiConsole.MarkupLine(message);

        string input = Console.ReadLine();

        input = input.Replace(".", ",");

        while ((!double.TryParse(input, out double n) || n < 0) && input != "0")
        {
            AnsiConsole.MarkupLine("[#FF0A0A]Invalid number.[/] Try again:");
            input = Console.ReadLine();
        }

        return double.Parse(input);
    }

    internal void Total()
    {
        double total = DataBase.GetTotalQuantity(Habit.Id, Habit.Type);

        ShowSingleResultTable("Total", "All records", total);
    }

    private void AverageYear()
    {
        while (true)
        {
            string year = GetValidYear();
            if (year == "0") return;

            if (!DataBase.YearExists(Habit.Id, year))
            {
                AnsiConsole.MarkupLine($"[#FF0A0A]No records found for year {year}[/]");
                Console.ReadKey();
                continue;
            }

            double avg = DataBase.AveragePerYear(Habit.Id, Habit.Type, year);

            ShowSingleResultTable("Year", year, avg);
            return;
        }
    }

    private void AverageMonth()
    {
        while (true)
        {
            string year = GetValidYear();
            if (year == "0") return;

            if (!DataBase.YearExists(Habit.Id, year))
            {
                AnsiConsole.MarkupLine($"[#FF0A0A]No records found for year {year}[/]");
                Console.ReadKey();
                continue;
            }

            string month = GetValidMonth();
            if (month == "0") return;

            if (!DataBase.MonthExists(Habit.Id, month, year))
            {
                AnsiConsole.MarkupLine($"[#FF0A0A]No records found for {month}-{year}[/]");
                Console.ReadKey();
                continue;
            }

            double avg = DataBase.AveragePerMonth(Habit.Id, Habit.Type, month, year);

            var table = new Table();
            table.AddColumn("[#DBB957]Month[/]");
            table.AddColumn("[#DBB957]Year[/]");
            table.AddColumn("[#DBB957]Average[/]");

            table.AddRow(month, year, avg.ToString("0.##", CultureInfo.InvariantCulture));

            table.Expand();
            AnsiConsole.Write(table);
            Console.ReadKey();
            return;
        }
    }

    internal string GetValidYear()
    {
        while (true)
        {
            int year = GetIntInput("Enter year (yy), example [#619B8A]24[/] or [#9A031E]0[/] to cancel:");
            if (year == 0) return "0";

            if (year >= 0 && year <= 99)
                return year.ToString("00");

            AnsiConsole.MarkupLine("[#FF0A0A]Invalid year. Use format yy (00–99)[/]");
        }
    }

    internal string GetValidMonth()
    {
        while (true)
        {
            int month = GetIntInput("Enter month (1–12) or [#9A031E]0[/] to cancel:");
            if (month == 0) return "0";

            if (month >= 1 && month <= 12)
                return month.ToString("00");

            AnsiConsole.MarkupLine("[#FF0A0A]Invalid month. Range 1–12[/]");
        }
    }

    private void ShowSingleResultTable(string label, string value, double result)
    {
        var table = new Table();
        table.AddColumn($"[#DBB957]{label}[/]");
        table.AddColumn("[#DBB957]Average[/]");

        table.AddRow(value, result.ToString("0.##", CultureInfo.InvariantCulture));

        table.Expand();
        AnsiConsole.Write(table);
        Console.ReadKey();
    }
}
