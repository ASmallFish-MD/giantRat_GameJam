using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour{
    public Transform GrabSlot;

    public float actionRange = 1.5f;

    public Animator animator;

    public enum actionType{
        Nothing,
        Dropping,
        PickingUp,
        UsePickaxe
    }

    // Start is called before the first frame update
    void Start(){
        GrabSlot = transform.Find("GrabSlot");
        if(GrabSlot == null){
            Debug.LogError("The Player has no GrabSlot. D:");
        }
    }

    public actionType performPlayerAction(){
        bool holding = GrabSlot.childCount > 0;
        Transform actionableObject = null;
        actionType returnAction = actionType.Nothing;

        if(holding){
            actionableObject = GrabSlot.GetChild(0);
            animator.SetBool("isHolding", false);
        }else{
            actionableObject = getClosestObject();
            animator.SetBool("isHolding", true);
        }

        if(actionableObject == null){
            animator.SetBool("isHolding", false);
            return actionType.Nothing;
        }

        if(actionableObject.CompareTag("Rock")){
            RockScript rockScript = actionableObject.GetComponent<RockScript>();
            if(rockScript == null){
                Debug.LogError("Garry, get in here, we found a Rock with no RockScript!");
                return actionType.Nothing;
            }else{
                if(rockScript.rockSize == RockScript.size.Big){
                    animator.SetBool("isHolding", false);
                    returnAction = actionType.UsePickaxe;
                }else if(rockScript.rockSize == RockScript.size.Small){
                    if(!holding){
                        returnAction = actionType.PickingUp;
                    }else{
                        returnAction = actionType.Dropping;
                    }
                }
                rockScript.reactToPlayerAction();
            }
        }else if(actionableObject.CompareTag("Duck")){
            DuckScript duckScript = actionableObject.GetComponent<DuckScript>();
            if(duckScript == null){
                Debug.LogError("Garry, get in here, we found a Duck with no DuckScript!");
                return actionType.Nothing;
            }else{
                if(!holding){
                    returnAction = actionType.PickingUp;
                }else{
                    returnAction = actionType.Dropping;
                }
                duckScript.reactToPlayerAction();
            }
        }

        return returnAction;
    }

    private Transform getClosestObject(){
        Transform actionableObjectsTransform = GameObject.FindGameObjectWithTag("ActionableObjectsTransform").transform;

        //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.sortedlist-2?view=net-5.0
        SortedList<float, Transform> actionableObjects = new SortedList<float, Transform>();
        //SortedList for storing the objects already in the level
        //keys = object x pos, values = object transform

        //make a list of objects on the platform to perform actions on
        int nActionableObjects = actionableObjectsTransform.childCount;
        for(int i = 0; i < nActionableObjects; i++){
            Transform actionableObject = actionableObjectsTransform.GetChild(i);
            if(actionableObject.CompareTag("Rock") && actionableObject.GetComponent<RockScript>().rockState == RockScript.state.OnPlatform
                || actionableObject.CompareTag("Duck") && actionableObject.GetComponent<DuckScript>().duckState == DuckScript.state.OnPlatform){
                
                if(!actionableObjects.ContainsKey(actionableObject.position.x)){ //stored list cant containt 2 items with same key, ignore if 2 objects with same x pos
                    actionableObjects.Add(actionableObject.position.x, actionableObject);
                    Debug.Log(actionableObject);
                }
            }
        }

        bool facingRight = Mathf.Sign(this.gameObject.transform.localScale.x) > 0; //TODO: get facing right from another source
        float plrXPos = this.gameObject.transform.position.x;

        Transform closestObject = null;

        //linear search through actionable object list to find closest one
        if(facingRight){
            for (int i = 0; i < actionableObjects.Count; i++){
                if(plrXPos <= actionableObjects.Keys[i] && actionableObjects.Keys[i] <= plrXPos + actionRange){
                    closestObject = actionableObjects.Values[i];
                }
            }
        }else{ //if facing left, search in the reverse order
            for (int i = 0; i < actionableObjects.Count; i++){
                if(plrXPos - actionRange <= actionableObjects.Keys[actionableObjects.Count-i-1] && actionableObjects.Keys[actionableObjects.Count-i-1] <= plrXPos){
                    closestObject = actionableObjects.Values[actionableObjects.Count-i-1];
                }
            }
        }
        
        return closestObject;
    }
}
