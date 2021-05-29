using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachAudioScript : MonoBehaviour
{
    Transform beach;
    AudioSource beachMusic;
    void Start(){
        beach = GameObject.FindGameObjectWithTag("TheBeach").transform;
        beachMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(beach.position.y < 7f){
            beachMusic.enabled = true;
        }
    }
}
