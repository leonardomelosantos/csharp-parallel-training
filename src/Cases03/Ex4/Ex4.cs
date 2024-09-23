/*
Sistema de Logística FastDelivery (2 pontos)
Descrição: A FastDelivery está desenvolvendo um sistema automatizado para gerenciar
suas entregas expressas. Nesse sistema, os Vendedores são responsáveis por registrar
solicitações de entrega, enquanto os Entregadores coletam essas solicitações e as
entregam aos clientes. Para garantir um fluxo eficiente, o sistema utiliza uma fila
compartilhada e segura para comunicação entre vendedores e entregadoresTasks. Considere que
todos os Vendedores vendem os mesmos produtos e todos os Entregadores podem
entregar qualquer produto.

Implemente um programa que simule esse sistema utilizando uma coleção concorrente. O
programa deve:
1. Criar N solicitações de entrega no total.
2. Distribuir a tarefa de criação de solicitações entre V Vendedores.
3. Distribuir a tarefa de entrega entre E Entregadores.
4. Cada Vendedor cria solicitações de entrega em intervalos aleatórios (Task.Delay) e
os coloca na fila.
5. Cada Entregador remove _pedidos da fila e imprime uma mensagem de entrega.

Exemplo:
Entrada: 2 2 2

Saída:
Vendedor 1: Pedido #001 criado.
Entregador 1: Pedido #001 entregue.
Vendedor 2: Pedido #002 criado.
Entregador 2: Pedido #002 entregue.
*/

using System.Collections.Concurrent;

namespace Lab03.Ex4;

class Program
{
    static void Main(string[] args)
    {
        // Leitura do número total de pedidos (N), vendedores (V) e entregadoresTasks (E)
        string[] entrada = Console.ReadLine().Split();
        int N = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);
        int E = int.Parse(entrada[2]);

        // Continue a implementação
        FastDelivery fastDelivery = new(N);
        fastDelivery.ProcessarPedidos(N, V, E);
    }
}

public class FastDelivery
{
    private const int DELAY_PROCESSAMENTO = 300;
    private const int DELAY_PROCESSAMENTO_FILA = 600;

    private ConcurrentQueue<int> _totalDeSolicitacoes;
    private BlockingCollection<int> _pedidosLiberadosParaEntrega;

    private List<Task> entregadoresTasks = new();
    private List<Task> vendedoresTasks = new();


    public FastDelivery(int totalPedidos)
    {
        _totalDeSolicitacoes = new();
        _pedidosLiberadosParaEntrega = new(totalPedidos);
    }

    public void ProcessarPedidos(int N, int V, int E)
    {
        for (int indexPedido = 1; indexPedido <= N; indexPedido++)
        {
            _totalDeSolicitacoes.Enqueue(indexPedido); // Registrar solicitação/pedido numa fila
        }

        for (int indexVendedor = 1; indexVendedor <= V; indexVendedor++)
        {
            int identificacaoVendedor = indexVendedor;
            Task vendaTask = Task.Run(async () => { await EfetivarVendaDeAlgumPedidoAsync(identificacaoVendedor); });

            vendedoresTasks.Add(vendaTask);
            entregadoresTasks.Add(vendaTask);
        }

        for (int indexEntregador = 1; indexEntregador <= E; indexEntregador++)
        {
            int identificacaoEntregador = indexEntregador;
            Task entregadorTask = Task.Run(() => { EntregarPedidoVendido(identificacaoEntregador); });
            entregadoresTasks.Add(entregadorTask);
        }

        Task.WhenAll(vendedoresTasks)
            .ContinueWith((act) => { FinalizarProcesso(); });

        Task.WaitAll(entregadoresTasks.ToArray());
    }

    internal void FinalizarProcesso()
    {
        _pedidosLiberadosParaEntrega.CompleteAdding();
    }

    internal async Task EfetivarVendaDeAlgumPedidoAsync(int vendedor)
    {
        Random numeroAleatorio = new Random();
        await Task.Delay(numeroAleatorio.Next(1, DELAY_PROCESSAMENTO));

        while (_totalDeSolicitacoes.TryDequeue(out int pedido))
        {
            Console.WriteLine($"Vendedor {vendedor}: Pedido #{pedido:000} criado.");
            _pedidosLiberadosParaEntrega.Add(pedido);

            await Task.Delay(numeroAleatorio.Next(1, DELAY_PROCESSAMENTO_FILA));
        }
    }

    internal void EntregarPedidoVendido(int entregador)
    {
        foreach (int pedido in _pedidosLiberadosParaEntrega.GetConsumingEnumerable())
        {
            Console.WriteLine($"Entregador {entregador}: Pedido #{pedido:000} entregue.");
        }
    }
}