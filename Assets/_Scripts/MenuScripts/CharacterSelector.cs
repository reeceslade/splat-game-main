using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

/**
 * Script for handling character selection logic.
 * @param characterSprite is the SpriteRenderer component of the character object's sprite.
 * @param characterDB is the CharacterDatabase component that is queried when selecting and buying characters.
 * @param coinDisplay is the CoinDisplay component that handles coin display logic.
 * @param pawPrints does the same for paw print display logic.
 * @param unlockedCharacters is a List object that is used to save, load and ensure that a character has been purchased before selecting.
 * @param selectedCharacteer is a global int variable that is used in most methods to reference an index of characterDB.
 */

public class CharacterSelector : MonoBehaviour
{
    public SpriteRenderer characterSprite;

    public CharacterDatabase characterDB;

    public CoinDisplay coinDisplay;
    public PawPrintsDisplay pawPrintsDisplay;

    public List<string> unlockedCharacters;

    public int selectedCharacter = 0;

    void Start()
    {
        GameObject button = GameObject.Find("PurchaseButton");
        if (characterDB.character[selectedCharacter].isUnlocked)
        {
            button.transform.localScale = new Vector3(0, 0, 0);
        }
        else { button.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f); }

        unlockedCharacters = SaveState.LoadUnlocks();

        if (unlockedCharacters == null)
        {
            unlockedCharacters = new List<string>();
        }

        foreach (Character item in characterDB.character)
        {
            foreach (string character in unlockedCharacters)
            {
                if (item.characterName == character)
                {
                    item.isUnlocked = true;
                }
            }
        }

        if (characterDB.character[0].isUnlocked)
        {
            GameObject characterCostDisplay = GameObject.Find("CharacterCostDisplay");
            characterCostDisplay.transform.localScale = new Vector3(0f, 0f, 0f);
        }


    }
    /**
     * Takes input from the left and right buttons in the character selection panel and scrolls through
     * the available characters.
     * 
     * @param isNext defines whether the input is intended to scroll to the next or previous character.
     */
    public void ScrollCharacter(bool isNext)
    {
        if (isNext)
        {
            selectedCharacter++;
            if (selectedCharacter >= characterDB.characterCount) { selectedCharacter = 0; }
        }
        else
        {
            selectedCharacter--;
            if (selectedCharacter < 0) { selectedCharacter = characterDB.characterCount - 1; }
        }
       
        UpdateCharacter(selectedCharacter);
        CheckUnlocked(selectedCharacter);
    }

    /**
     * Updates the character displayed on the character selection panel. Also configures the cost & type of the character.
     * 
     * @param selectedCharacter is the chosen index of characterDB.
     */
    public void UpdateCharacter(int selectedCharacter)
    {
        Character character = characterDB.GetCharacter(selectedCharacter);
        characterSprite.sprite = character.characterModel;
        GameObject characterCostDisplay = GameObject.Find("CharacterCostDisplay");
        if (!character.isUnlocked)
        {
            characterCostDisplay.transform.localScale = new Vector3(1f, 1f, 1f);
            TextMeshProUGUI characterCostText = GameObject.Find("CharacterCost").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI characterCostTitle = GameObject.Find("CharacterCostTitle").GetComponent<TextMeshProUGUI>();
            characterCostText.text = character.cost.ToString();

            if (character.isSpecialCharacter) characterCostTitle.text = $"Cost: (Paw Prints)";
            else characterCostTitle.text = $"Cost: (Coins)";
        }
        else
        {
            characterCostDisplay.transform.localScale = new Vector3(0f, 0f, 0f);
        }

    }

    /**
     * Selects a random character from characterDB and calls the UpdateCharacter method.
     */
    public void RandomCharacter()
    {
      int rand = Random.Range(0, characterDB.characterCount - 1);
      UpdateCharacter(rand);
    }

    /**
     * Ensures that the character is not already unlocked before calling HandlePurchase.
     */

    public void UnlockCharacter()
    {
        if (characterDB.character[selectedCharacter].isUnlocked) return;

        int characterCost = characterDB.character[selectedCharacter].cost;

        if (!characterDB.character[selectedCharacter].isSpecialCharacter)
        {
            HandlePurchase("coins", characterDB.character[selectedCharacter]);
        }
        else
        {
            HandlePurchase("paw_prints", characterDB.character[selectedCharacter]);
        }
    }   

    /**
     * Logic for selecting the unlocked/owned character. Ensures that the selected character has been unlocked before
     * setting the a playerprefs value with the selected character.
     */
    public void SelectCharacter()
    {
        Character character = characterDB.character[selectedCharacter];
        
        foreach (string item in unlockedCharacters)
        {
            if (character.characterName == item)
            {
                PlayerPrefs.SetString("selected_animal", character.characterName);
                Debug.Log("Selected character: " + character.characterName);
            }
            else Debug.Log("This character has not been unlocked.");
        }
        

    }

    /**
     * Handles the purchase logic in the character selection panel. Ensures that player's balance of coins/pawprints is
     * greater or equivalent to that of the character cost. Multiple methods are then called that handle the changed state
     * of both the player's currency amounts & the array of unlocked characters. 
     * 
     * A function of note - SaveUnlocks from SaveState is called when a purchase completes sucessfully. SaveUnlcoks saves the current
     * state of the unlockedCharacters list to a binary file.
     * 
     * @param currency is the type of currency that the player is buying the character with. (coins/pawprints)
     * @param character is the character object that is being purchased.
     */
    private void HandlePurchase(string currency, Character character)
    {
        int balance = PlayerPrefs.GetInt(currency);
        int cost = character.cost;

        if (balance >= cost)
        {
            character.isUnlocked = true;
            PlayerPrefs.SetInt(currency, balance - cost);
            CheckUnlocked(selectedCharacter);
            RefreshCurrencyDisplay(currency);
            unlockedCharacters.Add(character.characterName);
            SaveState.SaveUnlocks(unlockedCharacters);
        }
        else { Debug.Log("Not enough " + currency); }
    }

    /**
     * Checks if the selected chararacter has been unlocked and activates/deactivates the
     * purchase button accordingly.
     */

    public void CheckUnlocked(int selectedCharacter)
    {
        GameObject button = GameObject.Find("PurchaseButton");
        if (characterDB.character[selectedCharacter].isUnlocked)
        {
            button.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            button.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "Purchase";
        }
    }

    /**
     * Refreshes the displayed amount of a specific currency.
     * 
     * @param currency is the type of currency that has been chosen to be refreshed.
     */

    public void RefreshCurrencyDisplay(string currency)
    {
        if (currency == "coins")
        {
            coinDisplay = new CoinDisplay();
            coinDisplay.UpdateBalance();
        }
        else
        {
            pawPrintsDisplay = new PawPrintsDisplay();
            pawPrintsDisplay.UpdateBalance();
        }
    }


}
