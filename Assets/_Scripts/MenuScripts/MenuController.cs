using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/**
 * Script for managing the state of the various panels in the Menu Canvas.
**/

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject selectCharacterPanel;
    public GameObject howToPlayPanel;
    public GameObject settingsPanel;
    public GameObject coinPanel;
    public Button playGameButton;
    public Toggle goreToggle;
    public Slider sfxSlider;
    public Toggle sfxMuteToggle;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        selectCharacterPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);
        coinPanel.SetActive(false);
        Button playBtn = playGameButton.GetComponent<Button>();
        playBtn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
      Debug.Log("Play button pressed");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Test Scene");
    }

    public void SelectCharacter()
    {
        mainMenuPanel.SetActive(false);
        selectCharacterPanel.SetActive(true);
    }

    public void HowToPlay()
    {
        mainMenuPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

    public void Settings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ToggleGore()
    {
    }

    public void SFXOptions()
    {
        settingsPanel.SetActive(false);
    }

    public void AdjustSFXVolume()
    {
    }

    public void ToggleSFXMute()
    {

    }

    public void BuyCoins()
    {
        mainMenuPanel.SetActive(false);
        selectCharacterPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);
        coinPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        selectCharacterPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);
        coinPanel.SetActive(false);
    }
}
