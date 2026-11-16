using System.Globalization;

namespace AppointmentManager;

internal static class Program
{
    private static readonly List<Appointment> Appointments = new();

    private static void Main()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Appointment Manager");
            Console.WriteLine("1. Add Appointment");
            Console.WriteLine("2. View All Appointments");
            Console.WriteLine("3. View Upcoming (Next 7 Days)");
            Console.WriteLine("4. Search Appointments");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string? choice = Console.ReadLine();
            if (choice is null || choice.Trim() == "5")
            {
                Console.WriteLine("Goodbye!");
                return;
            }

            switch (choice.Trim())
            {
                case "1":
                    AddAppointment();
                    break;
                case "2":
                    DisplayAppointments(Appointments.OrderBy(appointment => appointment.Date));
                    break;
                case "3":
                    DisplayUpcomingAppointments();
                    break;
                case "4":
                    SearchAppointments();
                    break;
                default:
                    Console.WriteLine("Please enter a number from 1 to 5.");
                    break;
            }
        }
    }

    private static void AddAppointment()
    {
        string? title = ReadRequiredText("Enter appointment title: ");
        if (title is null)
        {
            return;
        }

        string? description = ReadRequiredText("Enter appointment description: ");
        if (description is null)
        {
            return;
        }

        DateTime date;
        while (true)
        {
            Console.Write("Enter date and time (MM/dd/yyyy HH:mm): ");
            string? input = Console.ReadLine();
            if (input is null)
            {
                return;
            }

            if (DateTime.TryParse(input, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                break;
            }

            Console.WriteLine("Invalid date. Please try again using MM/dd/yyyy HH:mm.");
        }

        Appointments.Add(new Appointment(title, description, date));
        Console.WriteLine("Appointment added.");
    }

    private static string? ReadRequiredText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (input is null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("This field cannot be empty. Please try again.");
        }
    }

    private static void DisplayUpcomingAppointments()
    {
        DateTime now = DateTime.Now;
        DateTime endOfWindow = now.AddDays(7);
        IEnumerable<Appointment> upcoming = Appointments
            .Where(appointment => appointment.Date >= now && appointment.Date <= endOfWindow)
            .OrderBy(appointment => appointment.Date);

        DisplayAppointments(upcoming);
    }

    private static void SearchAppointments()
    {
        string? searchTerm = ReadRequiredText("Enter search term: ");
        if (searchTerm is null)
        {
            return;
        }

        IEnumerable<Appointment> matches = Appointments
            .Where(appointment =>
                appointment.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                appointment.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(appointment => appointment.Date);

        DisplayAppointments(matches);
    }

    private static void DisplayAppointments(IEnumerable<Appointment> appointments)
    {
        List<Appointment> results = appointments.ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        foreach (Appointment appointment in results)
        {
            Console.WriteLine();
            Console.WriteLine(appointment.GetDisplayText());
        }
    }
}
