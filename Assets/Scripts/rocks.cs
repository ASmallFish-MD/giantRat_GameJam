


public float platformLeftEdge;
public float platformRightEdge;

public float maxRockSize;
public float minRockSize;

public int nRockTypes;


void spawnRock(){
    //make smaller rocks more common by changing the random distribution
    //by raising it to a power. Increase power to get larger rocks to be less common
    //e.g. with 3 rock types, type 1 = 87% chance, 2 = 18%, 3 = 13%
    float rand = Random.Range(0, 1) ** 3;
    float rockSize = minRockSize + (maxRockSize - minRockSize) * rand;
    //limit the range of rock types, since random.range is inclusive with limits
    int rockType = Mathf.Min(rand * nRockTypes, nRockTypes);
    float xPos = getNewRockXPos(rockSize);

    //TODO: spawn rock here
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
        GameObject rock = this.gameObject.transform.GetChild(i);
        float rockXPos = rock.transform.position.x;
        float rockWidth = rock.transform.localScale.x;
        rocks.add(rockXPos, rockWidth);
    }

    rocks.add(platformLeftEdge, 0);  //add edges, represented as rocks with 0 width
    rocks.add(platformRightEdge, 0);


    //now, calculate the segments of the platform between the rocks
    //with enough space to accomodate a new rock
    List<float> usableSegmentLeftEdge = new List<float>();
    List<float> usableSegmentRightEdge = new List<float>();
    List<float> usableSegmentLengths = new List<float>();
    for(int i = 0; i < nRocks+1; i++){ // for n rocks, there's n+1 segments between them
        //a usable segment starts at a rock y position + half it width and half the new rock width
        //it ends at the next rock's y position - half it width and half the new rock width
        usableSegmentLeftEdge[i] = rocks.keys[i] + rocks.values[i]/2 + newRockWidth/2;
        usableSegmentRightEdge[i] = rocks.keys[i+1] - rocks.values[i+1]/2 - newRockWidth/2;
        //the left and right edge might swap sides. This means there is no usable segment
        usableSegmentLengths[i] = Mathf.Max(usableSegmentRightEdge[i] - usableSegmentLeftEdge[i], 0);
    }

    if(sumList(usableSegmentLengths) > 0.01){ //if usable segments are available
        //randomly pick a segment, weighted by it's length
        int segmentN = GetRandomWeightedIndex(usableSegmentLengths);
        //return a random position along it's length
        return Random.Random(usableSegmentLeftEdge[segmentN], usableSegmentRightEdge[segmentN]);
    }
    else{ //if no usable segments available
        //spawn a rock in a random position
        return Random.Range(platformLeftEdge, platformRightEdge);
    }
}

int sumList(List<float> list){
    float listSum = 0f;
    foreach (float item in weights){
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