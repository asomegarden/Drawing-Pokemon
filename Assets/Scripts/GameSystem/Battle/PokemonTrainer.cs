using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonTrainer : MonoBehaviour
{
    public new string name;
    public Sprite portrait;
    public List<Pokemon> ownPokemons = new List<Pokemon>();

    public void TriggerBattle()
    {
        BattleManager.Instance.StartBattle(this);
    }

    public Pokemon AutoSentOutPokemon()
    {
        Pokemon pokemon = null;
        for(int i=0; i<ownPokemons.Count; i++)
        {
            if (ownPokemons[i].currentHp > 0)
            {
                pokemon = ownPokemons[i];
                break;
            }
        }

        return pokemon;
    }
}
