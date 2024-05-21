using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI hpText;
    public Slider hpBar;

    public void Set(Pokemon pokemon)
    {
        nameText.text = pokemon.name;
        levelText.text = $":L{pokemon.level}";
        typeText.text = pokemon.type.ToString();
        hpText.text = $"{pokemon.currentHp}/{pokemon.maxHp}";
        hpBar.maxValue = pokemon.maxHp;
        hpBar.value = pokemon.currentHp;
    }
}
