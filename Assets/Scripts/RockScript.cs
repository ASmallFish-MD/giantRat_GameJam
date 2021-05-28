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

    public float rockSizePercentVariation = 15f;

    public float smallRockSize = 1f;

    public List<Sprite> smallRockSprites;

    public enum state {
        FallingOntoPlatform,
        OnPlatform,
        Grabbed,
        FallingAfterGrab, //this may or may not be falling off the platform
        FallingOffPlatform, //this is falling off the platform
        BeingBroken
    }

    public state rockState;



    public enum size {
        Big, Small
    }

    public size rockSize;

    private float platformTopY;
    private float platformLeftEdge;
    private float platformRightEdge;

    //TODO
    //public sounds[]
    //THUD, falling onto platform, falling off platform (optional),
    //pickaxe clink, rock break

    // Start is called before the first frame update
    void Start(){
        rockState = state.FallingOntoPlatform;

        Transform platform = GameObject.FindGameObjectWithTag("Platform").transform;
        platformTopY = platform.position.y + platform.localScale.y/2f + transform.localScale.y/2f;
        platformLeftEdge = platform.position.x - platform.localScale.x/2f;
        platformRightEdge = platform.position.x + platform.localScale.x/2f;
    }

    // Update is called once per frame
    void Update(){
        if(rockState == state.FallingOntoPlatform || rockState == state.FallingAfterGrab || rockState == state.FallingOffPlatform){
            transform.Translate(Vector2.down * Time.deltaTime * fallSpeed);
        }
        if(rockState == state.FallingAfterGrab && transform.position.y < offPlatformYPos){
            rockState = state.FallingOffPlatform;
            //Play falling off sound (optional)
        }else if(rockState == state.FallingOffPlatform && transform.position.y < offScreenYPos){
            Object.Destroy(gameObject);
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
                //play pickup sound

                //parent itself to player
                transform.SetParent(
                    GameObject.FindGameObjectWithTag("Player").transform.Find("GrabSlot")
                );
                transform.localPosition = Vector3.zero;//reset position in players hand
            }else if(rockSize == size.Big){
                rockState = state.BeingBroken;
                //TODO Play pickaxe and rock break sound

                //spawn 2 small rocks
                spawnSmallRocks();

                //Delete self
                Object.Destroy(gameObject);
            }
        }else if(rockState == state.Grabbed){
            rockState = state.FallingAfterGrab;
            //unparent itself to player
            transform.SetParent(
                GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform
            );
        }
    }

    void spawnSmallRocks(){

        var rockPrefab = Resources.Load<Transform>("Prefabs/FallingRock");
        float randSizeMultiplier;
        float rockSize;

        float[] smallRockOffsets = {+1f, -1f};

        for (int i = 0; i < 2; i++){
            randSizeMultiplier = 1f + (Random.Range(-1f, 1f) * rockSizePercentVariation / 100f);
            rockSize = randSizeMultiplier * smallRockSize;
            Vector3 rockPos = transform.position + Vector3.right * distanceToSpawnSmallRockAt * smallRockOffsets[i];
            //clamp new small rock position so it doesnt fly off the edge?
            //rockPos.x = Mathf.Clamp(rockPos.x, platformLeftEdge + rockSize/2f, platformRightEdge - rockSize/2f);
            Transform spawnedRock = Instantiate(rockPrefab, rockPos, Quaternion.identity).transform;
            spawnedRock.GetComponent<RockScript>().rockSize = RockScript.size.Small;
            spawnedRock.transform.SetParent(
                GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform
            );
            spawnedRock.transform.localScale = new Vector2(rockSize, rockSize);
            spawnedRock.GetComponent<SpriteRenderer>().sprite = smallRockSprites[Random.Range(0, smallRockSprites.Count)];
        }
        
        return;
    }
}
