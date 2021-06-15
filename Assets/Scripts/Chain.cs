using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    [SerializeField] Transform character1;
    [SerializeField] Transform character2;

    [SerializeField] float maxChainlength = 8f; //Can change between Levels
    [SerializeField] float chainShowColourPercentage = 0.6f;
    SpriteRenderer spriteRenderer;
    LevelController levelController;

    PlayerController player1, player2;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        levelController = FindObjectOfType<LevelController>();
        player1 = character1.GetComponent<PlayerController>();
        player2 = character2.GetComponent<PlayerController>();
    }

    private void Update()
    {
        //Debug.DrawLine(Character1.position, Character2.position);



        Vector3 direction = character2.position - character1.position;
        transform.position = (character1.position + character2.position) / 2;
        //spriteRenderer.gameObject.transform.localScale = new Vector3(
        //    direction.magnitude, spriteRenderer.gameObject.transform.localScale.y);

        spriteRenderer.size = new Vector2(direction.magnitude, spriteRenderer.size.y);
        CheckChainBroken(direction.magnitude);
        direction.Normalize();
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
    }

    private void CheckChainBroken(float length)
    {
        if (levelController == null)
        {
            return;
        }

        if (levelController.IsLevelOver())
        {
            return;
        }

        if (length / maxChainlength < chainShowColourPercentage)
        {
            spriteRenderer.color = Color.white;
            player1.SetDefaultFace();
            player2.SetDefaultFace();
            return;
        }
        spriteRenderer.color = Color.Lerp(Color.white, Color.red, 
            (length -(maxChainlength * chainShowColourPercentage)) / (maxChainlength * (1- chainShowColourPercentage)));
        player1.SetWorryFace();
        player2.SetWorryFace();

        if(length > maxChainlength)
        {
            player1.SetDeathFace();
            player2.SetDeathFace();
            Destroy(this.gameObject);
            levelController.LoseLevel();
        }
    }
}
