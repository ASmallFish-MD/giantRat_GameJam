using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioScript : MonoBehaviour
{
    // Start is called before the first frame update
    Transform beach;
    void Start(){
        beach = GameObject.FindGameObjectWithTag("TheBeach").transform;
    }

    // Update is called once per frame
    void Update(){
        if(beach.position.y < 7f){
            Destroy(gameObject);
        }
    }
}
