namespace PermutationsApp.Services;

public class EnglishDictionaryService
{
    public EnglishDictionaryService()
    {
        EnglishDictionary = GetEnglishDictionaryFromFile();
    }
    
    public Dictionary<int, List<string>> EnglishDictionary { get;  }
    public int TotalWords { get; private set; }

    private Dictionary<int, List<string>> GetEnglishDictionaryFromFile()
    {
        var filePath = String.Format($"{GetExecutingFilePath()}/DB/words_clean.txt");
        var lines = File.ReadAllLines(filePath).AsParallel().ToList();
        TotalWords = lines.Count;
        return lines.GroupBy(GetWordHash).ToDictionary(v => v.Key, v => v.ToList());
    }

    private static string GetExecutingFilePath()
    {
        var rootDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        rootDir = rootDir.Substring(5, rootDir.Length - 5);
        return rootDir;
    }
    
    public static int GetWordHash(string word)
    {
        return word.Aggregate(word.Length, (f, s) => f ^ s.GetHashCode());
    }
}