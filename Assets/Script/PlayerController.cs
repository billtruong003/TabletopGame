using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<GameObject> players; // Danh sách đối tượng người chơi
    public GameObject playerPrefabs;
    public SpawnGrid spawnGrid; // Tham chiếu đến script SpawnGrid
    private int currentPlayerIndex; // Chỉ số của người chơi đang thực hiện nước đi
    private bool setupMove; // Biến kiểm tra xem người chơi có thể di chuyển hay không
    public LayerMask traplayerMask;
    public int numberOfPlayers; 
    public Transform playerParent;

    private void Start()
    {
        currentPlayerIndex = 0; // Khởi tạo chỉ số người chơi đầu tiên là 0
        setupMove = false; // Khởi tạo setupMove là false
        spawnGrid = GetComponent<SpawnGrid>(); // Lấy tham chiếu đến script SpawnGrid
        SpawnPlayers(); // Đặt vị trí ban đầu cho tất cả người chơi
    }

    private void Update()
    {
        trapCollide();
    }

    private void trapCollide()
    {
        if (CheckCollisionWithTrap(players[currentPlayerIndex].GetComponent<Collider2D>(), traplayerMask) && !setupMove)
        {
            Debug.Log("Va chạm với trap");
            StartCoroutine(CharacterMove(-3));
        }
    }

    public bool CheckCollisionWithTrap(Collider2D circleCollider, LayerMask layerMask)
    {
        // Kiểm tra va chạm với các đối tượng trong layer chỉ định
        Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCollider.bounds.center, circleCollider.bounds.extents.x, layerMask);

        // Nếu có ít nhất một collider trong mảng, tức là có va chạm
        return colliders.Length > 0;
    }

    private void SpawnPlayers()
    {
        if (spawnGrid.Grid.Count != 0 && !setupMove)
        {
            for (int i = 0; i < numberOfPlayers; i++)
            {
                GameObject player = Instantiate(playerPrefabs, new Vector3(0,0),Quaternion.identity, playerParent);
                players.Add(player);
                // Kiểm tra xem danh sách các ô trên lưới có đủ để đặt người chơi không
                player.transform.position = spawnGrid.Grid[0].transform.position; // Đặt vị trí ban đầu của người chơi
                player.GetComponent<PlayerData>().position = i; // Cập nhật vị trí của người chơi trong PlayerDatad
            }

            setupMove = true; // Đánh dấu setupMove là true để ngăn việc đặt vị trí ban đầu nhiều lần
        }
    }

    public IEnumerator CharacterMove(int step)
    {
        setupMove = true; // Đánh dấu setupMove là true để ngăn việc di chuyển trong quá trình di chuyển hiện tại
        int nextPosition = players[currentPlayerIndex].GetComponent<PlayerData>().position + step; // Tính toán vị trí tiếp theo của người chơi

        if (nextPosition < spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[nextPosition].transform.position; // Vị trí đích của người chơi là vị trí tiếp theo trên lưới

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime); // Di chuyển người chơi từ vị trí hiện tại đến vị trí đích
                yield return null;
            }

            players[currentPlayerIndex].GetComponent<PlayerData>().position = nextPosition; // Cập nhật vị trí hiện tại của người chơi
        }
        else if (nextPosition >= spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[(int)(Mathf.Round((float)nextPosition / 10) * 10) - 1].transform.position; // Vị trí đích của người chơi là vị trí cuối cùng trên lưới

            while (players[currentPlayerIndex].transform.position != targetPosition)
            {
                players[currentPlayerIndex].transform.position = Vector3.MoveTowards(players[currentPlayerIndex].transform.position, targetPosition, 5f * Time.deltaTime); // Di chuyển người chơi từ vị trí hiện tại đến vị trí đích
                yield return null;
            }

            players[currentPlayerIndex].GetComponent<PlayerData>().position = nextPosition; // Cập nhật vị trí hiện tại của người chơi
        }

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Chuyển lượt cho người chơi tiếp theo trong danh sách
        setupMove = false; // Đánh dấu setupMove là false khi di chuyển đã kết thúc
    }
}
