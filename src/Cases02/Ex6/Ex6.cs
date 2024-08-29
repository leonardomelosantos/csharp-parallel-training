/*
Exercício 6: Transações Bancárias (facultativo)

Objetivo: Exercitar a elaboração de aplicações utilizando funções assíncronas em C#.

Descrição: Implemente uma aplicação assíncrona para simular transações bancárias, onde
cada transação (depósito, saque, transferência) é realizada de forma assíncrona. A
aplicação deve garantir que todas as operações sejam concluídas corretamente, lidando
com possíveis exceções, como saldo insuficiente ou falhas durante as operações. Além
disso, registre todas as exceções em um arquivo de log separado e garanta que a
integridade dos dados seja mantida, mesmo em caso de falhas.

Detalhamento das transações bancárias:

    1.  Depósito: Adicione um valor ao saldo de uma conta bancária. Esta operação deve
        ser concluída de forma assíncrona, e você deve garantir que o saldo seja atualizado
        corretamente, mesmo com múltiplos depósitos simultâneos.
    2.  Saque: Subtrai um valor do saldo de uma conta. Antes de realizar o saque, verifique
        se a conta tem saldo suficiente. Se não tiver, uma exceção deve ser lançada e
        registrada no arquivo de log. A operação deve ser feita de maneira assíncrona.
    3.  Transferência: Transfere um valor de uma conta para outra. A transferência deve
        ocorrer em duas etapas: subtração do saldo da conta de origem e adição do saldo
        na conta de destino. Ambas as etapas devem ser tratadas como operações
        assíncronas, e a integridade das transações deve ser garantida. Se ocorrer um erro
        em qualquer uma das etapas, reverta a operação e registre o erro no log.
*/

using System;
using System.Threading.Tasks;

namespace Lab02.Ex6;

class Program {
    static void Main(string[] args)
    {
        // Continue a Implementação
        // ...
    }
}