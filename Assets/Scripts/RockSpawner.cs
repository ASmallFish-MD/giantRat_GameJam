using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour{
    private float platformLeftEdge;
    private float platformRightEdge;
    public float shadowYPos;

    public float rockSizePercentVariation = 15f;

    public List<float> rockSizes;
    public List<float> rockProbabilities;

    public Transform rockPrefab;
    public Transform rockShadowLargePrefab;
    public Transform rockShadowSmallPrefab;

    public float spawnYPos = 50f;

    public List<Sprite> smallRockSprites;
    public List<Sprite> bigRockSprites;

    private float platformTopY;

    public float delayBetweenRocks = 2f;
    private float lastFallingRockTime = 0;



    Transform beach;

    // Start is called before the first frame update
    void Start(){
        Transform platform = GameObject.FindGameObjectWithTag("Platform").transform;
        platformTopY = platform.position.y + platform.localScale.y/2f;
        platformLeftEdge = platform.position.x - platform.localScale.x/2f;
        platformRightEdge = platform.position.x + platform.localScale.x/2f;
        
        beach = GameObject.FindGameObjectWithTag("TheBeach").transform;
    }

    // Update is called once per frame
    void Update(){

        if(checkForFallingRocks()){
            lastFallingRockTime = Time.time;
        }
        
        if(Time.time > lastFallingRockTime + delayBetweenRocks
            && beach.position.y>10f){
            spawnRock();
            lastFallingRockTime = Time.time;
        }

        /*
        if(Input.GetButtonDown("Fire2")){
            if(!checkForFallingRocks()){
                spawnRock();
            }
        }
        */

        //spawn rocks faster to thurther you go up
        spawnYPos = Mathf.Lerp(20f, 50f, beach.position.y/150f);
    }

    bool checkForFallingRocks(){
        int nActionableObjects = this.gameObject.transform.childCount;

        for(int i = 0; i < nActionableObjects; i++){
            Transform actionableObject = this.gameObject.transform.GetChild(i);
            if(actionableObject.CompareTag("Rock")){
                if(actionableObject.GetComponent<RockScript>().rockState == RockScript.state.FallingOntoPlatform){
                    return true;
                }
            }
        }
        return false;
    }

    void spawnRock(){

        int rockType = GetRandomWeightedIndex(rockProbabilities);
        float randSizeMultiplier = 1f + (Random.Range(-1f, 1f) * rockSizePercentVariation / 100f);
        float rockSize = randSizeMultiplier * rockSizes[rockType];
        

        float xPos = getNewRockXPos(rockSize * rockPrefab.GetComponent<BoxCollider2D>().size.x);

        //spawn a rock
        Transform spawnedRock = Instantiate(rockPrefab, new Vector3(xPos, spawnYPos), Quaternion.identity);

        spawnedRock.SetParent(this.gameObject.transform);

        spawnedRock.transform.localScale = new Vector2(rockSize, rockSize);

        Transform spawnedRockShadow;

        if(rockType == 0){
            spawnedRock.GetComponent<RockScript>().rockSize = RockScript.size.Small;
            spawnedRock.GetComponent<SpriteRenderer>().sprite = smallRockSprites[Random.Range(0, smallRockSprites.Count)];
            //spawn shadow
            spawnedRockShadow = Instantiate(rockShadowSmallPrefab, new Vector3(xPos, shadowYPos), Quaternion.identity);
        }else{
            spawnedRock.GetComponent<RockScript>().rockSize = RockScript.size.Big;
            spawnedRock.GetComponent<SpriteRenderer>().sprite = bigRockSprites[Random.Range(0, bigRockSprites.Count)];
            //spawn shadow
            spawnedRockShadow = Instantiate(rockShadowLargePrefab, new Vector3(xPos, shadowYPos), Quaternion.identity);
        }

        //adjust shadow size to rock size
        spawnedRockShadow.transform.localScale = new Vector2(rockSize, spawnedRockShadow.transform.localScale.y); 

        spawnedRockShadow.transform.SetParent(
            GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform
        );

        //reset z position for showing in front of platform
        spawnedRockShadow.transform.localPosition = new Vector3(spawnedRockShadow.transform.localPosition.x, spawnedRockShadow.transform.localPosition.y, 0f);

        //set shadow to start transparent
        Color shadowColour = spawnedRockShadow.GetComponent<SpriteRenderer>().color;
        shadowColour.a = 0f;
        spawnedRockShadow.GetComponent<SpriteRenderer>().color = shadowColour;
        
        spawnedRock.GetComponent<RockScript>().shadow = spawnedRockShadow;//pass reference of shadow to rock
        
        return;
    }


    //this function randomly decides where to spawn the next falling rock,
    //avoiding the rocks already in the level
    float getNewRockXPos(float newRockWidth){
        int nActionableObjects = this.gameObject.transform.childCount;

        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-5.0
        SortedList<float, float> rocks = new SortedList<float, float>();
        //SortedList for storing the rocks already in the level
        //keys = rock x pos, values = rock width

        for(int i = 0; i < nActionableObjects; i++){
            Transform actionableObject = this.gameObject.transform.GetChild(i);
            if(actionableObject.CompareTag("Rock")){
                if(actionableObject.GetComponent<RockScript>().rockState == RockScript.state.OnPlatform){
                    float rockXPos = actionableObject.position.x;
                    float rockWidth = actionableObject.localScale.x;
                    rocks.Add(rockXPos, rockWidth);
                }
            }
        }

        rocks.Add(platformLeftEdge, 0);  //add edges, represented as rocks with 0 width
        rocks.Add(platformRightEdge, 0);


        //now, calculate the segments of the platform between the rocks
        //with enough space to accomodate a new rock
        List<float> usableSegmentLeftEdge = new List<float>();
        List<float> usableSegmentRightEdge = new List<float>();
        List<float> usableSegmentLengths = new List<float>();
        for(int i = 0; i < rocks.Count-1; i++){ // for n rocks, there's n+1 segments between them
            //a usable segment starts at a rock y position + half it width and half the new rock width
            //it ends at the next rock's y position - half it width and half the new rock width
            usableSegmentLeftEdge.Add(rocks.Keys[i] + rocks.Values[i]/2f + newRockWidth/2f);
            usableSegmentRightEdge.Add(rocks.Keys[i+1] - rocks.Values[i+1]/2f - newRockWidth/2f);
            //the left and right edge might swap sides. This means there is no usable segment
            usableSegmentLengths.Add(Mathf.Max(usableSegmentRightEdge[i] - usableSegmentLeftEdge[i], 0));
        }

        if(sumList(usableSegmentLengths) > 0.01f){ //if usable segments are available
            //randomly pick a segment, weighted by it's length
            int segmentN = GetRandomWeightedIndex(usableSegmentLengths);
            //return a random position along it's length
            return Random.Range(usableSegmentLeftEdge[segmentN], usableSegmentRightEdge[segmentN]);
        }
        else{ //if no usable segments available
            //spawn a rock in a random position
            Debug.Log("no space on platform, spawnning rock at random position");
            return Random.Range(platformLeftEdge, platformRightEdge);
        }
    }

    float sumList(List<float> list){
        float listSum = 0f;
        foreach (float item in list){
            listSum += item;
        }
        return listSum;
    }


    //https://forum.unity.com/threads/random-numbers-with-a-weighted-chance.442190/
    int GetRandomWeightedIndex(List<float> weights){
        // Get the total sum of all the weights.
        float weightSum = sumList(weights);
        float randomN = Random.Range(0, weightSum);
    
        // Step through all the possibilities, one by one, checking to see if each one is selected.
        int i = 0;
        int lastIndex = weights.Count - 1;
        while (i < lastIndex){
            if (randomN < weights[i]){
                return i;
            }
            randomN -= weights[i];
            i++;
        }
    
        // No other item was selected, so return very last index.
        return i;
    }
}
