/*
Exercício 4: Encontrar Números Primos (2.5 pontos)

Objetivo: Utilizar diferentes recursos da Task Parallel Library (TPL) do C#.

Descrição: Elabore uma aplicação que receba um número inteiro N, seguido por N
números inteiros. Para cada número k informado na entrada, a aplicação deve imprimir
todos os números primos entre 1 e k.
A. Faça uma versão que utilize Tasks.
B. Faça uma segunda versão que utiliza Parallel.For.
C. Faça uma terceira versão que utiliza Parallel.ForEach.
D. Compare o tempo de execução das três implementações.
*/

using System;
using System.Threading.Tasks;

namespace Lab02.Ex4;

class Program {
    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        string[] input = Console.ReadLine().Split(' ');
        int[] sequence = new int[N];

        for(int i = 0; i < N; i++) {
            sequence[i] = int.Parse(input[i]);
        }

        // Continue a Implementação
        // ...
    }
}