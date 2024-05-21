using System.Collections.Generic;
using UnityEngine;

public class PokemonManager : MonoBehaviour
{
    public static PokemonManager Instance { get; private set; }

    public List<PokemonData> pokemonDatas;
    public List<Pokemon> pokemons = new List<Pokemon>();

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    void Start()
    {
        InitializePokemons();
    }

    void InitializePokemons()
    {
        foreach (var data in pokemonDatas)
        {
            Pokemon pokemon = new Pokemon(data);
            pokemons.Add(pokemon);
        }
    }

    public Pokemon GetRandomPokemon()
    {
        Pokemon newPokemon = new Pokemon(pokemons[Random.Range(0, pokemons.Count)]);

        newPokemon.level = Random.Range(1, 101);
        newPokemon.maxHp = newPokemon.level * 10 + Random.Range(-20, 21);
        if(newPokemon.maxHp < 5) newPokemon.maxHp = 5;
        newPokemon.currentHp = newPokemon.maxHp;

        newPokemon.power = newPokemon.level * 5 + Random.Range(-10, 11);
        if (newPokemon.power < 5) newPokemon.power = 5;

        return newPokemon;
    }
}
