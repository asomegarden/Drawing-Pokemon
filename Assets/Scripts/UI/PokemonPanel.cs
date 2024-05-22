using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonPanel : MonoBehaviour
{
    public PokemonItem[] pokemonItems;

    public void Set(PokemonTrainer trainer)
    {
        List<Pokemon> pokemons = trainer.ownPokemons;

        for (int i = 0; i < pokemonItems.Length; i++)
        {
            if (i < pokemons.Count)
            {
                pokemonItems[i].gameObject.SetActive(true);
                pokemonItems[i].Set(pokemons[i]);
            }
            else
            {
                pokemonItems[i].gameObject.SetActive(false);
            }
        }
    }
}
