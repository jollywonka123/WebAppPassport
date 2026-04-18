using WebAppPassport.Common;

namespace WebAppPassport.DataBase.Models;

public class PassportCountryVisa
{
    public Guid Id { get; set; }
    public Guid UsingPassportId { get; set; }
    public Guid ToCountryId { get; set; }
    
    public VisaType VisaType { get; set; }
    
    public required Passport Passport { get; set; }
    public required Country Country { get; set; }
    
}