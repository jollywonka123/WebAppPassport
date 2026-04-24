namespace WebAppPassport.Common;

public static class VisaTypeLabels
{
    public const string VisaFree = "Visa free";
    public const string VisaOnArrival = "Visa on arrival";
    public const string ETA = "ETA";
    public const string VisaRequired = "Visa required";

    public static string From(VisaType type) => type switch
    {
        VisaType.VisaFree => VisaFree,
        VisaType.VisaOnArrival => VisaOnArrival,
        VisaType.ETA => ETA,
        VisaType.RequiredVisa => VisaRequired,
        _ => VisaRequired
    };
}
