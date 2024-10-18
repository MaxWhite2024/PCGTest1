using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpForce = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        rb.AddForce(Vector2.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);

        if(Input.GetKeyDown("space"))
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
