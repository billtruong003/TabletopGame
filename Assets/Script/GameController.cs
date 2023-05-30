using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Quản lý script")]
    public DiceControllerUI DiceControll;
    public PlayerController PlayerController;
    public SpawnGrid SetupMap;
    [Header("Quản lý button")]
    public GameObject card;
    public static bool DrawCard;
    
    [Header("Quản lý text")]
    [SerializeField] TextMeshProUGUI Playershow;
    public static bool canPlay;
    public static int numberOfPlayers;
    public static bool diceReady;
    public static bool Win;
    // Start is called before the first frame update
    void Start()
    {
        canPlay = false;
        numberOfPlayers = 2;
        diceReady = true;
        PlayerController.SpawnPlayers();
        DrawCard = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!DrawCard)
        {
            card.SetActive(true);
        }
    }
    public void appearText(){
        Playershow.text = string.Format("Player {0}", (PlayerController.currentPlayerIndex + 1).ToString());
    }
    public void DrawCardButton()
    {
        DrawCard = true;
    }
    

}
