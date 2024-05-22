using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonMonitor : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;

    public TextMeshProUGUI currentHpText;
    public TextMeshProUGUI maxHpText;

    public void Set(Pokemon pokemon)
    {
        nameText.text = pokemon.name;
        levelText.text = $":L{pokemon.level}";
        hpSlider.maxValue = pokemon.maxHp;
        hpSlider.value = pokemon.currentHp;

        if (currentHpText != null) currentHpText.text = pokemon.currentHp.ToString();
        if (maxHpText != null) maxHpText.text = pokemon.maxHp.ToString();
    }
}
