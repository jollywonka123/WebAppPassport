using WebAppPassport.Common;

namespace WebAppPassport.Services.Models;

public class PassportCountryVisa
{
    public required VisaType VisaType { get; set; }
    public required Passport Passport { get; set; }
    public required Country Country { get; set; }
}