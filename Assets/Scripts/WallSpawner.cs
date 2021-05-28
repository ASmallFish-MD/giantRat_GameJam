using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour {
    public float scrollSpeed;
    public Vector2 scrollDirection = Vector2.down;

    public float offScreenYPos = -10;


    void Start(){

    }

    // Update is called once per frame
    void Update() {
        //TODO: getting all walls each fram may be inefficient...
        int nWalls = this.gameObject.transform.childCount;
        for(int i = 0; i < nWalls; i++){
            Transform wall = this.gameObject.transform.GetChild(i);
            wall.Translate(scrollDirection * Time.deltaTime * scrollSpeed);
            if(wall.transform.position.y < offScreenYPos){
                //change sprite and teleport up
                //TODO
            }
        }
    }
}
