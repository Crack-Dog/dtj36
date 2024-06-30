using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float playerSpeed; // Base speed
    private float Move;

    // Jump variables/booleans
    public float jumpMax = 16; 
    public float jumpMin = 8;
    public bool jumpBool;
    public bool jumpCancelBool;

    // Related to player death
    public bool playerStuck = false;
    public float deadTime = 1;
    public bool playerFreeze = false;
    public float freezeTime = 5;

    public Rigidbody2D rb;

    // Box cast
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    // Dash variables/booleans
    public float playerDash;
    public float dashDuration;
    public bool isDashing = false;
    public float dashCooldown;
    public bool canDash = true;

   
    // Start is called before the first frame update
    void Start()
    {
        if (isDashing) // Restricts movement while dashing
        {
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (isDashing) // Restricts movement while dashing
        {
            return;
        }

        if (playerStuck) // When playStuck is true, restrict movement and run teleport method
        {
            StartCoroutine(teleport());
            return;
        }

        if (playerFreeze)
        {
            return;
        }


        Move = Input.GetAxis("Horizontal"); // Basic movement, returns 1 or -1 depending on direction
        rb.velocity = new Vector2(playerSpeed * Move, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) // Jump method
        {
            if (!jumpBool)
            {
                jumpBool = true;
                jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !isGrounded()){ // Checks if cancelling variable jump
            if (!jumpCancelBool)
            {
                jumpCancelBool = true;
                jumpCancel();
            }
        }
        {
            
        }


        if (Input.GetKeyDown(KeyCode.Q) && canDash) // Dash method
        {
            StartCoroutine(Dash());
        }

    }

   
    private void jump() // Base jump method
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpMax);
        jumpBool = false;
    }
    
    private void jumpCancel() // Variable jump, when jumpCancel, replaces current velocity with lower velocity if applicable
    {
        if (rb.velocity.y > jumpMin)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpMin);
        }
        jumpCancelBool = false;
    }


    public bool isGrounded() // Check if BoxCast interacting with ground layer
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnDrawGizmos() // Function to visualize BoxCast area
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }


    private IEnumerator Dash() // Dash coroutine logic
    {

        isDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // Prevents player from interacting with gravity during dash
        rb.velocity = new Vector2(Move * playerDash, 0);
        yield return new WaitForSeconds(dashDuration); // Propels player for dash duration
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown); // Dash cooldown
        canDash = true;
    }


     void OnCollisionEnter2D(Collision2D other) // Checks if the player is interacting with kill objects
    {
        if (other.gameObject.CompareTag("kill"))
        {
            playerStuck = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("freeze"))
        {
            playerFreeze = true;
            StartCoroutine(freeze());
        }
    }

    private IEnumerator teleport() // Teleports player to spawn when called
    {
        yield return new WaitForSeconds(deadTime);
        transform.position = new Vector2(0, -2);
        playerStuck = false;
    }

    private IEnumerator freeze()
    {
        yield return new WaitForSeconds(freezeTime);
        playerFreeze = false;
    }
}