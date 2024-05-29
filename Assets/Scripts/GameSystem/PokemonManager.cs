using System.Collections.Generic;
using UnityEngine;

public class PokemonManager : MonoBehaviour
{
    public static PokemonManager Instance { get; private set; }

    public List<PokemonData> pokemonDatas;
    public List<Pokemon> pokemons = new List<Pokemon>();

    public Dictionary<string, Pokemon> pokemonDict = new Dictionary<string, Pokemon>();
    public List<PokemonTrainer> trainers = new List<PokemonTrainer>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializePokemons();
        InitializeTrainers();
    }

    void InitializePokemons()
    {
        foreach (var data in pokemonDatas)
        {
            Pokemon pokemon = new Pokemon(data);
            pokemons.Add(pokemon);
            pokemonDict.Add(pokemon.id, pokemon);
        }
    }

    void InitializeTrainers()
    {
        foreach (var trainer in trainers)
        {
            int pokemonCount = Random.Range(1, 4);
            for(int i=0; i< pokemonCount; i++)
            {
                int level = trainer.level + Random.Range(-5, 6);
                if (level < 1) level = 1;
                else if (level > 100) level = 100;

                trainer.ownPokemons.Add(GetRandomPokemon(level));
            }
        }
    }

    public Pokemon GetRandomPokemon()
    {
        int level = Random.Range(1, 101);
        return GetRandomPokemon(level);
    }

    public Pokemon GetRandomPokemon(int level)
    {
        Pokemon newPokemon = new Pokemon(pokemons[Random.Range(0, pokemons.Count)]);

        return GetPokemon(newPokemon, level);
    }

    public Pokemon GetPokemon(string id, int level)
    {
        return GetPokemon(pokemonDict[id], level);
    }

    public Pokemon GetPokemon(Pokemon pokemon, int level)
    {
        pokemon.level = level;
        pokemon.maxHp = pokemon.level * 10 + Random.Range(-10, 11);
        if (pokemon.maxHp < 10) pokemon.maxHp = 10;
        pokemon.CurrentHp = pokemon.maxHp;

        pokemon.power = (int)(level * 2.5) + Random.Range(-2, 3);
        if (pokemon.power < 5) pokemon.power = 5;

        return pokemon;
    }
}
