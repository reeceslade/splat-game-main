using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


/**
 * Serializable character object that stores mulitple values.
 * 
 * @value characterName is the name of the character.
 * @value characterModel is the sprite of the character.
 * @value isSpecialCharacter is a boolean value that dictates which currency the character should be purchased with.
 * @value cost is the cost of the character.
 * @value isUnlocked is a bool value that dictates whether the player has unlocked the character.
 */
public class Character
{
    public string characterName;
    public Sprite characterModel;
    public bool isSpecialCharacter;
    public int cost;
    public bool isUnlocked;
    public int id;
}
