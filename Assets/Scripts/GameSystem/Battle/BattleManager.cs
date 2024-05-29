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

    public Pokemon enemyPokemon;
    public Pokemon playerPokemon;

    public Image enemyPortraitImage;
    public Image playerPortraitImage;

    public Image enemyPokemonPortraitImage;
    public Image playerPokemonPortraitImage;

    public PokeballMonitor enemyPokeballMonitor;
    public PokeballMonitor playerPokeballMonitor;

    public PokemonMonitor enemyPokemonMonitor;
    public PokemonMonitor playerPokemonMonitor;

    public TextMeshProUGUI dialogueText;

    private int selectedActionIndex = 0;
    public GameObject actionSelectPanel;
    public GameObject[] actionSelectCursors;

    public PokemonSelectPanel pokemonSelectPanel;

    public bool isBattleLoop;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void StartBattle(PokemonTrainer enemy)
    {
        player = PlayerManager.Instance.playerTrainerInfo;
        if(player.AutoSentOutPokemon() == null)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.sentences = new List<string>()
            {
                "뭐야, 싸울 수 있는 포켓몬부터 들고와!"
            };
            DialogueManager.Instance.StartDialogue(dialogue);
            
        }else if (enemy.AutoSentOutPokemon() == null)
        {
            Dialogue dialogue = new Dialogue();
            dialogue.sentences = new List<string>()
            {
                "앗 미안 지금은 싸울 수 있는 포켓몬이 없어"
            };
            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            PlayerController.Instance.DisableInput();

            this.enemy = enemy;
            battleScreenPanel.SetActive(true);

            StartCoroutine(BattleLoop());
        }
    }

    public IEnumerator BattleLoop()
    {
        // Init
        InputIndicator.Instance.HideAllIndicator();
        isBattleLoop = true;

        dialogueText.text = "";

        enemyPokeballMonitor.gameObject.SetActive(false);
        playerPokeballMonitor.gameObject.SetActive(false);

        enemyPortraitImage.gameObject.SetActive(false);
        playerPortraitImage.gameObject.SetActive(false);

        enemyPokemonPortraitImage.gameObject.SetActive(false);
        playerPokemonPortraitImage.gameObject.SetActive(false);

        enemyPokemonMonitor.gameObject.SetActive(false);
        playerPokemonMonitor.gameObject.SetActive(false);

        pokemonSelectPanel.gameObject.SetActive(false);
        actionSelectPanel.SetActive(false);

        //Setup
        yield return StartCoroutine(SetupBattle());

        yield return StartCoroutine(EnemySentOut(enemy.AutoSentOutPokemon()));
        yield return StartCoroutine(PlayerSentOut(player.AutoSentOutPokemon()));

        while (isBattleLoop)
        {
            yield return StartCoroutine(PlayerAction());
            if (isBattleLoop) yield return StartCoroutine(EnemyAction());
        }

        EndBattle();
    }

    private IEnumerator EnemyAction()
    {
        if (enemyPokemon.CurrentHp <= 0)
        {
            dialogueText.text = $"{enemyPokemon.name}(이)가 쓰러졌다!";
            yield return new WaitForSeconds(0.5f);
            enemyPokemonPortraitImage.gameObject.SetActive(false);
            enemyPokemonMonitor.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            enemyPortraitImage.gameObject.SetActive(true);
            enemyPokeballMonitor.gameObject.SetActive(true);
            enemyPokeballMonitor.Set(enemy.ownPokemons);

            yield return new WaitForSeconds(1f);

            enemyPokemon = enemy.AutoSentOutPokemon();

            if (enemyPokemon == null)
            {
                yield return StartCoroutine(DefeatEnemy());
            }
            else
            {
                yield return StartCoroutine(EnemySentOut(enemy.AutoSentOutPokemon()));
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(Attack(enemyPokemon, playerPokemon, playerPokemonMonitor));

            if (playerPokemon.CurrentHp <= 0)
            {
                dialogueText.text = $"{playerPokemon.name}(이)가 쓰러졌다!";
                playerPokemonPortraitImage.gameObject.SetActive(false);
                playerPokemonMonitor.gameObject.SetActive(false);

                yield return new WaitForSeconds(0.5f);

                playerPortraitImage.gameObject.SetActive(true);
                playerPokeballMonitor.gameObject.SetActive(true);
                playerPokeballMonitor.Set(player.ownPokemons);

                if (player.ownPokemons.Find(p => p.CurrentHp > 0) == null)
                {
                    yield return StartCoroutine(DefeatPlayer());
                }
                else
                {
                    yield return StartCoroutine(SelectPokemon());
                }
            }
        }
    }

    public IEnumerator SetupBattle()
    {
        dialogueText.text = $"{enemy.name}(이)가 승부를 걸어왔다!";

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

        dialogueText.text = "";
    }

    public IEnumerator EnemySentOut(Pokemon pokemon)
    {
        enemyPortraitImage.gameObject.SetActive(false);
        enemyPokeballMonitor.gameObject.SetActive(false);

        enemyPokemon = pokemon;
        dialogueText.text = $"{enemy.name}은(는) {pokemon.name}을(를) 내보냈다!";

        yield return new WaitForSeconds(1f);

        enemyPokemonPortraitImage.gameObject.SetActive(true);
        enemyPokemonPortraitImage.sprite = pokemon.frontPortrait;

        yield return new WaitForSeconds(1f);

        enemyPokemonMonitor.gameObject.SetActive(true);
        enemyPokemonMonitor.Set(pokemon);

        yield return new WaitForSeconds(1f);

        dialogueText.text = "";
    }

    public IEnumerator PlayerSentOut(Pokemon pokemon)
    {
        playerPokemon = pokemon;
        playerPortraitImage.gameObject.SetActive(false);
        playerPokemonPortraitImage.gameObject.SetActive(false);
        playerPokemonMonitor.gameObject.SetActive(false);
        playerPokeballMonitor.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        dialogueText.text = $"가라! {pokemon.name}!";

        yield return new WaitForSeconds(1f);

        playerPokemonPortraitImage.gameObject.SetActive(true);
        playerPokemonPortraitImage.sprite = pokemon.behindPortrait;

        yield return new WaitForSeconds(1f);

        playerPokemonMonitor.gameObject.SetActive(true);
        playerPokemonMonitor.Set(pokemon);

        dialogueText.text = "";
    }

    public IEnumerator ActionSelect()
    {
        actionSelectPanel.SetActive(true);

        for(int i=0; i<actionSelectCursors.Length; i++)
        {
            if (i == selectedActionIndex) actionSelectCursors[i].SetActive(true);
            else actionSelectCursors[i].SetActive(false);
        }

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "선택"));

        while (!Input.GetKeyDown(KeyCode.E))
        {
            float input = Input.GetAxisRaw("Horizontal");
            if (input < 0)
            {
                if(--selectedActionIndex < 0) selectedActionIndex = actionSelectCursors.Length - 1;

                for (int i = 0; i < actionSelectCursors.Length; i++)
                {
                    if (i == selectedActionIndex) actionSelectCursors[i].SetActive(true);
                    else actionSelectCursors[i].SetActive(false);
                }

                yield return new WaitForSeconds(0.5f);
            }
            else if (input > 0)
            {
                if(++selectedActionIndex == actionSelectCursors.Length) selectedActionIndex = 0;

                for (int i = 0; i < actionSelectCursors.Length; i++)
                {
                    if (i == selectedActionIndex) actionSelectCursors[i].SetActive(true);
                    else actionSelectCursors[i].SetActive(false);
                }

                yield return new WaitForSeconds(0.25f);
            }

            yield return null;
        }

        InputIndicator.Instance.HideAllIndicator();
        actionSelectPanel.SetActive(false);
    }

    private IEnumerator PlayerAction()
    {
        yield return StartCoroutine(ActionSelect());

        if (selectedActionIndex == 0)
        {
            yield return StartCoroutine(Attack(playerPokemon, enemyPokemon, enemyPokemonMonitor));
        }
        else if (selectedActionIndex == 1)
        {
            yield return StartCoroutine(Runaway());
        }
        else if (selectedActionIndex == 2)
        {
            yield return StartCoroutine(SelectPokemon());
        }
    }

    private IEnumerator Attack(Pokemon attacker, Pokemon target, PokemonMonitor targetMonitor)
    {
        dialogueText.text = $"{attacker.name}이(가) 공격했다!";

        yield return new WaitForSeconds(0.5f);
        int damage = attacker.power;
        float coef = attacker.GetDamageCoefficient(target.type);

        damage = (int)(damage * coef);
        target.CurrentHp -= damage;
        targetMonitor.Set(target);

        yield return new WaitForSeconds(0.5f);

        dialogueText.text = $"{target.name}이(가) {damage} 피해를 입었다!";

        yield return new WaitForSeconds(0.5f);

        if (coef > 1.1f)
        {
            dialogueText.text = "효과는 굉장했다!";
            yield return new WaitForSeconds(0.5f);
        }
        else if(coef < 0.9f)
        {
            dialogueText.text = "효과는 미미했다..";
            yield return new WaitForSeconds(0.5f);
        }

        dialogueText.text = "";
    }

    private IEnumerator SelectPokemon()
    {
        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "선택"));

        pokemonSelectPanel.gameObject.SetActive(true);
        dialogueText.text = "";
        actionSelectPanel.gameObject.SetActive(false);

        do
        {
            pokemonSelectPanel.Set(player);
            yield return new WaitUntil(() => pokemonSelectPanel.isSelected == true);

        } while (pokemonSelectPanel.selectedPokemon.CurrentHp <= 0);

        pokemonSelectPanel.gameObject.SetActive(false);

        InputIndicator.Instance.HideAllIndicator();

        yield return PlayerSentOut(pokemonSelectPanel.selectedPokemon);
    }

    private void EndBattle()
    {
        battleScreenPanel.SetActive(false);
        PlayerController.Instance.EnableInput();
        InputIndicator.Instance.ShowIndicator();
    }

    private IEnumerator DefeatEnemy()
    {
        dialogueText.text = "말도 안돼! 내가 지다니!";

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "종료"));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) == true);

        isBattleLoop = false;
    }

    private IEnumerator DefeatPlayer()
    {
        dialogueText.text = $"{enemy.name}에게 지고말았다..";

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "종료"));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) == true);

        isBattleLoop = false;
    }

    private IEnumerator Runaway()
    {
        yield return new WaitForSeconds(0.25f);
        dialogueText.text = $"{enemy.name}에게서 도망쳤다.";
        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "종료"));

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E) == true);

        isBattleLoop = false;
    }
}
