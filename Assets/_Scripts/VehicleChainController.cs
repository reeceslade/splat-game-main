using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum Direction { left = 0, right = 1}
/**
 * This script will be a component on all instances of the vehicle chain prefab
 * It is responsible for determining how many vehicles will be in the chain, positioning them so
 * they are randomly spaced apart and then determing the direction of the chain and positioning the whole chain so it starts off camera.
 * 
 */
public class VehicleChainController : MonoBehaviour
{
    private static int minVehicles = 1;
    private static int maxVehicles = 5;
    private static int vehicleWidth = 4;

    [SerializeField] private GameObject dummyVehiclePrefab;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isPaused = false;
    private float inactiveDelayTimestamp = 0f;
    private float speed;

    private void OnEnable()
    {
        GameStateController.OnResumeGame += toggleGamePaused;
        GameStateController.OnPauseGame += toggleGamePaused;
    }

    private void OnDisable()
    {
        GameStateController.OnResumeGame -= toggleGamePaused;
        GameStateController.OnPauseGame -= toggleGamePaused;
    }

    private void Start()
    {
        initialiseChain();
    }

    private void Update()
    {
        if(!isPaused)
        {
            if (Vector2.Distance(transform.position, endPosition) > 0.5f && Time.timeSinceLevelLoad > inactiveDelayTimestamp)
            {
                transform.position = Vector2.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            }
            else
            {
                transform.position = startPosition;
                inactiveDelayTimestamp = Time.timeSinceLevelLoad + Random.Range(0f, 5f);
            }
        }
    }

    public void initialiseChain()
    {
        //set number of vehicles (random)
        int numOfVehicles = Random.Range(minVehicles, maxVehicles);
        //set direction (random)
        Direction direction = getRandDirection();
        //generate dummy vehicles
        float[] offsets = genListOfIncreasingOffsets(numOfVehicles);
        for (int i = 0; i < numOfVehicles; i++)
        {
            Vector2 pos = new Vector2(transform.position.x + offsets[i], transform.position.y);
            if(direction == Direction.right)
            {
                pos = new Vector2(transform.position.x - offsets[i], transform.position.y);
            }
            //generate dummy vehicle
            GameObject dummyVehicle = Instantiate(dummyVehiclePrefab, pos, Quaternion.identity);
            if(direction == Direction.right)
            {
                dummyVehicle.GetComponent<SpriteRenderer>().flipX = true;
            }
            dummyVehicle.transform.parent = transform;
        }
        //set beginning and end position (so that the chain begins and ends up off camera)
        float chainLength = totalFloatArray(offsets) + vehicleWidth / 2f;
        float leftMostPosition = -6f;
        float rightMostPosition = 6f;

        if(direction == Direction.right)
        {
            startPosition = new Vector2(leftMostPosition, transform.position.y);
            endPosition = new Vector2(rightMostPosition + chainLength, transform.position.y);
        }
        else
        {
            startPosition = new Vector2(rightMostPosition, transform.position.y);
            endPosition = new Vector2(leftMostPosition - chainLength, transform.position.y);
        }

        transform.position = startPosition;
        //set inactive delay
        inactiveDelayTimestamp = Random.Range(0f, 5f);
        //set speed
        speed = Random.Range(5f, 8f);
    }

    public Direction getRandDirection()
    {
        return (Direction)Random.Range(0, 2);
    }

    private void toggleGamePaused(GameStateController.GameState gameState)
    {
        if (gameState == GameStateController.GameState.Paused)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }

    /**
     * This method returns a list of floats that increase in value at random increments of vehicleWidth + random value between 0.5 and 9
     */
    private float[] genListOfIncreasingOffsets(int length)
    {

        float[] offsets = new float[length];
        float offset = vehicleWidth / 2f;
        for (int i = 0; i < length; i++)
        {
            if (i == 0)
            {
                offsets[i] = offset;
            }
            else
            {                
                float increase = Random.Range(0, 18);
                increase *= 0.5f;
                increase += vehicleWidth;
                offset += increase;
                offsets[i] = offset;
            }
        }
        return offsets;
    }

    private float totalFloatArray(float[] arr)
    {
        float total = 0f;
        foreach (var item in arr)
        {
            total += item;
        }
        return total;
    }
}
