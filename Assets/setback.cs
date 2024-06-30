using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setback : MonoBehaviour
{
    private void OnColisionEnter2D(Collision2D collision) {

        if (collision.collider.GetComponent<MovementScript>()) {

            collision.collider.GetComponent<MovementScript>().Die();
        }
    }
}
