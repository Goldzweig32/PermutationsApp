using PermutationsApp.DataModels;

namespace PermutationsApp.Services;

public class PermutationsService
{
    public PermutationsService(StatsService statsService, EnglishDictionaryService englishDictionaryService)
    {
        StatsService = statsService;
        EnglishDictionaryService = englishDictionaryService;
    }
    
    private StatsService StatsService { get; }
    private EnglishDictionaryService EnglishDictionaryService { get; }
    
    public ApiStatsResponse GetStats()
    {
        Console.WriteLine($"[{DateTime.Now} | INFO] Stats is being prepared!");
        return StatsService.GetStats();
    }

    public async Task<List<string>> SimilarWords(string givenWord)
    {
        Console.WriteLine($"[{DateTime.Now} | INFO] Word permutation process start!");

        var words = EnglishDictionaryService.EnglishDictionary
            .GetValueOrDefault(EnglishDictionaryService.GetWordHash(givenWord));
        if (words == null)
        {
            return new List<string>();
        }

        //Create histogram of the given word
        var wordDictionary = GetWordDictionary(givenWord);

        var listOfPermutations = await PermutationsOfGivenWord(wordDictionary, words, givenWord.Length);
        
        if (listOfPermutations.Contains(givenWord))
        {
            listOfPermutations.Remove(givenWord);
        }
        
        Console.WriteLine($"[{DateTime.Now} | INFO] Word permutation process end!");
        return listOfPermutations;
    }
    
    private static async Task<List<string>> PermutationsOfGivenWord(Dictionary<char, int> wordDictionary, List<string> engDictionary, int givenWordLength)
    {
        var listOfPermutations = new List<string>();

        //Iterating over the words in the dictionary
        foreach (var word in engDictionary)
        {
            var counter = 0;
            var cloneWordDictionary = new Dictionary<char, int>(wordDictionary);
            //Iterating over the letters
            foreach (var letter in word)
            {
                //If the letter exists in the word histogram reduce the counter of this letter in 1
                if (cloneWordDictionary.ContainsKey(letter))
                {
                    if (cloneWordDictionary[letter] > 0)
                    {
                        counter++;
                    }
                    cloneWordDictionary[letter]--;
                }
                else
                {
                    break;
                }
            }

            if (counter == givenWordLength)
            {
                Console.WriteLine($"[{DateTime.Now} | INFO] {word} is a permutation of the given word");
                listOfPermutations.Add(word);
            }
        }

        return listOfPermutations;
    }

    private static Dictionary<char, int> GetWordDictionary(string word)
    {
        return word
            .GroupBy(v => v)
            .ToDictionary(v => v.Key, v => v.Count());
    }
}