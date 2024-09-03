/*
Sistema de Logística FastDelivery (2 pontos)
Descrição: A FastDelivery está desenvolvendo um sistema automatizado para gerenciar
suas entregas expressas. Nesse sistema, os Vendedores são responsáveis por registrar
solicitações de entrega, enquanto os Entregadores coletam essas solicitações e as
entregam aos clientes. Para garantir um fluxo eficiente, o sistema utiliza uma fila
compartilhada e segura para comunicação entre vendedores e entregadores. Considere que
todos os Vendedores vendem os mesmos produtos e todos os Entregadores podem
entregar qualquer produto.

Implemente um programa que simule esse sistema utilizando uma coleção concorrente. O
programa deve:
1. Criar N solicitações de entrega no total.
2. Distribuir a tarefa de criação de solicitações entre V Vendedores.
3. Distribuir a tarefa de entrega entre E Entregadores.
4. Cada Vendedor cria solicitações de entrega em intervalos aleatórios (Task.Delay) e
os coloca na fila.
5. Cada Entregador remove pedidos da fila e imprime uma mensagem de entrega.

Exemplo:
Entrada: 2 2 2

Saída:
Vendedor 1: Pedido #001 criado.
Entregador 1: Pedido #001 entregue.
Vendedor 2: Pedido #002 criado.
Entregador 2: Pedido #002 entregue.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex4;

class Program
{
    static void Main(string[] args)
    {
        // Leitura do número total de pedidos (N), vendedores (V) e entregadores (E)
        string[] entrada = Console.ReadLine().Split();
        int N = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);
        int E = int.Parse(entrada[2]);

        // Continue a implementação

    }
}
