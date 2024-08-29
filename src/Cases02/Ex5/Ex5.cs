/*
Exercício 5: Pokédex (3 pontos)

Objetivo: Utilizar o TPL para realizar requisições HTTP de forma assíncrona.

Descrição: O programa receberá uma lista com P números inteiros, que representam os
identificadores de determinados pokémons na Pokédex. Para cada identificador na lista, a
aplicação deverá fazer uma requisição na PokeAPI para obter o nome e o(s) tipo(s) do
pokémon correspondente. Ao final, salve essas informações no arquivo pokedex.txt,
colocando os dados de um pokémon por linha. Utilize o seguinte formato para salvar as
informações: nome,tipo_1,...,tipo_N. Lembre-se de que um pokémon pode ter mais de um
tipo.
A. Implemente uma primeira versão sem paralelismo/concorrência
B. Implemente outra versão usando Parallel.For ou Parallel.ForEach
C. Implemente uma terceira versão usando funções assíncronas (async/await)
D. Calcule o speedup das versões B e C em relação a versão A. O que você observa?

Dica: Para obter os dados de um pokémon a partir de seu identificador na Pokédex, você
pode usar o endpoint https://pokeapi.co/api/v2/pokemon/{id}, onde id é o identificador do
pokémon. Uma requisição GET para esse endpoint retornará um JSON. Entre os campos
disponíveis, você deve recuperar os valores dos campos "name" e "types". A lista completa
de campos pode ser encontrada na documentação da API.
*/

using System;
using System.Threading.Tasks;

namespace Lab02.Ex5;

class Program
{
    static void Main(string[] args)
    {
        string[] input = Console.ReadLine().Split(' ');
        int P = input.Length;
        int[] pokemonIds = new int[P];

        for (int i = 0; i < P; i++)
        {
            pokemonIds[i] = int.Parse(input[i]);
        }

        // Continue a Implementação
        // ...
    }
}