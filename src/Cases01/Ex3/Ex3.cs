/*
RESULTADOS E OBSERVAÇÕES

3. Coloca o número de thread como parâmetro do programa e calcula o speedup das
duas versões usando vários números de threads (por exemplo 2/4/8/10/12/16). O que você observa?

    RESULTADO: Com base nos resultados abaixo, é possível perceber que há uma efiência melhor de tempo quando 
    se tem estrutura compartilhada. E é possível observar também que o melhor resultado foi com 2, 4 ou 8 threads;
    Ou seja, não necessariamente quanto mais threads obteremos melhor tempo de término.

    Valores com 100.000 numeros aleatorios (0 a 100)

                Versao 01                   Versao 02
                (compartilhando contador)   (contador privado)
    01 Thread   14ms                        20ms
    02 Threads  11ms                        14ms
    04 Threads  12ms                        14ms
    08 Threads  15ms                        16ms
    10 Threads  13ms                        17ms
    12 Threads  13ms                        17ms
    16 Threads  20ms                        20ms


4. Aumenta a quantidade de número aleatórios e o range dos números, o que você observa?

    RESULTADO: Com base nos resultados abaixo, diferentemente dos numeros anteriores, é possível perceber que há uma efiência 
    melhor de tempo quando se tem estrutura privada. E é possível observar também que o melhor resultado foi com 4 threads;
    Novamente é possível observar que não necessariamente quanto mais threads obteremos melhor speedup.

    Valores com 1.000.000 numeros aleatorios (0 a 9999)

                Versao 01                   Versao 02
                (compartilhando contador)   (contador privado)
    01 Thread   48ms                        37ms
    02 Threads  43ms                        41ms
    04 Threads  34ms                        33ms
    08 Threads  63ms                        54ms
    10 Threads  60ms                        61ms
    12 Threads  66ms                        71ms
    16 Threads  150ms                       128ms

5. Tente implementar uma terceira versão usando uma forma mais otimizada de agregar os resultados.

    RESULTADO: Adoção de Parallel.ForEach e ConcurrentDictionary mostraram piora nos resultados de performance.
    A adoção de lock e foreach tradicional, apresentou bons resultados.

*/

using System.Diagnostics;

namespace Lab01.Ex3;

class Program
{
    private static Barrier _barreiraThreads;
    private static Dictionary<int, int> _resultadoCompartilhado = new Dictionary<int, int>();
    private static String _lockUtil = new String(string.Empty);

    static void Main(string[] args)
    {
        Console.Write("Digite a quantidade de threads: ");
        int M = int.Parse(Console.ReadLine());

        Console.Write("Digite a quantidade de numeros: ");
        int N = int.Parse(Console.ReadLine());

        // Criar o vetor de tamanho N
        int[] vetor = new int[N];

        // Inicializar o vetor com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            vetor[i] = rand.Next(1, 9999);
        }

        // Iniciar a medição de tempo
        Stopwatch timer = new Stopwatch();
        timer.Start();

        // Continue a Implementação
        BuildAndExecuteThreads(vetor, M);

        while (_barreiraThreads.CurrentPhaseNumber == 0) { } // Esperar execução das Threads

        // Contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
    }

    private static void BuildAndExecuteThreads(int[] sequence, int qtdThreads)
    {
        _resultadoCompartilhado.Clear();
        int qtdNumerosPorThread = sequence.Length / qtdThreads;

        _barreiraThreads = new Barrier(qtdThreads);

        for (int indexThread = 1; indexThread <= qtdThreads; indexThread++)
        {
            Thread thread = new Thread(ContabilizarOcorrenciasThreadMethod);
            
            // Preparando o bloco de inteiros que foi repartido para enviar como parâmetro para cada thread.
            int[] parametroThread = (indexThread < qtdThreads || qtdThreads == 1)
                ? sequence.Skip((indexThread - 1) * qtdNumerosPorThread).Take(qtdNumerosPorThread).ToArray()
                : sequence.TakeLast(sequence.Length - ((indexThread - 1) * qtdNumerosPorThread)).ToArray();

            thread.Start(parametroThread);
        }
    }

    private static void ContabilizarOcorrenciasThreadMethod(object? paramThread)
    {
        if (paramThread is int[] arrayInts)
        {
            Dictionary<int, int> agrupamento = arrayInts
                .GroupBy(numeroInt => numeroInt)
                .ToDictionary(g => g.Key, v => v.Count()); // Contando as ocorrências de cada número do array

            lock (_resultadoCompartilhado) // Efetuando lock manualmente mostrou ser mais eficiente do uso de ConcurrentDictionary
            {
                foreach (var par in agrupamento) // Foreach tradicional se mostrou muito mais eficiente do que usar Parallel.ForEach
                {
                    if (_resultadoCompartilhado.ContainsKey(par.Key))
                    {
                        _resultadoCompartilhado[par.Key] += par.Value; // Somando as ocorrências para o número espeficado na key do dicionário
                    }
                    else
                    {
                        _resultadoCompartilhado.Add(par.Key, par.Value); // Colocando no dicionado a ocorrência do numero
                    }
                }
            }
        }
        _barreiraThreads.SignalAndWait();
    }
}
