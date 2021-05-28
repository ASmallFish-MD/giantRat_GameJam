using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    private CharacterController controller;
    
    // because of bad coding practices these need to be changed if you want to change the sprite scale, sorry
    public Vector3 faccingLeft = new Vector3(-0.3f, 0.3f, 0.3f);
    public Vector3 faccingRight = new Vector3(0.3f, 0.3f, 0.3f);

    // also because we've got no idea what we're doing, this is gonna be how we decide when the character falls off.
    public float platformEdgeL = -5.5f;
    public float platformEdgeR = 5.5f;
    public float wallEdgeL = -8;
    public float wallEdgeR = 8;

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
        //controller = gameObject.AddComponent<CharacterController>();
        //controller.detectCollisions = true;
    }

    // Update is called once per frame
    void Update()
    {
        // input
        float xInput = Input.GetAxisRaw("Horizontal");
        bool valid = xInput != 0;

        // position
        float playerXpos = sprite.transform.position.x;

        // vertical speed calculation
        if (playerXpos < platformEdgeL || playerXpos > platformEdgeR) {
            playerSpeed.y = max(playerSpeed.y - 0.05f * Time.deltaTime, -maxSpeed);
            xInput = 0;
        }

        // horizontal speed calculation
        if (valid && !isOpposite(xInput, playerSpeed.x)) {
            playerSpeed.x = min(xInput * Time.deltaTime * playerAcceleration + playerSpeed.x, maxSpeed); 
        } else {
            playerSpeed.x = max(-playerDeceleration * Time.deltaTime + playerSpeed.x, 0);
        }

            // collision logic
        if (playerXpos < wallEdgeL || playerXpos > wallEdgeR) {
            playerSpeed.x = 0;
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

        transform.Translate(playerSpeed);

        if (Input.GetButtonDown("Use")){
            PlayerAction.actionType actionTypeDone = this.GetComponent<PlayerAction>().performPlayerAction();
            if(actionTypeDone == PlayerAction.actionType.PickingUp){
                //PickingUp animation
            }
            else if(actionTypeDone == PlayerAction.actionType.Dropping){
                //Dropping animation
            }
            else if(actionTypeDone == PlayerAction.actionType.UsePickaxe){
                //UsePickaxe animation
            }
            else if(actionTypeDone == PlayerAction.actionType.Nothing){
                //TODO: optionally play a 'cant pickup' sound
            }
        }
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
