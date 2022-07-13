namespace PermutationsApp.DataModels;

public class ApiStatsResponse
{
    public int TotalWords { get; set; }
    
    public int TotalRequests { get; set; }
    
    public int AvgProcessingTimeNs { get; set; }
}