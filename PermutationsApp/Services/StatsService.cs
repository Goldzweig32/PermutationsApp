using PermutationsApp.DataModels;

namespace PermutationsApp.Services;

public sealed class StatsService 
{
    private int _totalRequests;

    private long _sumProcessingTimeNs;
    
    public StatsService(EnglishDictionaryService englishDictionaryService)
    {
        EnglishDictionaryService = englishDictionaryService;
    }
    
    private EnglishDictionaryService EnglishDictionaryService { get; }

    public void UpdateStats(long processingTimeNs)
    {
        Interlocked.Increment(ref _totalRequests);
        Interlocked.Add(ref _sumProcessingTimeNs, processingTimeNs);
    }

    public ApiStatsResponse GetStats()
    {
        var stats = new ApiStatsResponse()
        {
            TotalRequests = _totalRequests,
            TotalWords = EnglishDictionaryService.TotalWords
        };
        var avgProcessingTimeNs = _totalRequests != 0 ? _sumProcessingTimeNs / _totalRequests : 0;
        stats.AvgProcessingTimeNs = (int) avgProcessingTimeNs;
        return stats;
    }
}