namespace MauiMastery.CommonControlsAndStyles.Models;

public class FormInputModel
{
    public string Username { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public bool AcceptsNewsletter { get; set; } = true;
    public double ExperienceYears { get; set; } = 3.5;
    public DateTime BirthDate { get; set; } = DateTime.Today.AddYears(-25);
}