/*
Resultados coletados com 10.000 números (modo Release)

1 thread    =   20ms    16ms    18ms    Média 18.0 ms
2 threads   =   16ms    18ms    20ms    Média 18.0 ms
4 threads   =   14ms    14ms    17ms    Média 15.0 ms
8 threads   =   13ms    19ms    24ms    Média 18.6 ms
16 threads  =   14ms    25ms    24ms    Média 21.0 ms
32 threads  =   22ms    23ms    25ms    Média 23.3 ms

Observações:
1) Existem pequenas variações de tempo, por isso que estabeleci média.
2) Observa-se que o aumento do número de threads não necessariamente melhora a performance total. Há tendência de aumento de duração quando aumenta-se threads.
*/

using System.Diagnostics;

namespace Lab01.Ex2;

class Program
{
    private static Barrier _barreiraThreads;
    private static String _lockUtil = new String(string.Empty);
    private static int totalSomadoPelasThreads = 0;

    static void Main(string[] args)
    {
        Console.Write("Digite a quantidade de threads: ");
        int M = int.Parse(Console.ReadLine());

        Console.Write("Digite a quantidade de numeros: ");
        int N = int.Parse(Console.ReadLine());

        int[] sequence = new int[N];

        // Inicializar o array com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.Next(1, 100);
        }

        // Iniciar a contagem do tempo de execução

        Stopwatch timer = new Stopwatch();
        timer.Start();

        // Continue a Implementação
        if (M > sequence.Length)
        {
            M = sequence.Length; // Evitando número de threads superior ao quantitativo de número para somar.
        }
        
        BuildAndExecuteThreads(sequence, M);

        while (_barreiraThreads.CurrentPhaseNumber == 0) { } // Esperar execução das Threads

        // Finalizar a contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
        Console.WriteLine("Resultado da soma: " + totalSomadoPelasThreads);
    }

    private static void BuildAndExecuteThreads(int[] sequence, int qtdThreads)
    {
        totalSomadoPelasThreads = 0;
        int qtdNumerosPorThread = sequence.Length / qtdThreads;

        _barreiraThreads = new Barrier(qtdThreads);

        for (int indexThread = 1; indexThread <= qtdThreads; indexThread++)
        {
            Thread thread = new Thread(SomarArray);

            // Preparando o bloco de inteiros que foi repartido para enviar como parâmetro para cada thread.
            int[] parametroThread = (indexThread < qtdThreads || qtdThreads == 1)
                ? sequence.Skip((indexThread - 1) * qtdNumerosPorThread).Take(qtdNumerosPorThread).ToArray()
                : sequence.TakeLast(sequence.Length - ((indexThread - 1) * qtdNumerosPorThread)).ToArray();

            thread.Start(parametroThread);
        }
    }

    private static void SomarArray(object? obj)
    {
        if (obj is int[] arrayInts)
        {
            var soma = 0;
            foreach (int i in arrayInts)
                soma += i;

            lock (_lockUtil)
            {
                totalSomadoPelasThreads += soma;
            }
        }
        _barreiraThreads.SignalAndWait();
    }
}

