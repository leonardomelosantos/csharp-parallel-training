using System;
using System.Diagnostics;
using System.Threading.Tasks;

class Seg
{
    static void Main()
    {
        int[] largeArray = new int[1000000];
        // Inicializar o array com valores de 0 a 999999
        for (int i = 0; i < largeArray.Length; i++)
        {
            largeArray[i] = i;
        }

        int segmentSize = largeArray.Length / 4; // Dividir o array em 4 segmentos

        //Contar tempo de execução do laço
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        // Processar o array em paralelo usando ArraySegment
        Parallel.For(0, 4, segmentIndex =>
        {
            ArraySegment<int> segment = new ArraySegment<int>(largeArray, segmentIndex * segmentSize, segmentSize);

            // Processar o segmento (por exemplo, somar 1 a cada elemento)
            for (int i = segment.Offset; i < segment.Offset + segment.Count; i++)
            {
                largeArray[i] += 1; // Atualizar o array original
            }
        });
        stopwatch.Stop();
        Console.WriteLine($"Tempo de execução (com ArraySegment): {stopwatch.ElapsedMilliseconds} ms");

        // Exibir os primeiros 5 elementos para ver o resultado
        Console.WriteLine("Primeiros 5 elementos (com ArraySegment):");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(largeArray[i]);
        }

        Console.ReadLine();
    }
}
