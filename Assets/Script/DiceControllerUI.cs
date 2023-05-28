using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceControllerUI : MonoBehaviour
{
    public Sprite[] diceFaces; // Mảng chứa các hình ảnh của các mặt xúc xắc
    private Image imageDice;
    private bool isRolling = false;
    public PlayerController playerController; // Tham chiếu đến PlayerController
    int randomFaceIndex;
    public int faceAppear;

    private void Awake()
    {
        imageDice = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameController.canPlay)
        {
            if (!isRolling)
            {
                StartCoroutine(RollDice());
            }
        }
    }

    private IEnumerator RollDice()
    {
        isRolling = true;

        for (int i = 0; i < Random.Range(20, 30); i++)
        {
            yield return new WaitForSeconds(0.05f);
            randomFaceIndex = Random.Range(0, diceFaces.Length);
            imageDice.sprite = diceFaces[randomFaceIndex];
        }

        int faceAppear = randomFaceIndex + 1;
        isRolling = false;
        Debug.Log(randomFaceIndex);

        // Di chuyển người chơi
        StartCoroutine(playerController.CharacterMove(faceAppear)); 
    }
}
