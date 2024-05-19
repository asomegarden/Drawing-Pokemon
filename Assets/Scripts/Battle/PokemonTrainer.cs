using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonTrainer : MonoBehaviour
{
    public new string name;
    public Sprite portrait;
    public List<Pokemon> ownPokemons;

    public void TriggerBattle()
    {
        BattleManager.Instance.StartBattle(this);
    }
}
