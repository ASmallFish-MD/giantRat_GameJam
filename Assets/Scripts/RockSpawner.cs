using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour{
    private float platformLeftEdge;
    private float platformRightEdge;
    public float shadowYPos;

    public float rockSizePercentVariation = 15f;

    public List<float> rockSizes; //1,3
    public List<float> rockProbabilities; //50,50

    // Start is called before the first frame update
    void Start(){
        Transform platform = GameObject.FindGameObjectWithTag("Platform").transform;
        platformLeftEdge = platform.position.x - platform.localScale.x/2f;
        platformRightEdge = platform.position.x + platform.localScale.x/2f;
    }

    // Update is called once per frame
    void Update(){
        
    }

    void spawnRock(){

        int rockType = GetRandomWeightedIndex(rockProbabilities);
        float randSizeMultiplier = 1f + (Random.Range(-1, 1) * rockSizePercentVariation / 100f);
        float rockSize = randSizeMultiplier * rockSizes[rockType];
        

        float xPos = getNewRockXPos(rockSize);

        //TODO: spawn rock here

        if(rockType == 0){
            //GetComponent<RockScript>().rockSize = RockScript.size.Small;
        }else{
            //GetComponent<RockScript>().rockSize = RockScript.size.Big;
        }
        
        return;
    }


    //this function randomly decides where to spawn the next falling rock,
    //avoiding the rocks already in the level
    float getNewRockXPos(float newRockWidth){
        int nRocks = this.gameObject.transform.childCount;

        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-5.0
        SortedList<float, float> rocks = new SortedList<float, float>();
        //SortedList for storing the rocks already in the level
        //keys = rock x pos, values = rock width

        for(int i = 0; i < nRocks; i++){
            Transform rock = this.gameObject.transform.GetChild(i);
            if(rock.GetComponent<RockScript>().rockState == RockScript.state.OnPlatform){
                float rockXPos = rock.position.x;
                float rockWidth = rock.localScale.x;
                rocks.Add(rockXPos, rockWidth);
            }
        }

        rocks.Add(platformLeftEdge, 0);  //add edges, represented as rocks with 0 width
        rocks.Add(platformRightEdge, 0);


        //now, calculate the segments of the platform between the rocks
        //with enough space to accomodate a new rock
        List<float> usableSegmentLeftEdge = new List<float>();
        List<float> usableSegmentRightEdge = new List<float>();
        List<float> usableSegmentLengths = new List<float>();
        for(int i = 0; i < nRocks+1; i++){ // for n rocks, there's n+1 segments between them
            //a usable segment starts at a rock y position + half it width and half the new rock width
            //it ends at the next rock's y position - half it width and half the new rock width
            usableSegmentLeftEdge[i] = rocks.Keys[i] + rocks.Values[i]/2 + newRockWidth/2;
            usableSegmentRightEdge[i] = rocks.Keys[i+1] - rocks.Values[i+1]/2 - newRockWidth/2;
            //the left and right edge might swap sides. This means there is no usable segment
            usableSegmentLengths[i] = Mathf.Max(usableSegmentRightEdge[i] - usableSegmentLeftEdge[i], 0);
        }

        if(sumList(usableSegmentLengths) > 0.01){ //if usable segments are available
            //randomly pick a segment, weighted by it's length
            int segmentN = GetRandomWeightedIndex(usableSegmentLengths);
            //return a random position along it's length
            return Random.Range(usableSegmentLeftEdge[segmentN], usableSegmentRightEdge[segmentN]);
        }
        else{ //if no usable segments available
            //spawn a rock in a random position
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
