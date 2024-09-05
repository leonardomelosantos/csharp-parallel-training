/*
Monitoramento de Temperatura (1.5 pontos)

Descrição: Implemente um programa que simula um sensor de temperatura que atualiza
seu valor periodicamente em intervalos aleatórios, permitindo que múltiplos usuários
(threads de leitura) consultem a temperatura simultaneamente. Utilize
ReaderWriterLockSlim para garantir leituras concorrentes seguras, enquanto a atualização
do sensor é feita de forma exclusiva. Cada usuário fará suas leituras em intervalos de
tempo aleatórios, e o sensor atualizará sua temperatura de forma independente, também
em intervalos aleatórios. 

O programa deve receber como entrada: O número de usuários U,
o número de atualizações do sensor S. Em seguida, U linhas onde cada linha especifica a
quantidade de leituras que um usuário realizará.

Entrada:
3 2
2
4
1

Saída:
Usuário 3: Temperatura lida: 0,00°C
Usuário 1: Temperatura lida: 0,00°C
Usuário 2: Temperatura lida: 0,00°C
Usuário 1: Temperatura lida: 0,00°C
[Sensor] Temperatura atualizada: 26,04°C
Usuário 2: Temperatura lida: 26,04°C
Usuário 2: Temperatura lida: 26,04°C
[Sensor] Temperatura atualizada: 3,50°C
Usuário 2: Temperatura lida: 3,50°C
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab03.Ex2;

class Program
{
    static ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
    static decimal _temperaturaSensor = 0;

    static void Main(string[] args)
    {
        // Obter a quantidade de usuários e atualizações do sensor
        string[] entrada = Console.ReadLine().Split();
        int usuarios = int.Parse(entrada[0]);
        int atualizacoes = int.Parse(entrada[1]);

        // Obter a quantidade de leituras realizadas por cada usuário
        int[] leituras = new int[usuarios];
        for(int i = 0; i < usuarios; i++) {
            leituras[i] = int.Parse(Console.ReadLine());
        }

        // Continue a implementação
        Task[] tasksUsuarios = new Task[usuarios];
        Task[] tasksAtualizacoes = new Task[atualizacoes];

        for (int i = 0; i < usuarios; i++)
        {
            Task task = Task.Factory.StartNew(() => { ExecutaLeiturasUsuario(i, leituras[i]); });
            tasksUsuarios[i] = task;
        }

        for (int i = 0; i < atualizacoes; i++)
        {
            Task task = Task.Factory.StartNew(() => { ExecutaAtualizacaoSensor(); });
            tasksAtualizacoes[i] = task;
        }

        Task.WaitAll(tasksUsuarios);
        Task.WaitAll(tasksAtualizacoes);
    }

    static void ExecutaLeiturasUsuario(int usuarioIndex, int qtdLeituras)
    {
        for(int i = 0;i < qtdLeituras; i++)
        {
            Task.Delay((new Random(1500)).Next()); // Delay aleatório
            _readerWriterLockSlim.EnterReadLock();
            decimal valorLeitura = _temperaturaSensor;
            _readerWriterLockSlim.ExitReadLock();
            Console.WriteLine($"Usuário {usuarioIndex+1}: Temperatura lida: {valorLeitura}°C");
        }
    }

    static void ExecutaAtualizacaoSensor()
    {
        Task.Delay(new Random(1500).Next()); // Delay aleatório
        _readerWriterLockSlim.EnterWriteLock();
        _temperaturaSensor = Math.Round((decimal)((new Random(10000).Next()) / 100), 2);
        Console.WriteLine($"[Sensor] Temperatura atualizada: {_temperaturaSensor}°C");
        _readerWriterLockSlim.ExitWriteLock();
    }
}
