using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    private CharacterController controller;
    
    // because of bad coding practices these need to be changed if you want to change the sprite scale, sorry
    private Vector3 faccingLeft = new Vector3(-0.3f, 0.3f, 0.3f);
    private Vector3 faccingRight = new Vector3(0.3f, 0.3f, 0.3f);

    // also because we've got no idea what we're doing, this is gonna be how we decide when the character falls off.
    private float platformEdgeL;
    private float platformEdgeR;
    private float wallEdgeL;
    private float wallEdgeR;

    private float maxSpeed = 0.3f;
    private float playerAcceleration = 0.05f;
    private float playerDeceleration = 0.2f;
    private Vector3 playerSpeed = new Vector3(0, 0, 0);
    private bool facingRight = true;

    public Animator animator;
    public GameObject sprite;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        controller.detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        if (xInput != 0 && !isOpposite(xInput, playerSpeed.x)) {
            playerSpeed.x = min(xInput * Time.deltaTime * playerAcceleration + playerSpeed.x, maxSpeed); 
        } else {
            playerSpeed.x = max(-playerDeceleration * Time.deltaTime + playerSpeed.x, 0);
        }

        // movement logic should occur before here
        animator.SetFloat("playerSpeed", Mathf.Abs(playerSpeed.x));

        if (xInput > 0 && !facingRight) {
            sprite.transform.localScale = faccingRight;
            facingRight = true;
        } else if (xInput < 0 && facingRight) {
            sprite.transform.localScale = faccingLeft;
            facingRight = false;
        }

        controller.Move(playerSpeed);
    }

    float min(float a, float b) {
        if (a < b) {
            return a;
        }
        return b;
    }

    float max(float a, float b) {
        if (a > b) {
            return a;
        }
        return b;
    }

    bool isOpposite(float a, float b) {
        return ((a < 0 && b > 0) || (a > 0 && b < 0));
    }
}
