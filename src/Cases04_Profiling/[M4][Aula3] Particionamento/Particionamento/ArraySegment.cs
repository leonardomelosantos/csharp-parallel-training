using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ConcurrencyVisualizer.Instrumentation;

class ArraySegment
{
    static void Main()
    {
        var initMarker = Markers.EnterSpan("Inicialização");
        int[] largeArray = new int[1000000];
        // Inicializar o array com valores de 0 a 999999
        for (int i = 0; i < largeArray.Length; i++)
        {
            largeArray[i] = i;
        }

        // Copiar o array original para reutilização em ambas as abordagens
        int[] arrayForArraySegment = (int[])largeArray.Clone();
        int[] arrayForCopy = (int[])largeArray.Clone();
        initMarker.Leave();

        // Criar marcador para "Com ArraySegments"
        var withArraySegmentMarker = Markers.EnterSpan("Com ArraySegments");

        // Medir o tempo de execução usando ArraySegment
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        ProcessArrayWithArraySegment(arrayForArraySegment);
        stopwatch.Stop();
        Console.WriteLine($"Tempo de execução (com ArraySegment): {stopwatch.ElapsedMilliseconds} ms");

        // Sair do marcador para "Com ArraySegments"
        withArraySegmentMarker.Leave();

        // Criar marcador para "Sem ArraySegments"
        var withoutArraySegmentMarker = Markers.EnterSpan("Sem ArraySegments");

        // Medir o tempo de execução sem ArraySegment (fazendo cópias)
        stopwatch.Restart();
        ProcessArrayWithoutArraySegment(arrayForCopy);
        stopwatch.Stop();
        Console.WriteLine($"Tempo de execução (sem ArraySegment): {stopwatch.ElapsedMilliseconds} ms");

        // Sair do marcador para "Sem ArraySegments"
        withoutArraySegmentMarker.Leave();

        // Exibir os primeiros 5 elementos de cada abordagem para garantir que o processamento foi realizado
        Console.WriteLine("\nPrimeiros 5 elementos após ArraySegment:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(arrayForArraySegment[i]);
        }

        Console.WriteLine("\nPrimeiros 5 elementos após cópia:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(arrayForCopy[i]);
        }
    }

    static void ProcessArrayWithArraySegment(int[] largeArray)
    {
        int segmentSize = largeArray.Length / 10; // Dividir o array em 4 segmentos

        // Processar o array em paralelo usando ArraySegment
        Parallel.For(0, 4, segmentIndex =>
        {
            ArraySegment<int> segment = new ArraySegment<int>(largeArray, segmentIndex * segmentSize, segmentSize);

            // Processar o segmento (por exemplo, somar 1 a cada elemento)
            for (int i = segment.Offset; i < segment.Offset + segment.Count; i++)
            {
                segment.Array[i] += 1; // Atualizar o array original
            }
        });
    }

    static void ProcessArrayWithoutArraySegment(int[] largeArray)
    {
        int segmentSize = largeArray.Length / 10; // Dividir o array em 4 segmentos

        // Processar o array em paralelo sem ArraySegment (fazendo cópias)
        Parallel.For(0, 4, segmentIndex =>
        {
            // Criar um novo array copiando o segmento correspondente
            int[] segmentCopy = new int[segmentSize];
            Array.Copy(largeArray, segmentIndex * segmentSize, segmentCopy, 0, segmentSize);

            // Processar o segmento copiado
            for (int i = 0; i < segmentSize; i++)
            {
                segmentCopy[i] += 1; // Atualizar a cópia
            }

            // Copiar de volta para o array original
            Array.Copy(segmentCopy, 0, largeArray, segmentIndex * segmentSize, segmentSize);
        });
    }
}
