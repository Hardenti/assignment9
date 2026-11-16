namespace AppointmentManager;

public class Appointment
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }

    public Appointment(string title, string description, DateTime date)
    {
        Title = title;
        Description = description;
        Date = date;
    }

    public string GetDisplayText()
    {
        return $"[{Date.ToString("MM/dd/yyyy HH:mm")}] {Title}{Environment.NewLine}{Description}";
    }
}
