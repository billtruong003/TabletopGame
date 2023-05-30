using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public DiceControllerUI DiceControll;
    public PlayerController PlayerController;
    public SpawnGrid SetupMap;
    public static bool canPlay;
    public static int numberOfPlayers;
    public static bool diceReady;
    // Start is called before the first frame update
    void Start()
    {
        canPlay = false;
        numberOfPlayers = 2;
        diceReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
