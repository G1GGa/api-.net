namespace PhishingDetectorApi.Models;

public class CheckResponse
{
    public bool IsSafe { get; set; }
    public int RiskScore { get; set; }
    public string Reason { get; set; } = string.Empty;
}