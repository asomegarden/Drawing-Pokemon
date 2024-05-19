using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public GameObject battleScreenPanel;
    public PokemonTrainer enemy;
    public PokemonTrainer player;

    public Image enemyPortraitImage;
    public Image playerPortraitImage;

    public Image enemyPokemonPortraitImage;
    public Image playerPokemonPortraitImage;

    public PokeballMonitor enemyPokeballMonitor;
    public PokeballMonitor playerPokeballMonitor;

    public PokemonMonitor enemyPokemonMonitor;
    public PokemonMonitor playerPokemonMonitor;

    public TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartBattle(PokemonTrainer enemy)
    {
        this.enemy = enemy;
        battleScreenPanel.SetActive(true);

        StartCoroutine(BattleLoop());
    }

    public IEnumerator BattleLoop()
    {
        // Init
        dialogueText.text = "";

        enemyPokeballMonitor.gameObject.SetActive(false);
        playerPokeballMonitor.gameObject.SetActive(false);

        enemyPortraitImage.gameObject.SetActive(false);
        playerPortraitImage.gameObject.SetActive(false);

        enemyPokemonPortraitImage.gameObject.SetActive(false);
        playerPokemonPortraitImage.gameObject.SetActive(false);

        enemyPokemonMonitor.gameObject.SetActive(false);
        playerPokemonMonitor.gameObject.SetActive(false);

        //Setup
        yield return StartCoroutine(SetupBattle());

        yield return StartCoroutine(EnemySentOut(enemy.ownPokemons[0]));
        yield return StartCoroutine(PlayerSentOut(player.ownPokemons[0]));
    }

    public IEnumerator SetupBattle()
    {
        dialogueText.text = $"포켓몬 트레이너 {enemy.name}(이)가 승부를 걸어왔다!";

        yield return new WaitForSeconds(1f);

        enemyPortraitImage.gameObject.SetActive(true);
        enemyPortraitImage.sprite = enemy.portrait;

        playerPortraitImage.gameObject.SetActive(true);
        playerPortraitImage.sprite = player.portrait;

        yield return new WaitForSeconds(1f);

        enemyPokeballMonitor.gameObject.SetActive(true);
        enemyPokeballMonitor.Set(enemy.ownPokemons);

        playerPokeballMonitor.gameObject.SetActive(true);
        playerPokeballMonitor.Set(player.ownPokemons);

        yield return new WaitForSeconds(1f);

        dialogueText.text = "";
        enemyPortraitImage.gameObject.SetActive(false);
        enemyPokeballMonitor.gameObject.SetActive(false);
        playerPokeballMonitor.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator EnemySentOut(Pokemon pokemon)
    {
        dialogueText.text = $"{enemy.name}는 {pokemon.name}을 내보냈다!";

        yield return new WaitForSeconds(1f);

        enemyPokemonPortraitImage.gameObject.SetActive(true);
        enemyPokemonPortraitImage.sprite = pokemon.portrait;

        yield return new WaitForSeconds(1f);

        enemyPokemonMonitor.gameObject.SetActive(true);
        enemyPokemonMonitor.Set(pokemon);

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayerSentOut(Pokemon pokemon)
    {
        playerPortraitImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        dialogueText.text = $"가라! {pokemon.name}!";

        yield return new WaitForSeconds(1f);

        playerPokemonPortraitImage.gameObject.SetActive(true);
        playerPokemonPortraitImage.sprite = pokemon.portrait;

        yield return new WaitForSeconds(1f);

        playerPokemonMonitor.gameObject.SetActive(true);
        playerPokemonMonitor.Set(pokemon);
    }
}
