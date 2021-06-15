using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpSpeed = 500f;
    [SerializeField] bool isPlayer1 = true;
    [SerializeField] BoxCollider2D feetCollider;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite happySprite;
    [SerializeField] Sprite worriedFace;
    [SerializeField] Sprite deadFace;

    private bool canAct = true;
    LevelController levelController;

    float leftRightMovement = 0f;
    Rigidbody2D myRb2d;
    AudioManager audioManager;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRb2d = GetComponent<Rigidbody2D>();
        levelController = FindObjectOfType<LevelController>();
        audioManager = FindObjectOfType<AudioManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAct)
        {
            return;
        }
        CheckJump();
        
    }

    private void FixedUpdate()
    {
        if (!canAct)
        {
            return;
        }
        Movement();
        
    }

    private void Movement()
    {
        if(isPlayer1)
        {
            //Check player 1 keybinds
            leftRightMovement = Input.GetAxisRaw("HorizontalPlayer1");
        }
        else
        {
            //check player 2 keybinds
            leftRightMovement = Input.GetAxisRaw("HorizontalPlayer2");
        }
        Vector2 movement = new Vector2(leftRightMovement * moveSpeed, myRb2d.velocity.y);
        myRb2d.velocity = Vector2.Lerp(myRb2d.velocity, movement, 0.5f);

        //transform.position = new Vector2(transform.position.x + leftRightMovement * moveSpeed, transform.position.y);
    }

    private void CheckJump()
    {
        if (isPlayer1)
        {
            //Check player 1 jump
            if (Input.GetButtonDown("JumpPlayer1"))
            {
                Jump();
            }
        }
        else
        {
            //check player 2 jump
            if(Input.GetButtonDown("JumpPlayer2"))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        if(!feetCollider.IsTouchingLayers(Physics2D.AllLayers))
        {
            return;
        }
        audioManager.PlayClip("jump", isPlayer1 ? 0: 0.9f);
        //Check if touching ground first or w/e
        myRb2d.AddForce(new Vector2(0, jumpSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!canAct)
        {
            return;
        }

        int collisionLayer = 1 << collision.gameObject.layer;
        if(collisionLayer == LayerMask.GetMask("Purple") && isPlayer1)
        {
            //Load Next Level
            levelController.AddObjective();
            spriteRenderer.sprite = happySprite;
        }
        else if (collisionLayer == LayerMask.GetMask("Green") && !isPlayer1)
        {
            //Load Next Level
            levelController.AddObjective();
            spriteRenderer.sprite = happySprite;
        }
        else if(collisionLayer == LayerMask.GetMask("Lose"))
        {
            //Restart Current Level
            SetDeathFace();
            levelController.LoseLevel();
        }
        else if (collisionLayer == LayerMask.GetMask("Toggle"))
        {
            //Toggle the blue & red colours
            levelController.ToggleBarriers();
        }

        //Can add extra things here to toggle colours of the stuff  
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if(!canAct)
        {
            return;
        }
        int collisionLayer = 1 << collision.gameObject.layer;
        if (collisionLayer == LayerMask.GetMask("Purple") && isPlayer1)
        {
            //Load Next Level
            levelController.ReduceObjective();
        }
        else if (collisionLayer == LayerMask.GetMask("Green") && !isPlayer1)
        {
            //Load Next Level
            levelController.ReduceObjective();
        }
        else
        {
            return;
        }

        spriteRenderer.sprite = defaultSprite;
    }

    public void StopActions()
    {
        canAct = false;
    }

    public void SetDeathFace()
    {
        spriteRenderer.sprite = deadFace;
    }

    public void SetWorryFace()
    {
        //For when chain is close to breaking
        //Don't set if happy face is active
        if(spriteRenderer.sprite == happySprite)
        {
            return;
        }

        spriteRenderer.sprite = worriedFace;
    }

    public void SetDefaultFace()
    {
        if (spriteRenderer.sprite == happySprite)
        {
            return;
        }

        spriteRenderer.sprite = defaultSprite;
    }
}
