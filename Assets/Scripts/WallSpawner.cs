using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour {
    public float scrollSpeed;
    public Vector2 scrollDirection = Vector2.down;

    public float offScreenYPos = -10f;

    public List<Sprite> wallSprites;
    public List<Sprite> oceanWallSprites;
    public Transform wallPrefab;

    public float wallXPos = 8.5f;

    public float generateWallUntillY = 10f;

    public float maxYofWallGenerated;


    Transform beach;

    void Start(){
        maxYofWallGenerated = -10f;
        beach = GameObject.FindGameObjectWithTag("TheBeach").transform;
    }

    // Update is called once per frame
    void Update() {
        //TODO: getting all walls each fram may be inefficient...
        int nWalls = this.gameObject.transform.childCount;
        Vector2 scrollAmount = scrollDirection * Time.deltaTime * scrollSpeed;
        maxYofWallGenerated += scrollAmount.y;
        for(int i = 0; i < nWalls; i++){
            Transform wall = this.gameObject.transform.GetChild(i);
            wall.Translate(scrollAmount);
            if(wall.transform.position.y < offScreenYPos){
                Object.Destroy(wall.gameObject);
            }
        }
        if(maxYofWallGenerated < generateWallUntillY){
            spawnWallPair();
        }
    }

    void spawnWallPair(){
        Transform spawnedWall;

        for(int i = -1; i < 2; i += 2){
            spawnedWall = Instantiate(wallPrefab, new Vector3(i * wallXPos, maxYofWallGenerated), Quaternion.identity);
            spawnedWall.SetParent(transform);

            float chanceOfOceanTile = Mathf.Lerp(0f, 0.97f, Mathf.Pow(Mathf.InverseLerp(20f, 140f, 150f-beach.position.y), 2f));
            Sprite wallSprite;
            if(Random.value < chanceOfOceanTile){
                wallSprite = oceanWallSprites[Random.Range(0, oceanWallSprites.Count)];
            }else{
                wallSprite = wallSprites[Random.Range(0, wallSprites.Count)];
            }

            spawnedWall.GetComponent<SpriteRenderer>().sprite = wallSprite;

            if(i > 0){
                spawnedWall.GetComponent<SpriteRenderer>().flipX = true;

                maxYofWallGenerated += spawnedWall.GetComponent<SpriteRenderer>().size.y;
            }
        }
    }
}