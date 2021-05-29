using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour{

    public float platformDistance = 0f;
    public float platformSpeed = 1.2f;

    public float platformMaxSpeed = 1.2f;

    public float smallRockWeight = 1f;
    public float bigRockWeight = 2f;

    public float platformMinSpeed = 0.5f;
    public float minSpeedWeight = 10f;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        platformSpeed = Mathf.Lerp(platformMaxSpeed, platformMinSpeed, getPlatformWeight()/minSpeedWeight);
    }

    private float getPlatformWeight(){
        float totalWeight = 0;

        Transform actionableObjectsTransform = GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform;

        //make a list of objects on the platform to perform actions on
        int nActionableObjects = actionableObjectsTransform.childCount;
        for(int i = 0; i < nActionableObjects; i++){
            Transform actionableObject = actionableObjectsTransform.GetChild(i);
            if(actionableObject.CompareTag("Rock")){
                if(actionableObject.GetComponent<RockScript>().rockSize == RockScript.size.Small){
                    totalWeight += smallRockWeight;
                }else if(actionableObject.GetComponent<RockScript>().rockSize == RockScript.size.Big){
                    totalWeight += bigRockWeight;
                }
            }
        }
        return totalWeight;
    }
}
