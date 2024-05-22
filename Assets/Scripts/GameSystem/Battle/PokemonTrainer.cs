using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonTrainer : MonoBehaviour
{
    public new string name;
    public int level;
    public Sprite portrait;
    public List<Pokemon> ownPokemons = new List<Pokemon>();
    public TrainerData data;

    private void Start()
    {
        if(data != null)
        {
            this.name = data.name;
            this.portrait = data.portrait;
            GetComponent<SpriteRenderer>().sprite = data.sprite;
        }
    }

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
