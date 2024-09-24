using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

class ProCons
{
    static async Task Main(string[] args)
    {
        // Criar o BufferBlock para compartilhar entre produtores e consumidores
        var bufferBlock = new BufferBlock<int>();

        // Instanciar o produtor e consumidor
        var producer = new Producer(bufferBlock);
        var consumer = new Consumer(bufferBlock);

        // Iniciar a produção e o consumo
        Task producerTask = producer.ProduceAsync();
        Task consumerTask = consumer.ConsumeAsync();

        // Aguardar ambos completarem
        await Task.WhenAll(producerTask, consumerTask);

        Console.WriteLine("Processo de produção e consumo concluído.");
    }
}

class Producer
{
    private readonly BufferBlock<int> _bufferBlock;

    public Producer(BufferBlock<int> bufferBlock)
    {
        _bufferBlock = bufferBlock;
    }

    public async Task ProduceAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Produzindo item {i}");
            await _bufferBlock.SendAsync(i); // Enviar dados para o buffer
            await Task.Delay(500); // Simula tempo de produção
        }

        // Sinaliza que a produção terminou
        _bufferBlock.Complete();
        Console.WriteLine("Produção concluída.");
    }
}

class Consumer
{
    private readonly BufferBlock<int> _bufferBlock;

    public Consumer(BufferBlock<int> bufferBlock)
    {
        _bufferBlock = bufferBlock;
    }

    public async Task ConsumeAsync()
    {
        while (await _bufferBlock.OutputAvailableAsync())
        {
            // Receber dados do buffer
            int item = await _bufferBlock.ReceiveAsync();
            Console.WriteLine($"Consumindo item {item}");

            // Simula tempo de processamento
            await Task.Delay(1000);
        }

        Console.WriteLine("Consumo concluído.");
    }
}
