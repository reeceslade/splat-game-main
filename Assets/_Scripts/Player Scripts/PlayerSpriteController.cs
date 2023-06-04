using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private Sprite forwardFacing;
    private Sprite leftFacing;
    private Sprite rightFacing;
    private Sprite cameraFacing;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        /*if(getAnimalString() == "cat")
        {
            setAnimalString("dog");
        }
        else
        {
            setAnimalString("cat");
        }*/

        loadSprites();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = forwardFacing;
    }

    private void OnEnable()
    {
        GestureDetection.OnSwipeUp += setSprite;
        GestureDetection.OnSwipeDown += setSprite;
        GestureDetection.OnSwipeRight += setSprite;
        GestureDetection.OnSwipeLeft += setSprite;
        GestureDetection.OnTap += setSprite;
    }

    private void OnDisable()
    {
        GestureDetection.OnSwipeUp -= setSprite;
        GestureDetection.OnSwipeDown -= setSprite;
        GestureDetection.OnSwipeRight -= setSprite;
        GestureDetection.OnSwipeLeft -= setSprite;
        GestureDetection.OnTap -= setSprite;
    }

    private void setSprite(TouchData td)
    {
        if(td.direction == Vector2.up || td.direction == Vector2.zero)
        {
            spriteRenderer.sprite = forwardFacing;
        }
        else if (td.direction == Vector2.right)
        {
            spriteRenderer.sprite = rightFacing;
        }
        else if (td.direction == Vector2.down)
        {
            spriteRenderer.sprite = cameraFacing;
        }
        else if (td.direction == Vector2.left)
        {
            spriteRenderer.sprite = leftFacing;
        }
    }

    private void loadSprites()
    {
        string animal = getAnimalString();
        Debug.Log("Selected character: " + animal);
        Sprite[] animalSprites = Resources.LoadAll<Sprite>("Sprites/Game Sprites/Characters/" + animal);
        //Sprite[] animalSprites = getAnimalSpritesFromString(animal);
        //Debug.Log(animalSprites.Length);

        forwardFacing = getSpriteWithName("forward_facing", animalSprites);
        leftFacing = getSpriteWithName("left_facing", animalSprites);
        rightFacing = getSpriteWithName("right_facing", animalSprites);
        cameraFacing = getSpriteWithName("camera_facing", animalSprites);
    }

    /**
     * Checks if "selected_animal" exists in playerprefs key 
     * if it doesn't it makes it and sets it to "blackcat"
     */
    public static string getAnimalString()
    {
        if (!PlayerPrefs.HasKey("selected_animal"))
        {
            PlayerPrefs.SetString("selected_animal", "cat");
        }
        return PlayerPrefs.GetString("selected_animal");
    }

    public static void setAnimalString(string animal)
    {
        PlayerPrefs.SetString("selected_animal", animal);
    }

    private Sprite getSpriteWithName(string name, Sprite[] sprites)
    {
        foreach (var sprite in sprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        return null;
    }
}
