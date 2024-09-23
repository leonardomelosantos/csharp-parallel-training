/*
Gerenciamento de Pedidos (2 pontos)

Descrição: Você deve implementar um sistema de gerenciamento de _pedidos para um
restaurante com um número limitado de _cozinheiros. Cada cozinheiro pode preparar apenas
um pedido por vez. Para garantir que o número de _pedidos sendo processados ao mesmo
tempo não exceda o número de _cozinheiros disponíveis, utilize semáforos. O programa
deve receber o número de _pedidos P e o número de _cozinheiros C, seguido de P linhas
contendo o nome do prato e a quantidade de tempo necessária para preparar o prato.
Instruções:
1. Use um semáforo para limitar o número de _pedidos que podem ser processados
simultaneamente.
2. Simule o tempo de preparação de cada pedido usando Task.Delay.
3. Imprima uma mensagem quando o preparo de um pedido começar e quando
terminar.
4. Garanta que nunca mais _pedidos sejam processados simultaneamente do que o
número de _cozinheiros.

Dicas:
● Utilize o SemaphoreSlim para controlar o acesso aos recursos.
● Utilize Task.WhenAll para aguardar a conclusão de todos os _pedidos.

Exemplo:
Entrada:
4 2
Moqueca de Peixe, 50
Bife à Cavalo, 40
Frango com Quiabo, 40
Lasanha, 50

Saída:

O prato 'Moqueca de Peixe' começou a ser preparado.
O prato 'Bife à Cavalo' começou a ser preparado.
O prato 'Bife à Cavalo' está pronto.
O prato 'Frango com Quiabo' começou a ser preparado.
O prato 'Moqueca de Peixe' está pronto.
O prato 'Lasanha' começou a ser preparado.
O prato 'Frango com Quiabo' está pronto.
O prato 'Lasanha' está pronto.
*/

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex3;

class Program
{
    static void Main(string[] args)
    {
        // Obter o número de _pedidos (P) e de _cozinheiros (C)
        string[] entrada = Console.ReadLine().Split();
        int P = int.Parse(entrada[0]); // Pedidos
        int C = int.Parse(entrada[1]); // Cozinheiros

        List<(string prato, int tempo)> pedidos = new List<(string prato, int tempo)>();

        for (int i = 0; i < P; i++)
        {
            string[] pedido = Console.ReadLine().Split(',');
            string prato = pedido[0].Trim();
            int tempo = int.Parse(pedido[1].Trim());
            pedidos.Add((prato, tempo));
        }

        // Continue a implementação
        Cozinha cozinha = new Cozinha(C, pedidos);
        cozinha.Iniciar();
    }
}

class Cozinha
{
    private readonly SemaphoreSlim _semaphoreSlim;
    private readonly int _pedidos;
    private readonly List<(string prato, int tempo)> _pedidosFila;
    private readonly List<Task> _pedidosTasks = new();

    public Cozinha(int cozinheiros, List<(string prato, int tempo)> pedidosFila)
    {
        _semaphoreSlim = new SemaphoreSlim(cozinheiros);
        _pedidos = pedidosFila.Count;
        _pedidosFila = pedidosFila;
    }

    public void Iniciar()
    {
        foreach ((string prato, int tempo) pedido in _pedidosFila)
        {
            Task t = PrepararProximoPrato(pedido.prato, pedido.tempo);
            _pedidosTasks.Add(t);
        }

        Task.WhenAll(_pedidosTasks.ToArray())
            .Wait();
    }

    private async Task PrepararProximoPrato(string prato, int tempo)
    {
        try
        {
            _semaphoreSlim?.Wait();
            Console.WriteLine($"O prato '{prato}' começou a ser preparado.");
            await Task.Delay(tempo);
            Console.WriteLine($"O prato '{prato}' está pronto.");
        }
        finally
        {
            _semaphoreSlim?.Release();
        }
    }
}
