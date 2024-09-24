using System;
using System.Diagnostics;
using System.Threading.Tasks;

class NoSeg
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

        stopwatch.Stop();
        Console.WriteLine($"Tempo de execução (sem ArraySegment): {stopwatch.ElapsedMilliseconds} ms");


        // Exibir os primeiros 5 elementos para ver o resultado
        Console.WriteLine("Primeiros 5 elementos (sem ArraySegment):");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(largeArray[i]);
        }

        Console.ReadLine();
    }
}
