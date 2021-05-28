using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckScript : MonoBehaviour{

    public float fallSpeed = 10f;
    public float offScreenYPos = -10;
    public float offPlatformYPos = -2;

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
    }

    // Update is called once per frame
    void Update(){
        if(duckState == state.FallingAfterGrab || duckState == state.FallingOffPlatform){
            transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
        }else if(duckState == state.FallingAfterGrab && transform.position.y < offPlatformYPos){
            duckState = state.FallingOffPlatform;
            //Play falling off sound (optional)
        }else if(duckState == state.FallingOffPlatform && transform.position.y < offScreenYPos){
            //todo GAMEOVER
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        else if(duckState == state.FallingAfterGrab && other.CompareTag("Platform")){
            duckState = state.OnPlatform;
            //play hit platform sound
        }
    }

    public void reactToPlayerAction(){
        if(duckState == state.OnPlatform){
            duckState = state.Grabbed;
            //parent itself to player
            //play pickup sound
        }else if(duckState == state.Grabbed){
            duckState = state.FallingAfterGrab;
        }
    }
}
