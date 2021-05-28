using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO: audio after destroy
//https://answers.unity.com/questions/1213839/destroy-object-and-play-sound.html

public class RockScript : MonoBehaviour{

    public float fallSpeed = 10f;
    public GameObject shadow;

    public float offScreenYPos = -10;
    public float offPlatformYPos = -2;

    

    public enum state {
        FallingOntoPlatform,
        OnPlatform,
        Grabbed,
        FallingAfterGrab, //this may or may not be falling off the platform
        FallingOffPlatform //this is falling off the platform
    }

    public state rockState;



    public enum size {
        Big, Small
    }

    public size rockSize;

    //TODO
    //public sounds[]
    //THUD, falling onto platform, falling off platform (optional),
    //pickaxe clink, rock break

    // Start is called before the first frame update
    void Start(){
        rockState = state.FallingOntoPlatform;
    }

    // Update is called once per frame
    void Update(){
        if(rockState == state.FallingOntoPlatform || rockState == state.FallingAfterGrab){
            transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
        }else if(rockState == state.FallingAfterGrab && transform.position.y < offPlatformYPos){
            rockState = state.FallingOffPlatform;
            //Play falling off sound (optional)
        }else if(rockState == state.FallingOffPlatform && transform.position.y < offScreenYPos){
            //todo destroy
        }

        if(rockState == state.FallingOntoPlatform){
            //update shadow
        }
    }

    private void OnTriggerEnter(Collider other){
        if(rockState == state.FallingOntoPlatform && other.CompareTag("Player")){
            //GAME OVER due to player being conked
        }
        else if(rockState == state.FallingOntoPlatform && other.CompareTag("Duck")){
            //GAME OVER due to duck being crushed
        }
        else if(rockState == state.FallingOntoPlatform && other.CompareTag("Platform")){
            rockState = state.OnPlatform;
            //TODO:delete shadow
            //play thud sound
        }
    }

    public void playerAction(){
        if(rockState == state.OnPlatform){
            if(rockSize == size.Small){
                rockState = state.Grabbed;
                //parent itself to player
                //play pickup sound
            }else if(rockSize == size.Big){
                //Play pickaxe and rock break sound
                //TODO:spawn 2 small rock
                //Delete self
            }
        }else if(rockState == state.Grabbed){
            rockState = state.FallingAfterGrab;
        }
    }
}
