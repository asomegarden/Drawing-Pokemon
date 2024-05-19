using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeballMonitor : MonoBehaviour
{
    public Image[] pokeballImages;

    public Sprite aliveBallSprite;
    public Sprite dieBallSprite;
    public Sprite emptyBallSprite;

    public void Set(List<Pokemon> pokemons)
    {
        for(int i=0; i<pokeballImages.Length; i++)
        {
            if(i >= pokemons.Count)
            {
                pokeballImages[i].sprite = emptyBallSprite;
            }
            else
            {
                if (pokemons[i].currentHp > 0)
                {
                    pokeballImages[i].sprite = aliveBallSprite;
                }
                else
                {
                    pokeballImages[i].sprite = dieBallSprite;
                }
            }
        }
    }
}