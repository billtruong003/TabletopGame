using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player; // Đối tượng người chơi
    public SpawnGrid spawnGrid; // Tham chiếu đến script SpawnGrid
    public int position; // Vị trí hiện tại của người chơi trên lưới
    private bool setupMove; // Biến kiểm tra xem người chơi có thể di chuyển hay không
    public LayerMask traplayerMask;

    private void Start()
    {
        position = 0; // Khởi tạo vị trí ban đầu là 0
        setupMove = false; // Khởi tạo setupMove là false
        spawnGrid = GetComponent<SpawnGrid>(); // Lấy tham chiếu đến script SpawnGrid
        SpawnPlayer(); // Đặt vị trí ban đầu của người chơi trên lưới
    }
    private void Update() {
        trapCollide();
    }
    private void trapCollide()
    {
        if(CheckCollisionWithTrap(player.GetComponent<Collider2D>(), traplayerMask) && !setupMove){
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

    private void SpawnPlayer()
    {
        if (spawnGrid.Grid.Count != 0 && !setupMove)
        {
            player.transform.position = spawnGrid.Grid[0].transform.position; // Đặt vị trí ban đầu của người chơi là vị trí đầu tiên trên lưới
            setupMove = true; // Đánh dấu setupMove là true để ngăn việc đặt vị trí ban đầu nhiều lần
        }
    }
    

    public IEnumerator CharacterMove(int step)
    {
        setupMove = true; // Đánh dấu setupMove là true để ngăn việc di chuyển trong quá trình di chuyển hiện tại
        int nextPosition = position + step; // Tính toán vị trí tiếp theo của người chơi

        if (nextPosition < spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[nextPosition].transform.position; // Vị trí đích của người chơi là vị trí tiếp theo trên lưới

            while (player.transform.position != targetPosition)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 5f * Time.deltaTime); // Di chuyển người chơi từ vị trí hiện tại đến vị trí đích
                yield return null;
            }

            position = nextPosition; // Cập nhật vị trí hiện tại của người chơi
        }
        else if (nextPosition >= spawnGrid.Grid.Count)
        {
            Vector3 targetPosition = spawnGrid.Grid[(int)(Mathf.Round((float)nextPosition / 10) * 10) - 1].transform.position; // Vị trí đích của người chơi là vị trí cuối cùng trên lưới

            while (player.transform.position != targetPosition)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 5f * Time.deltaTime); // Di chuyển người chơi từ vị trí hiện tại đến vị trí đích
                yield return null;
            }

            position = nextPosition; // Cập nhật vị trí hiện tại của người chơi
        }

        setupMove = false; // Đánh dấu setupMove là false khi di chuyển đã kết thúc
    }
}
