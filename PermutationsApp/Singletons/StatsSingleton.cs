using PermutationsApp.DataModels;

namespace PermutationsApp.Singletons;

public sealed class StatsSingleton
{
    public int TotalWords { get; set; }
    
    private int TotalRequests { get; set; }
    
    private int SumProcessingTimeNs { get; set; }
    
    private static StatsSingleton instance = null;
    private static readonly object padlock = new object();

    public StatsSingleton()
    {
        TotalWords = 0;
        TotalRequests = 0;
        SumProcessingTimeNs = 0;
    }

    public static StatsSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new StatsSingleton();
                }
                return instance;
            }
        }
    }

    public void UpdateStats(int processingTimeNs)
    {
        lock (padlock)
        {
            TotalRequests++;
            SumProcessingTimeNs += processingTimeNs;
        }
    }

    public ApiStatsResponse GetStats()
    {
        lock (padlock)
        {
            var stats = new ApiStatsResponse()
            {
                AvgProcessingTimeNs = TotalRequests != 0 ? SumProcessingTimeNs / TotalRequests : 0,
                TotalRequests = TotalRequests,
                TotalWords = TotalWords != 0 ? TotalWords : EnglishDictionarySingleton.Instance.EnglishDictionary.Count
            };
            
            return stats;
        }
    }
}