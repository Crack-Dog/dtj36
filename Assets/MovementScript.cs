using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float playerSpeed;

    public float jumpMax = 16;
    public float jumpMin = 8;
    public bool jumpBool;
    public bool jumpCancelBool;

    public Rigidbody2D rb;

    private float Move;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    public float playerDash;
    public float dashDuration;
    public bool isDashing = false;
    public float dashCooldown;
    public bool canDash = true;

    private void Awake() {
        startingPosition = transform.position;
    }
    Vector2 startingPosition;
    public void Die() {
        transform.position = startingPosition;
    }

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

        Move = Input.GetAxis("Horizontal"); // Basic movement, returns 1 or -1 depending on direction
        rb.velocity = new Vector2(playerSpeed * Move, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) // Jump function
        {
            if (!jumpBool)
            {
                jumpBool = true;
                jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && !isGrounded()){
            if (!jumpCancelBool)
            {
                jumpCancelBool = true;
                jumpCancel();
            }
        }
        {
            
        }


        if (Input.GetKeyDown(KeyCode.Q) && canDash) // Dash function
        {
            StartCoroutine(Dash());
        }

    }

   
    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpMax);
        jumpBool = false;
    }
    
    private void jumpCancel()
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
}