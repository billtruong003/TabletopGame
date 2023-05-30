using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGrid : MonoBehaviour
{
    [Header("Tạo bàn cờ để chơi")]
    public GameObject gridGenerate;
    public List<GameObject> Grid;
    public Transform gridParent;
    public int numberOfGrid;
    public int numberOfLine;
    public float tileSize;
    public Color32 evenGrid;
    public Color32 oddGrid;
    private float startX;
    private float startY;

    [Header("Tạo bẫy")]
    public GameObject TrapPrefabs;
    public bool checkTrap;
    public int Trap_randomPos;
    public int trapNumber;
    public List<GameObject> Trap;
    public List<int> TrapPosition;
    public Transform trapParent;
    public Transform trapPos;
    public Color32 TrapColor;

    [Header("Tạo ô tăng phúc")]
    public GameObject boostPrefabs;
    public bool checkBoost;
    public int Boost_randomPos;
    public int boostNumber;
    public List<GameObject> boost;
    public List<int> boostPosition;
    public Transform boostParent;
    public Transform boostPos;
    public Color32 boostColor;
    
    private void Start()
    {
        numberOfLine = numberOfGrid / 10;
        startX = -(numberOfGrid / 2) * tileSize + tileSize / 2;
        startY = -(numberOfLine / 2) * tileSize + tileSize / 2;
        StartCoroutine(MapSetup());
        
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < numberOfLine; i++)
        {
            startY = i - ((numberOfLine / 2) - 0.5f);
            for (int j = 0; j < numberOfGrid / numberOfLine; j++)
            {
                startX = j - (((numberOfGrid / numberOfLine) / 2) - 0.5f);
                Vector3 tilePos = new Vector3(startX, startY);
                GameObject grid = Instantiate(gridGenerate, tilePos, Quaternion.identity, gridParent);
                Grid.Add(grid);
                SpriteRenderer colorSprite = grid.GetComponent<SpriteRenderer>();
                colorSprite.color = (i + j) % 2 == 0 ? evenGrid : oddGrid;
            }
        }
        trapGenerate();
    }
    public void trapGenerate(){
        for(int i = 0; i < trapNumber; i++) {
            Debug.Log("Start trap rồi nè");
            trapPos = Grid[Random.Range(1, numberOfGrid - 1)].transform;
            GameObject trapObj = Instantiate(TrapPrefabs, trapPos.position, Quaternion.identity, trapParent);
            Trap.Add(trapObj);
            SpriteRenderer trapSprite = trapObj.GetComponent<SpriteRenderer>();
            trapSprite.color = TrapColor;
            trapSprite.sortingOrder = 2;
        }
    }
    public void boostGenerate(){
        for(int i = 0; i < boostNumber; i++) {
            Debug.Log("Start boost rồi nè");
            boostPos = Grid[Random.Range(1, numberOfGrid - 1)].transform;
            GameObject boostObj = Instantiate(TrapPrefabs, trapPos.position, Quaternion.identity, trapParent);
            boost.Add(boostObj);
            SpriteRenderer trapSprite = boostObj.GetComponent<SpriteRenderer>();
            trapSprite.color = boostColor;
            trapSprite.sortingOrder = 2;
        }
    }
    IEnumerator MapSetup(){
        for (int i = 0; i < numberOfLine; i++)
        {
            startY = i - ((numberOfLine / 2) - 0.5f);
            for (int j = 0; j < numberOfGrid / numberOfLine; j++)
            {
                startX = j - (((numberOfGrid / numberOfLine) / 2) - 0.5f);
                Vector3 tilePos = new Vector3(startX, startY);
                GameObject grid = Instantiate(gridGenerate, tilePos, Quaternion.identity, gridParent);
                Grid.Add(grid);
                SpriteRenderer colorSprite = grid.GetComponent<SpriteRenderer>();
                colorSprite.color = (i + j) % 2 == 0 ? evenGrid : oddGrid;
                yield return new WaitForSeconds(0.02f);
            }
        }
        trapNumber = Random.Range(trapNumber, trapNumber + 3);
        for(int i = 0; i < trapNumber; i++) {
            checkTrap = true;
            Trap_randomPos = Random.Range(10, numberOfGrid - 1);
            // Kiểm tra xem vị trí boost_randomPos có trùng với bất kỳ vị trí trap nào không
            while (TrapPosition.Contains(Boost_randomPos) && TrapPosition.Count != 0)
            {
                Boost_randomPos = Random.Range(1, numberOfGrid - 1);
            }
            trapPos = Grid[Trap_randomPos].transform;
            TrapPosition.Add(Trap_randomPos);
            GameObject trapObj = Instantiate(TrapPrefabs, trapPos.position, Quaternion.identity, trapParent);
            Trap.Add(trapObj);
            SpriteRenderer trapSprite = trapObj.GetComponent<SpriteRenderer>();
            trapSprite.color = TrapColor;
            trapSprite.sortingOrder = 2;
            trapSprite.transform.eulerAngles = new Vector3(0, 0, 90);
            yield return new WaitForSeconds(0.02f);
        }

        for (int i = 0; i < boostNumber; i++)
        {
            checkBoost = true;
            Boost_randomPos = Random.Range(10, numberOfGrid - 1);
            
            // Kiểm tra xem vị trí boost_randomPos có trùng với bất kỳ vị trí trap nào không
            while (TrapPosition.Contains(Boost_randomPos) && TrapPosition.Count != 0)
            {
                Boost_randomPos = Random.Range(1, numberOfGrid - 1);
            }
            
            boostPos = Grid[Boost_randomPos].transform;
            boostPosition.Add(Boost_randomPos);
            GameObject boostObj = Instantiate(boostPrefabs, boostPos.position, Quaternion.identity, boostParent);
            Trap.Add(boostObj);
            SpriteRenderer boostSprite = boostObj.GetComponent<SpriteRenderer>();
            boostSprite.color = boostColor;
            boostSprite.sortingOrder = 2;
            boostSprite.transform.eulerAngles = new Vector3(0, 0, -90);
            yield return new WaitForSeconds(0.02f);
        }
        GameController.canPlay = true;
        yield return null;
    }
}
