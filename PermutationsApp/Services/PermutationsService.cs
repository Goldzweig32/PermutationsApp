using PermutationsApp.DataModels;
using PermutationsApp.Singletons;

namespace PermutationsApp.Services;

public class PermutationsService
{

    public ApiStatsResponse GetStats()
    {
        Console.WriteLine($"[{DateTime.Now} | INFO] Stats is being prepared!");
        return StatsSingleton.Instance.GetStats();
    }

    public async Task<List<string>> SimilarWords(string givenWord)
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

        var listOfPermutations = await PermutationsOfGivenWord(wordDictionary, onlyRelevantWordsByLength, givenWord.Length);
        
        if (listOfPermutations.Contains(givenWord))
        {
            listOfPermutations.Remove(givenWord);
        }
        
        Console.WriteLine($"[{DateTime.Now} | INFO] Word permutation process end!");
        return listOfPermutations;
    }

    private async Task<List<string>> PermutationsOfGivenWord(Dictionary<char, int> wordDictionary, List<string> engDictionary, int givenWordLength)
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