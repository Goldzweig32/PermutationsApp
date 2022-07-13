using PermutationsApp.DataModels;
using PermutationsApp.Singletons;

namespace PermutationsApp.Services;

public class PermutationsService
{

    public ApiStatsResponse GetStats()
    {
        Console.WriteLine($"[{DateTime.Now} | INFO] Stats is being prepared!");
        return new ApiStatsResponse()
        {
            AvgProcessingTimeNs = StatsSingleton.Instance.TotalRequests != 0 ? StatsSingleton.Instance.SumProcessingTimeNs / StatsSingleton.Instance.TotalRequests : 0,
            TotalRequests = StatsSingleton.Instance.TotalRequests,
            TotalWords = StatsSingleton.Instance.TotalWords != 0 ? StatsSingleton.Instance.TotalWords : EnglishDictionarySingleton.Instance.EnglishDictionary.Count
        };
    }

    public List<string> SimilarWords(string givenWord)
    {
        Console.WriteLine($"[{DateTime.Now} | INFO] Word permutation process start!");

        //Create histogram of the given word
        var wordDictionary = GetWordDictionary(givenWord);

        if (givenWord.Length > EnglishDictionarySingleton.Instance.EnglishDictionary.Count || givenWord.Length < 1)
        {
            return new List<string>();
        }
        
        //Get the relevant dictionary (list that contains only the words in the length of the given word)
        var onlyRelevantWordsByLength = EnglishDictionarySingleton.Instance.EnglishDictionary[givenWord.Length - 1];

        var listOfPermutations = PermutationsOfGivenWord(wordDictionary, onlyRelevantWordsByLength);
        
        if (listOfPermutations.Contains(givenWord))
        {
            listOfPermutations.Remove(givenWord);
        }
        
        Console.WriteLine($"[{DateTime.Now} | INFO] Word permutation process end!");
        return listOfPermutations;
    }
    

    private List<string> PermutationsOfGivenWord(Dictionary<char, int> wordDictionary, List<string> engDictionary)
    {
        var listOfPermutations = new List<string>();
        
        //Iterating over the words in the dictionary
        foreach (var word in engDictionary)
        {
            var cloneWordDictionary = new Dictionary<char, int>(wordDictionary);
            var isPossiblePermutation = true;
            //Iterating over the letters
            foreach (var letter in word)
            {
                //If the letter exists in the word histogram reduce the counter of this letter in 1
                if (cloneWordDictionary.ContainsKey(letter))
                {
                    cloneWordDictionary[letter]--;
                }
                else
                {
                    isPossiblePermutation = false;
                    break;
                }
            }

            if (isPossiblePermutation)
            {
                foreach (var entry in cloneWordDictionary)
                {
                    //If all the values in the histogram reduced to zero it's a match
                    if (entry.Value != 0)
                    {
                        isPossiblePermutation = false;
                        break;
                    }
                }
            }

            if (isPossiblePermutation)
            {
                Console.WriteLine($"[{DateTime.Now} | INFO] {word} is a permutation of the given word");
                listOfPermutations.Add(word);
            }
        }

        return listOfPermutations;
    }

    private Dictionary<char, int> GetWordDictionary(string word)
    {
        var dictionary = new Dictionary<char, int>();
        foreach (var letter in word)
        {
            if (dictionary.ContainsKey(letter))
            {
                dictionary[letter]++;
            }
            else
            {
                dictionary.Add(letter, 1);
            }
        }

        return dictionary;
    }
}