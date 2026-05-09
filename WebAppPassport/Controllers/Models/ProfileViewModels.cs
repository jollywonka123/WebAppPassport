namespace WebAppPassport.Controllers.Models;

public class ProfileViewModel
{
    public required string Username { get; set; }
    public List<PassportListItemViewModel>? Passports { get; set; }
    public int? PassportCount { get; set; }
    public List<CountrySummaryViewModel>? Countries { get; set; }
    public int? CountryCount { get; set; }
}

public class VisibilityRequest
{
    public required bool Show { get; set; }
}
