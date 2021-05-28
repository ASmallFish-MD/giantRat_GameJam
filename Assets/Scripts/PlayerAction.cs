using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour{
    public Transform GrabSlot;

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public actionType performPlayerAction(){
        bool holding = GrabSlot.childCount > 0;
        Transform actionableObject = null;
        actionType returnAction = actionType.Nothing;

        if(holding){
            actionableObject = GrabSlot.GetChild(0);
        }else{
            //TODO actionableObject = getClosestObject();
        }

        if(actionableObject.CompareTag("Rock")){
            RockScript rockScript = actionableObject.GetComponent<RockScript>();
            if(rockScript == null){
                Debug.LogError("Garry, get in here, we found a Rock with no RockScript!");
                return actionType.Nothing;
            }else{
                if(rockScript.rockSize == RockScript.size.Big){
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
}
