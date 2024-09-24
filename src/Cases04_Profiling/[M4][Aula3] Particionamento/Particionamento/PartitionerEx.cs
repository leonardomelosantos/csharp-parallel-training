using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

class PartitionerEx
{
    static void Main()
    {
        // Criar uma lista grande de números
        int[] numbers = new int[1000];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i;
        }

        // Criar um Partitioner para dividir a coleção em partes
        var rangePartitioner = Partitioner.Create(0, numbers.Length);

        // Usar Parallel.ForEach para processar cada partição
        Parallel.ForEach(rangePartitioner, (range, state) =>
        {
            // range.Item1 é o índice inicial da partição
            // range.Item2 é o índice final da partição
            for (int i = range.Item1; i < range.Item2; i++)
            {
                // Processar o número (neste caso, apenas exibir)
                Console.WriteLine($"Processando número: {numbers[i]} na thread {Task.CurrentId}");
            }
        });

        Console.WriteLine("Processamento concluído.");
    }
}
