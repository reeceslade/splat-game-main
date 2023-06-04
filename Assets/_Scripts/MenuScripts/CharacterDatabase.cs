using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterDatabase : ScriptableObject
{
    public Character[] character;

    /**
     * Getter that returns the number of characters in the Character[] array.
     **/
    public int characterCount
    {
      get { return character.Length; }
    }

     /**
     * Getter that returns a specific character from the Character[] array.
     * @param i The chosen index of character[].
     **/
    public Character GetCharacter(int i)
    {
      return character[i];
    }
}
