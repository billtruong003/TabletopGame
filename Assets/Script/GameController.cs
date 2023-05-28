using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public DiceControllerUI DiceControll;
    public PlayerController PlayerController;
    public SpawnGrid SetupMap;
    public static bool canPlay;
    // Start is called before the first frame update
    void Start()
    {
        canPlay = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
