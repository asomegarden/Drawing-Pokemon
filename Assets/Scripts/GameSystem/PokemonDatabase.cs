using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonDatabase : MonoBehaviour
{
    public static PokemonDatabase Instance {  get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }
}
