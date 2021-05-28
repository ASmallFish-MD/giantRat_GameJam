using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    public bool gameHasEnded = false;

    public void EndGame(string gameOverMessage){
        if(!gameHasEnded){
            gameHasEnded = true;
            Debug.Log("GameOver: " + gameOverMessage);
        }
    }
}
