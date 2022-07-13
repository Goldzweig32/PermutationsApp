namespace PermutationsApp.Singletons;

public sealed class StatsSingleton
{
    public int TotalWords { get; set; }
    
    public int TotalRequests { get; set; }
    
    public int SumProcessingTimeNs { get; set; }
    
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
}