using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveState { 


    /**
     * Saves unlocked characters to a binary file.
     * 
     * @param unlockedCharacters is a list object of all the characters the player has unlocked.
     */
    public static void SaveUnlocks(List<string> unlockedCharacters)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "unlockedcharacters.binary";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, unlockedCharacters);
        stream.Close();
    }

     /**
     * Loads unlocked characters from the binary file to the unlockedCharacters array.
     * 
     */
    public static List<string> LoadUnlocks()
    {
        string filePath = Application.persistentDataPath + "unlockedcharacters.binary";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            List<string> data = formatter.Deserialize(stream) as List<string>;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("File not found.");
            return null;
        }
    }

}
