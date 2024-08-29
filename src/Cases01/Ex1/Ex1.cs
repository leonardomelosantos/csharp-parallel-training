namespace Lab01.Ex1;

class Program
{
    static void Main(string[] args)
    {
        (int quantidadeThreads, string[] frases) = GetEntradas(args);

        if (!IsEntradasValidas(quantidadeThreads, frases))
        {
            return;
        }

        CriarThreads(quantidadeThreads, frases);

        Console.WriteLine("Programa terminou");
    }

    private static void CriarThreads(int quantidadeThreads, string[] frases)
    {
        foreach (string frase in frases)
        {
            Thread novaThread = new Thread(ExecutarThread);
            novaThread.Start(frase);
            novaThread.Join();
        }
    }

    private static void ExecutarThread(object? obj)
    {
        if (obj is string frase)
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: " + frase);
        }
    }

    private static (int, string[]) GetEntradas(string[] args)
    {
        string? quantidadeThreadasStr = Console.ReadLine();
        if (!int.TryParse(quantidadeThreadasStr, out int quantidadeThreads))
        {
            return (0, Array.Empty<string>());
        }

        string[] frases = new string[quantidadeThreads];
        for (int i = 0; i < quantidadeThreads; i++)
        {
            frases[i] = Console.ReadLine();
        }

        return (quantidadeThreads, frases);
    }

    /// <summary>
    /// Valida se as entradas estão consistentes.
    /// </summary>
    /// <param name="quantidadeThreads"></param>
    /// <param name="frases"></param>
    /// <returns></returns>
    private static bool IsEntradasValidas(int quantidadeThreads, string[] frases)
    {
        if (quantidadeThreads <= 0 || frases.Length == 0)
        {
            Console.WriteLine("Programa encerrado: parâmetros de entrada inválidos.");
            return false;
        }

        if (quantidadeThreads != frases.Length)
        {
            Console.WriteLine("Programa encerrado: número de threads é diferente do número de frases");
            return false;
        }
        return true;
    }
}
