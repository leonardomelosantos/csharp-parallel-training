/*
Exercício 3: Frequência das Palavras no Texto (2 pontos)

Objetivo: Utilizar paralelismo em tarefas de processamento de texto.

Descrição: Escreva um programa que recebe o nome de um arquivo de texto, um inteiro N
e N palavras. O programa deve contar, em paralelo, a quantidade de vezes que cada
palavra do conjunto aparece no texto contido no arquivo indicado na entrada.

Observação: O programa não deve distinguir letras maiúsculas e minúsculas. Por exemplo,
“Paralelo” e “paralelo”, devem ser consideradas como a mesma palavra pelo programa.
*/

namespace Lab02.Ex3;

class Program
{
    static void Main(string[] args)
    {
        (string[] words, string text) = GetInputData();

        string[] wordsFullText = GetFullTextWordsSeparated(text);

        ExecuteWholeCounter(words, wordsFullText);
    }

    private static void ExecuteWholeCounter(string[] words, string[] separatedWordsFullText)
    {
        Task[] tasks = new Task[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            tasks[i] = Task.Factory.StartNew(() => { WordCounter(word, separatedWordsFullText); });
        }
        Task.WaitAll(tasks);
    }

    private static void WordCounter(string word, string[] separatedWordsFullText)
    {
        int totalCount = 0;
        foreach (string s in separatedWordsFullText)
        {
            if (word.Trim().Equals(s.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                totalCount++;
            }
        }
        Console.WriteLine($"Result for '{word}': {totalCount} times");
    }

    private static string[] GetFullTextWordsSeparated(string text)
    {
        const string SEPARATOR = " ";

        string normalizaedText = text
            .Replace("-", SEPARATOR)
            .Replace(",", SEPARATOR)
            .Replace(".", SEPARATOR)
            .Replace("(", SEPARATOR)
            .Replace(")", SEPARATOR)
            .Replace("\"", SEPARATOR)
            .Replace(Environment.NewLine, SEPARATOR);

        return normalizaedText.Split(SEPARATOR);
    }

    private static (string[] words, string text) GetInputData()
    {
        string fileName = Console.ReadLine();
        int N = int.Parse(Console.ReadLine());

        string[] words = new string[N];

        for (int i = 0; i < N; i++)
        {
            words[i] = Console.ReadLine();
        }

        string text;
        try
        {
            text = File.ReadAllText("Ex3\\" + fileName);
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao ler o arquivo: ", e.Message);
            return (words, "");
        }
        return (words, text);
    }
}