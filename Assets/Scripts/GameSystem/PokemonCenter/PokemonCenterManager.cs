using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PokemonCenterManager : MonoBehaviour
{
    public static PokemonCenterManager Instance {  get; private set; }

    public PokemonTrainer playerTrainer;
    public GameObject pokemonRemoveScreen;
    public PokemonSelectPanel pokemonSelectPanel;

    private void Awake()
    {
        if(Instance == null)Instance = this;
    }

    public void GetRandomPokemon()
    {
        StartCoroutine(GetRandomPokemonCoroutine());
    }

    private IEnumerator GetRandomPokemonCoroutine()
    {
        string sentence = "";
        for (int i = 3; i > 0; i--)
        {
            sentence += $"{i}..";
            DialogueManager.Instance.ShowForceDialogue(sentence);
            yield return new WaitForSeconds(1f);
        }

        if(playerTrainer.ownPokemons.Count == 6)
        {
            DialogueManager.Instance.ShowForceDialogue("오류: 보유한 포켓몬이 너무 많습니다.");
        }
        else
        {
            Pokemon newPokemon = PokemonManager.Instance.GetRandomPokemon();
            playerTrainer.ownPokemons.Add(newPokemon);
            DialogueManager.Instance.ShowForceDialogue($"완료: Lv.{newPokemon.level} {newPokemon.name}을(를) 획득했습니다!");
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        DialogueManager.Instance.HideForceDialogue();
    }

    public void HealPlayerOwnAllPokemons()
    {
        StartCoroutine(HealPlayerOwnAllPokemonsCoroutine());
    }

    public void StartPokemonRemove()
    {
        StartCoroutine(PokemonRemoveCoroutine());
    }

    private IEnumerator PokemonRemoveCoroutine()
    {
        PlayerController.Instance.DisableInput();

        pokemonRemoveScreen.SetActive(true);
        pokemonSelectPanel.Set(playerTrainer);

        while(!Input.GetKeyDown(KeyCode.Escape))
        {
            if (pokemonSelectPanel.isSelected)
            {
                if (pokemonSelectPanel.selectedPokemon == null) break;
                playerTrainer.ownPokemons.Remove(pokemonSelectPanel.selectedPokemon);
                pokemonSelectPanel.Set(playerTrainer);
            }
            yield return null;
        }

        pokemonRemoveScreen.SetActive(false);

        PlayerController.Instance.EnableInput();
    }

    private IEnumerator HealPlayerOwnAllPokemonsCoroutine()
    {
        playerTrainer.ownPokemons.ForEach(p =>
        {
            p.currentHp = p.maxHp;
        });

        string sentence = "";
        for(int i = 3; i>0; i--)
        {
            sentence += $"{i}..";
            DialogueManager.Instance.ShowForceDialogue(sentence);
            yield return new WaitForSeconds(1f);
        }

        DialogueManager.Instance.ShowForceDialogue("치료가 완료되었습니다!");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        DialogueManager.Instance.HideForceDialogue();
    }
}
