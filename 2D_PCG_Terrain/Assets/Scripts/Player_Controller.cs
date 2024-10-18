using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 1f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Jump");
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.velocity += new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        //rb.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }
}
