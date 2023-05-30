using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> players;
    public GameObject playerPrefab;
    public SpawnGrid spawnGrid;
    public int currentPlayerIndex;
    private bool setupMove;
    public LayerMask trapLayerMask;
    public LayerMask boostLayerMask;
    public Transform playerParent;

    private void Start()
    {
        currentPlayerIndex = 0;
        setupMove = false;
        spawnGrid = GetComponent<SpawnGrid>();
        SpawnPlayers();
    }

    private void Update()
    {   
        
    }

    private void Boost_TrapCollide()
    {
        bool isCollidingWithTrap = CheckCollisionWithTrap(players[currentPlayerIndex].GetComponent<Collider2D>(), trapLayerMask);
        bool isCollidingWithBoost = CheckCollisionWithTrap(players[currentPlayerIndex].GetComponent<Collider2D>(), boostLayerMask);
        
        if (isCollidingWithTrap && !setupMove)
        {
            StartCoroutine(Boost_TrapMove(-3));
        }
        else if (isCollidingWithBoost && !setupMove)
        {
            StartCoroutine(Boost_TrapMove(3));
        }
        else
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            GameController.diceReady = true;
        }
    }
    

    public bool CheckCollisionWithTrap(Collider2D circleCollider, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCollider.bounds.center, circleCollider.bounds.extents.x, layerMask);
        return colliders.Length > 0;
    }
    public bool CheckCollisionWithBoost(Collider2D circleCollider, LayerMask layerMask)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCollider.bounds.center, circleCollider.bounds.extents.x, layerMask);
        return colliders.Length > 0;
    }

    private void SpawnPlayers()
    {
        if (spawnGrid.Grid.Count != 0 && !setupMove)
        {
            for (int i = 0; i < GameController.numberOfPlayers; i++)
            {
                GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, playerParent);
                players.Add(player);
                player.transform.position = spawnGrid.Grid[0].transform.position;
                player.GetComponent<PlayerData>().position = 0;
            }

            setupMove = true;
        }
    }

    public IEnumerator CharacterMove(int step)
    {
        GameController.diceReady = false;
        setupMove = true;
        int nextPosition = players[currentPlayerIndex].GetComponent<PlayerData>().position + step;

        if (nextPosition < spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[nextPosition].transform.position;

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null;
            }

            players[currentPlayerIndex].GetComponent<PlayerData>().position = nextPosition;
        }
        else if (nextPosition >= spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[(int)(Mathf.Round((float)nextPosition / 10) * 10) - 1].transform.position;

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null;
            }
        }
        
        setupMove = false;
        Debug.Log("Chạy Trap Check");
        Boost_TrapCollide();
    }
    public IEnumerator Boost_TrapMove(int step)
    {
        setupMove = true;
        int nextPosition = players[currentPlayerIndex].GetComponent<PlayerData>().position + step;

        if (nextPosition < spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[nextPosition].transform.position;

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null;
            }

            players[currentPlayerIndex].GetComponent<PlayerData>().position = nextPosition;
        }
        else if (nextPosition >= spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[(int)(Mathf.Round((float)nextPosition / 10) * 10) - 1].transform.position;

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime);
                yield return null;
            }
        }
        
        setupMove = false;
        Debug.Log("Chạy Trap Check");
        Boost_TrapCollide();
    }
}
