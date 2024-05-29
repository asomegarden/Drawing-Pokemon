using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingComputer : MonoBehaviour
{
    public void DrawPokemon()
    {
        PokemonCenterManager.Instance.DrawPokemon();
    }
}
