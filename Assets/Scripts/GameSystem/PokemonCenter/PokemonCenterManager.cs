using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonCenterManager : MonoBehaviour
{
    public static PokemonCenterManager Instance {  get; private set; }

    private PokemonTrainer playerTrainer;

    private void Awake()
    {
        if(Instance == null)Instance = this;
    }

    private void Start()
    {
        playerTrainer = PlayerController.Instance.trainer;
    }

    public void HealPlayerOwnAllPokemons()
    {
        StartCoroutine(HealPlayerOwnAllPokemonsCoroutine());
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
