using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSelectPanel : MonoBehaviour
{
    public PokemonItem[] pokemonItems;
    public GameObject[] cursors;

    private int selectedIndex;
    private List<Pokemon> pokemons;

    public Pokemon selectedPokemon;
    public bool isSelected = false;

    public void Set(PokemonTrainer trainer)
    {
        pokemons = trainer.ownPokemons;

        if(pokemons.Count == 0)
        {
            isSelected = true;
            selectedPokemon = null;
            return;
        }
        isSelected = false;
        selectedIndex = 0;

        for(int i=0; i<pokemonItems.Length; i++)
        {
            if(i < pokemons.Count)
            {
                pokemonItems[i].gameObject.SetActive(true);
                pokemonItems[i].Set(pokemons[i]);
            }
            else
            {
                pokemonItems[i].gameObject.SetActive(false);
            }
        }
        UpdateCursor();
        StartCoroutine(SelectCoroutine());
    }

    private IEnumerator SelectCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        while (!Input.GetKeyDown(KeyCode.E))
        {
            float input = Input.GetAxisRaw("Vertical");
            if (input > 0)
            {
                if (--selectedIndex < 0) selectedIndex = pokemons.Count - 1;
                UpdateCursor();
                yield return new WaitForSeconds(0.5f);
            }
            else if (input < 0)
            {
                if (++selectedIndex == pokemons.Count) selectedIndex = 0;
                UpdateCursor();
                yield return new WaitForSeconds(0.25f);
            }

            yield return null;
        }

        selectedPokemon = pokemons[selectedIndex];

        isSelected = true;
    }

    private void UpdateCursor()
    {

        for (int i = 0; i < cursors.Length; i++)
        {
            if (i == selectedIndex) cursors[i].SetActive(true);
            else cursors[i].SetActive(false);
        }
    }
}
