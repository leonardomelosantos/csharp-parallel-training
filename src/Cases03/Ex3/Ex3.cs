/*
Gerenciamento de Pedidos (2 pontos)

Descrição: Você deve implementar um sistema de gerenciamento de pedidos para um
restaurante com um número limitado de cozinheiros. Cada cozinheiro pode preparar apenas
um pedido por vez. Para garantir que o número de pedidos sendo processados ao mesmo
tempo não exceda o número de cozinheiros disponíveis, utilize semáforos. O programa
deve receber o número de pedidos P e o número de cozinheiros C, seguido de P linhas
contendo o nome do prato e a quantidade de tempo necessária para preparar o prato.
Instruções:
1. Use um semáforo para limitar o número de pedidos que podem ser processados
simultaneamente.
2. Simule o tempo de preparação de cada pedido usando Task.Delay.
3. Imprima uma mensagem quando o preparo de um pedido começar e quando
terminar.
4. Garanta que nunca mais pedidos sejam processados simultaneamente do que o
número de cozinheiros.

Dicas:
● Utilize o SemaphoreSlim para controlar o acesso aos recursos.
● Utilize Task.WhenAll para aguardar a conclusão de todos os pedidos.

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
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex3;

class Program
{
    static void Main(string[] args)
    {

        // Obter o número de pedidos (P) e de cozinheiros (C)
        string[] entrada = Console.ReadLine().Split();
        int P = int.Parse(entrada[0]);
        int C = int.Parse(entrada[1]);

        var pedidos = new List<(string prato, int tempo)>();

        for (int i = 0; i < P; i++)
        {
            string[] pedido = Console.ReadLine().Split(',');
            string prato = pedido[0].Trim();
            int tempo = int.Parse(pedido[1].Trim());
            pedidos.Add((prato, tempo));
        }

        // Continue a implementação
    }
}
