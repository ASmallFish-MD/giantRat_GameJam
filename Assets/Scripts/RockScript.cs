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

    public float distanceToSpawnSmallRockAt = 0.1f;

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

    private float platformTopY;

    //TODO
    //public sounds[]
    //THUD, falling onto platform, falling off platform (optional),
    //pickaxe clink, rock break

    // Start is called before the first frame update
    void Start(){
        rockState = state.FallingOntoPlatform;

        Transform platform = GameObject.FindGameObjectWithTag("Platform").transform;
        platformTopY = platform.position.y + platform.localScale.y/2f + transform.localScale.y/2f;
    }

    // Update is called once per frame
    void Update(){
        if(rockState == state.FallingOntoPlatform || rockState == state.FallingAfterGrab || rockState == state.FallingOffPlatform){
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

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log(other.tag);
        if(rockState == state.FallingOntoPlatform && other.CompareTag("Player")){
            //GAME OVER due to player being conked
        }
        else if(rockState == state.FallingOntoPlatform && other.CompareTag("Duck")){
            //GAME OVER due to duck being crushed
        }
        else if((rockState == state.FallingOntoPlatform || rockState == state.FallingAfterGrab) && other.CompareTag("Platform")){
            rockState = state.OnPlatform;
            transform.position = new Vector2(transform.position.x, platformTopY); //reset y position

            //TODO:delete shadow
            //play thud sound
        }
    }

    public void reactToPlayerAction(){
        if(rockState == state.OnPlatform){
            if(rockSize == size.Small){
                rockState = state.Grabbed;
                //parent itself to player
                //play pickup sound
            }else if(rockSize == size.Big){
                //TODO Play pickaxe and rock break sound

                //spawn 2 small rock
                Transform smallRock1 = Instantiate(rockPrefab, transform.position + new Vector3(distanceToSpawnSmallRockAt, 0), Quaternion.identity);
                Transform smallRock2 = Instantiate(rockPrefab, transform.position - new Vector3(distanceToSpawnSmallRockAt, 0), Quaternion.identity);
                
                smallRock1.GetComponent<RockScript>().rockSize = RockScript.size.Small;
                smallRock2.GetComponent<RockScript>().rockSize = RockScript.size.Small;

                //Delete self
                Object.Destroy(this.gameObject);
            }
        }else if(rockState == state.Grabbed){
            rockState = state.FallingAfterGrab;
        }
    }
}
