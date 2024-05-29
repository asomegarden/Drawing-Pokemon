using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PokemonCenterManager : MonoBehaviour
{
    public static PokemonCenterManager Instance {  get; private set; }

    public GameObject pokemonRemoveScreen;
    public PokemonSelectPanel pokemonSelectPanel;

    public float scoreThreshold = 0.4f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void DrawPokemon()
    {
        StartCoroutine(DrawPokemonCoroutine());
    }

    private IEnumerator DrawPokemonCoroutine()
    {
        PokemonTrainer playerTrainer = PlayerManager.Instance.playerTrainerInfo;
        InputIndicator.Instance.HideAllIndicator();

        if(playerTrainer.ownPokemons.Count == 6)
        {
            DialogueManager.Instance.ShowForceDialogue("����: ������ ���ϸ��� �ʹ� �����ϴ�.");
        }
        else
        {
            PainterManager.Instance.EnablePainter();
            Painter painter = PainterManager.Instance.painter;
            yield return new WaitUntil(() => painter.isActive == false);

            Texture2D pokemonImage = painter.resultImage;

            PainterManager.Instance.DisablePainter();
            InputIndicator.Instance.HideAllIndicator();
            DialogueManager.Instance.ShowForceDialogue("�̹��� �м���");

            byte[] imageBytes = pokemonImage.EncodeToPNG();
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", imageBytes, "image.png", "image/png");

            using (UnityWebRequest www = UnityWebRequest.Post("http://39.124.11.92:50000/which_pokemon", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"���� ��û ����: {www.error}");
                }
                else
                {
                    string jsonResponse = www.downloadHandler.text;
                    var result = JsonUtility.FromJson<PokemonResponse>(jsonResponse);
                    
                    if(scoreThreshold < result.score)
                    {
                        float score = result.score - scoreThreshold;
                        score /= (1f - scoreThreshold);
                        int level = (int)(score * 100);
                        Pokemon pokemon = PokemonManager.Instance.GetPokemon(result.pokemon, level);
                        playerTrainer.ownPokemons.Add(pokemon);

                        DialogueManager.Instance.ShowForceDialogue($"Lv.{pokemon.level} {pokemon.name}��(��) ȹ���߽��ϴ�!");
                    }
                    else
                    {
                        DialogueManager.Instance.ShowForceDialogue("�׸��� �ĺ��� �� �����ϴ�..");
                    }
                }
            }
        }

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "Ȯ��"));
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
        InputIndicator.Instance.HideAllIndicator();
        PokemonTrainer playerTrainer = PlayerManager.Instance.playerTrainerInfo;

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
        InputIndicator.Instance.HideAllIndicator();
        PlayerManager.Instance.playerTrainerInfo.ownPokemons.ForEach(p =>
        {
            p.CurrentHp = p.maxHp;
        });

        string sentence = "";
        for(int i = 3; i>0; i--)
        {
            sentence += $"{i}..";
            DialogueManager.Instance.ShowForceDialogue(sentence);
            yield return new WaitForSeconds(1f);
        }

        DialogueManager.Instance.ShowForceDialogue("ġ�ᰡ �Ϸ�Ǿ����ϴ�!");

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "Ȯ��"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        DialogueManager.Instance.HideForceDialogue();
    }
}

[Serializable]
public class PokemonResponse
{
    public string pokemon;
    public float score;
}