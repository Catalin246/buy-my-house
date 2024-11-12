public class MortgageApplication
{
    public int ApplicationId { get; set; }
    public int HouseId { get; set; }
    public int ApplicantId { get; set; }
    public string? Status { get; set; }
    public string? OfferDetails { get; set; }
    public House? House { get; set; } 
    public Applicant? Applicant { get; set; } 
}

