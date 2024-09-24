/*
Controle de Entrada e Saída de Veículos em um Estacionamento (3 pontos)

Descrição: Sua empresa foi designada para desenvolver um pequeno sistema para
gerenciar a entrada e saída de veículos em um estacionamento. O sistema deve controlar o
número de vagas disponíveis e garantir que o estacionamento nunca fique superlotado.
Para isso, você usará eventos para notificar o sistema quando um veículo entrar ou sair, e
sinais para sincronizar as ações dos veículos que chegam e saem do estacionamento.
● Estacionamento: O estacionamento tem um número limitado de vagas. O sistema
deve controlar a entrada e saída dos veículos de forma que, ao atingir a capacidade
máxima, nenhum outro veículo possa entrar até que uma vaga seja liberada.
● Veículos: Cada veículo é representado por uma Thread/Task que tenta entrar no
estacionamento. Quando um veículo entra, ele ocupa uma vaga e deve aguardar um
tempo aleatório (simulando o tempo que fica estacionado) antes de sair e liberar a
vaga. Se um veículo não consegue entrar no estacionamento, ele deve esperar um
tempo e tentar novamente.
● Eventos: O sistema deve emitir um evento sempre que um veículo entrar e sair do
estacionamento. Ao receber esses eventos, o sistema deve atualizar e exibir o
número de vagas disponíveis.
● Sinais: Utilize ManualResetEvent ou AutoResetEvent para controlar o momento em
que os veículos podem tentar entrar no estacionamento.

Seu programa deve receber como entrada a capacidade do estacionamento C e o número
de veículos que utilizaram o estacionamento qtdVeiculos. Imprima mensagens no console, conforme
os eventos (entrada e saída de veículos) ocorrerem no sistema.

Exemplo:
Entrada: 2 3

Saída:
Veículo 1 esperando para entrar...
Veículo 3 esperando para entrar...
Veículo 2 esperando para entrar...
Evento: Veículo entrou. Vagas disponíveis: 1
Veículo 1 estacionou.
Evento: Veículo entrou. Vagas disponíveis: 0
Veículo 2 estacionou.
Evento: Veículo saiu. Vagas disponíveis: 1
Veículo 1 saiu.
Evento: Veículo entrou. Vagas disponíveis: 0
Veículo 3 estacionou.
Evento: Veículo saiu. Vagas disponíveis: 1
Veículo 3 saiu.
Evento: Veículo saiu. Vagas disponíveis: 2
Veículo 2 saiu.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex5;

class Program
{
    static void Main()
    {
        // Obter a capacidade do estacionamento (C) e o número de veículos (qtdVeiculos)
        string[] entrada = Console.ReadLine().Split();
        int C = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);

        // Continue a implementação
        Parking p = new Parking(C);
        _ = p.IniciarMonitoramento();
        p.IniciarTasksSimulacao(V);
    }
}

public class Parking
{
    private const int DELAY_SIMULACAO_MINIMO_MILISEGUNDOS = 350;
    private const int DELAY_SIMULACAO_MAXIMO_MILISEGUNDOS = 1200;

    private EventWaitHandle _waitEvent;
    private SemaphoreSlim _lotacaoSemaphoreSlim;
    private SemaphoreSlim _umPorVezSemaphoreSlim = new(1);
    private volatile bool _monitoramentoAtivadoVolatile;
    private int _vagasDisponiveisAtomic;
    private string _sinalizacaoAcaoAtomic = string.Empty;
    private List<Task> veiculosTasks = new List<Task>();

    public Parking(int vagasDisponiveis)
    {
        _vagasDisponiveisAtomic = vagasDisponiveis;
        _waitEvent = new AutoResetEvent(false);
        _lotacaoSemaphoreSlim = new SemaphoreSlim(vagasDisponiveis);
    }

    internal Task IniciarMonitoramento()
    {
        _monitoramentoAtivadoVolatile = true;

        return Task.Run(() =>
        {
            while (_monitoramentoAtivadoVolatile)
            {
                _waitEvent.WaitOne(); // Wait for some signal
                Console.WriteLine($"Evento: Veículo {_sinalizacaoAcaoAtomic}. Vagas disponíveis: {_vagasDisponiveisAtomic}");
            }
        });
    }

    internal async Task TryStartVeiculoAsync(int identificacaoVeiculo)
    {
        await SimularEntrada(identificacaoVeiculo); // Local function, apenas para organizar melhor, sem ter excesso de metodos na classe

        await SimularDelayPermanencia(identificacaoVeiculo);
        
        SimularSaida(identificacaoVeiculo); // Local function, apenas para organizar melhor, sem ter excesso de metodos na classe

        async Task SimularEntrada(int identificacaoVeiculo)
        {
            Console.WriteLine($"Veículo {identificacaoVeiculo} esperando para entrar...");
            await _umPorVezSemaphoreSlim.WaitAsync();
            await _lotacaoSemaphoreSlim.WaitAsync();
            _umPorVezSemaphoreSlim.Release();

            Console.WriteLine($"Veículo {identificacaoVeiculo} estacionou.");

            Interlocked.Decrement(ref _vagasDisponiveisAtomic);
            Interlocked.Exchange(ref _sinalizacaoAcaoAtomic, "entrou");

            _waitEvent.Set(); // Send signal
        }

        void SimularSaida(int identificacaoVeiculo)
        {
            _lotacaoSemaphoreSlim.Release();
            Console.WriteLine($"Veículo {identificacaoVeiculo} saiu.");

            Interlocked.Increment(ref _vagasDisponiveisAtomic);
            Interlocked.Exchange(ref _sinalizacaoAcaoAtomic, "saiu");
            _waitEvent.Set(); // Send signal
        }
    }

    private static async Task SimularDelayPermanencia(int identificacaoVeiculo)
    {
        await Task.Delay(new Random().Next(DELAY_SIMULACAO_MINIMO_MILISEGUNDOS, DELAY_SIMULACAO_MAXIMO_MILISEGUNDOS));
    }

    internal void IniciarTasksSimulacao(int qtdVeiculos)
    {
        for (int indexVeiculo = 1; indexVeiculo <= qtdVeiculos; indexVeiculo++)
        {
            // Esse timer para simular uma fila de chegada, causou efeito colateral.
            //await Task.Delay(indexVeiculo * 10);

            int identificacaoVeiculo = indexVeiculo;
            Task veiculoTask = Task.Run(async () =>
            {
                await TryStartVeiculoAsync(identificacaoVeiculo);
            });
            veiculosTasks.Add(veiculoTask);
        }

        Task.WhenAll(veiculosTasks)
            .ContinueWith((obj) => { _monitoramentoAtivadoVolatile = false; })
            .Wait();
    }
}
