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
pode usar o endpoint https://pokeapi.co/api/v2/pokemon/{pokeId}, onde pokeId é o identificador do
pokémon. Uma requisição GET para esse endpoint retornará um JSON. Entre os campos
disponíveis, você deve recuperar os valores dos campos "Description" e "types". A lista completa
de campos pode ser encontrada na documentação da API.

====================
Minha observação é que a versao A (sequencial) claramente não tem bom desempenho, porque
seria uma fila sequencial sem vantagem alguma. 

Já as versões B e C tem bom desempenho. Observo que o uso do Parallel.ForEach teve uma
certa vantagem porque não foi necessário criar Tasks e esperar todas elas. A opção B
tem apenas uma contenção com um lock na hora de gravar arquivo.
 
 */

using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Lab02.Ex5;

class Program
{
    static void Main(string[] args)
    {
        int[] pokemonIds = GetInputData();

        // Continue a Implementação
        var tempoSequencial = new PokemonFinderVersionA().Execute(pokemonIds);
        var tempoParallel = new PokemonFinderVersionB().Execute(pokemonIds);
        var tempoAsyncAwait = new PokemonFinderVersionC().Execute(pokemonIds);

        Console.WriteLine(
            $"Sequencial: {tempoSequencial}ms, " +
            $"Parallel: {tempoParallel}ms, " +
            $"Async/Await: {tempoAsyncAwait}ms");
    }

    private static int[] GetInputData()
    {
        string[] input = Console.ReadLine().Split(' ');
        int P = input.Length;
        int[] pokemonIds = new int[P];

        for (int i = 0; i < P; i++)
        {
            pokemonIds[i] = int.Parse(input[i]);
        }

        return pokemonIds;
    }
}

public class PokemonFinderVersionA : PokemonFinderBase
{
    protected override void FindPokemons(int[] pokemonIds)
    {
        foreach (var id in pokemonIds)
        {
            EscreverArquivo(GetPokemonFromApi(id));
        }
    }
}

public class PokemonFinderVersionB : PokemonFinderBase
{
    protected override void FindPokemons(int[] pokemonIds)
    {
        Parallel.ForEach(pokemonIds, id =>
        {
            EscreverArquivo(GetPokemonFromApi(id));
        });
    }
}

public class PokemonFinderVersionC : PokemonFinderBase
{
    protected override void FindPokemons(int[] pokemonIds)
    {
        List<Task> tasks = new();
        foreach (int pokeId in pokemonIds)
        {
            Task taskPokemon = Task.Run(async () =>
            {
                Task<PokemonVO> pokemonVO = GetPokemonFromApiAsync(pokeId);
                EscreverArquivo(await pokemonVO);
            });
            tasks.Add(taskPokemon);
        }
        Task.WaitAll(tasks.ToArray());
    }

    private async Task<PokemonVO> GetPokemonFromApiAsync(int pokeId)
    {
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri(API_URL);
            HttpResponseMessage response = await client.GetAsync($"pokemon/{pokeId}/");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PokemonVO>(result);
            }
            return await Task.FromResult<PokemonVO>(null);
        }
    }
}

public abstract class PokemonFinderBase
{
    protected const string API_URL = "https://pokeapi.co/api/v2/";
    protected const string FILENAME = "pokedex.txt";

    private readonly object _lockWrite = new();

    protected abstract void FindPokemons(int[] pokemonIds); // Abstract

    public double Execute(int[] pokemonIds)
    {
        DeleteFileIfExist();

        Stopwatch timer = new();
        timer.Start();

        FindPokemons(pokemonIds); // Abstract

        timer.Stop();
        return timer.ElapsedMilliseconds;
    }

    private void DeleteFileIfExist()
    {
        lock (_lockWrite)
        {
            FileInfo file = new FileInfo(FILENAME);
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }

    protected void EscreverArquivo(PokemonVO pokemon)
    {
        lock (_lockWrite)
        {
            using (StreamWriter stream = File.AppendText(FILENAME))
            {
                stream.WriteLine(pokemon.ToString());
            }
        }
    }

    protected PokemonVO GetPokemonFromApi(int pokeId)
    {
        HttpWebRequest request = WebRequest.Create($"{API_URL}pokemon/{pokeId}/") as HttpWebRequest;
        request.Method = "GET";

        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string httpResult = reader.ReadToEnd();
                return JsonSerializer.Deserialize<PokemonVO>(httpResult);
            }
        }
    }
}

public class PokemonVO
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int Id { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("types")]
    public IList<PokemonTypeEntityVO> Types { get; set; }

    private string GetJoinedTypes()
    {
        IEnumerable<string> tipos = Types.Select(tipo => tipo.ToString());
        return string.Join(", ", tipos);
    }

    public override string ToString()
    {
        return $"{Name}, {GetJoinedTypes()}";
    }
}

public class DescriptionPokemonTypeVO
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Description { get; set; }

    public override string ToString() => Description;
}

public class PokemonTypeEntityVO
{
    [System.Text.Json.Serialization.JsonPropertyName("type")]
    public DescriptionPokemonTypeVO DescriptionType { get; set; }

    public override string ToString() => DescriptionType.ToString();
}