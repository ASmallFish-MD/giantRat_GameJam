using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckScript : MonoBehaviour{

    public float fallSpeed = 10f;
    public float offScreenYPos = -10;
    public float offPlatformYPos = -2;

    private float platformTopY;

    public enum state {
        OnPlatform,
        Grabbed,
        FallingAfterGrab, //this may or may not be falling off the platform
        FallingOffPlatform //this is falling off the platform
    }

    public state duckState;

    //TODO
    //public sounds[]
    //pickup (optional), hit platform, falling off platform (optional) 

    // Start is called before the first frame update
    void Start(){
        duckState = state.OnPlatform;

        Transform platform = GameObject.FindGameObjectWithTag("Platform").transform;
        platformTopY = platform.position.y + platform.localScale.y/2f + transform.localScale.y/2f;
    }

    // Update is called once per frame
    void Update(){
        if(duckState == state.FallingAfterGrab || duckState == state.FallingOffPlatform){
            transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
        }
        if(duckState == state.FallingAfterGrab && transform.position.y < offPlatformYPos){
            duckState = state.FallingOffPlatform;
            //Play falling off sound (optional)
            //todo
        }else if(duckState == state.FallingOffPlatform && transform.position.y < offScreenYPos){
            //GAMEOVER
            FindObjectOfType<GameController>().EndGame("Your duck is lost forever in the depths");
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(duckState == state.FallingAfterGrab && other.CompareTag("Platform")){
            duckState = state.OnPlatform;

            transform.position = new Vector2(transform.position.x, platformTopY); //reset y position
            //play hit platform sound
        }
    }

    public void reactToPlayerAction(){
        if(duckState == state.OnPlatform){
            duckState = state.Grabbed;
            //play pickup sound

            //parent itself to player
            transform.SetParent(
                GameObject.FindGameObjectWithTag("Player").transform.Find("GrabSlot")
            );
            transform.localPosition = Vector3.zero;//reset position in players hand
        }else if(duckState == state.Grabbed){
            duckState = state.FallingAfterGrab;
            //unparent itself to player
            transform.SetParent(
                GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform
            );
        }
    }
}
