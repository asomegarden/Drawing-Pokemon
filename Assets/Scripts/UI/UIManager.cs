using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    public MenuPanel menuPanel;
    public PokemonPanel pokemonPanel;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.gameObject.activeSelf)
            {
                HideMenuPanel();
            }
            else if (pokemonPanel.gameObject.activeSelf)
            {
                HidePokemonPanel();
            }
            else
            {
                if (PlayerController.Instance.InputEnabled)
                {
                    ShowMenuPanel();
                }
            }
        }
    }

    public void ShowMenuPanel()
    {
        PlayerController.Instance.DisableInput();

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.E, "����"), new ActionGuide(KeyCode.Escape, "�ݱ�"));

        menuPanel.gameObject.SetActive(true);
        menuPanel.Show();
    }

    public void HideMenuPanel()
    {
        PlayerController.Instance.EnableInput();
        InputIndicator.Instance.ShowIndicator();

        menuPanel.gameObject.SetActive(false);
    }

    public void ShowPokemonPanel()
    {
        HideMenuPanel();

        PlayerController.Instance.DisableInput();

        InputIndicator.Instance.ShowIndicatorNoDefault(new ActionGuide(KeyCode.Escape, "�ݱ�"));

        pokemonPanel.gameObject.SetActive(true);
        pokemonPanel.Set(PlayerManager.Instance.playerTrainerInfo);
    }

    public void HidePokemonPanel()
    {
        PlayerController.Instance.EnableInput();
        InputIndicator.Instance.ShowIndicator();

        pokemonPanel.gameObject.SetActive(false);
    }
}
