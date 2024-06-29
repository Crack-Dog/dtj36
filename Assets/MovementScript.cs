using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float playerSpeed;
    public float playerJump;

    public Rigidbody2D rb;

    private float Move;

    public bool isJumping = false;

    public float playerDash;
    public float dashDuration;
    public bool isDashing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (isDashing)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isDashing)
        {
            return;
        }

        Move = Input.GetAxis("Horizontal"); // Basic movement, returns 1 or -1 depending on direction
        rb.velocity = new Vector2(playerSpeed * Move, rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && isJumping == false) // Jump function
        {
            rb.velocity += Vector2.up * playerJump;
        }
    

        if (Input.GetKeyDown(KeyCode.Q)) // Dash function
        {
            StartCoroutine(Dash());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = true;
        }
    }

    private IEnumerator Dash()
    {

        isDashing = true;
        rb.velocity = new Vector2(Move * playerDash, rb.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}
