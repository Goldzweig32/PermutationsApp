namespace PermutationsApp.Singletons;

public class EnglishDictionarySingleton
{
    public List<List<string>> EnglishDictionary { get; set; }
    
    private static EnglishDictionarySingleton instance;
    private static readonly object padlock = new object();

    public EnglishDictionarySingleton()
    {
        EnglishDictionary = GetEnglishDictionaryFromFile();
    }
    
    public static EnglishDictionarySingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new EnglishDictionarySingleton();
                }
                return instance;
            }
        }
    }

    public void Initialize() {}

    private List<List<string>> GetEnglishDictionaryFromFile()
    {
        var filePath = String.Format($"{GetExecutingFilePath()}/DB/words_clean.txt");
        var lines = File.ReadAllLines(filePath).AsParallel().ToList();
        
        //Total words count
        StatsSingleton.Instance.TotalWords = lines.Count;

        //Get the length of the longest word in the dictionary
        var max = lines.MaxBy(x => x.Length)?.Length;

        if (max == null)
        {
            throw new InvalidOperationException("There was an error while reading the dictionary");
        }
        
        //Create dedicated list to every length of word
        var listOfListsByWords = new List<List<string>>();
        for (int i = 0; i < max; i++)
        {
            listOfListsByWords.Add(new List<string>());
        }

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null) {
                //Add the word to matching list by the length
                listOfListsByWords[line.Length - 1].Add(line);
            }
        }
        
        return listOfListsByWords;
    }
    
    private string GetExecutingFilePath()
    {
        var rootDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        rootDir = rootDir.Substring(5, rootDir.Length - 5);
        return rootDir;
    }
}