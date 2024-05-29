using System.Collections.Generic;
using UnityEngine;

public class PokemonManager : MonoBehaviour
{
    public static PokemonManager Instance { get; private set; }

    public List<PokemonData> pokemonDatas;
    public List<Pokemon> pokemons = new List<Pokemon>();

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

        newPokemon.level = level;
        newPokemon.maxHp = newPokemon.level * 10 + Random.Range(-10, 11);
        if (newPokemon.maxHp < 10) newPokemon.maxHp = 10;
        newPokemon.CurrentHp = newPokemon.maxHp;

        newPokemon.power = (int)(level * 2.5) + Random.Range(-2, 3);
        if (newPokemon.power < 5) newPokemon.power = 5;

        return newPokemon;
    }
}
