using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    
    public bool gameHasEnded = false;

    public List<Transform> deathMessages;

    public Transform gameOverBackground;

    AudioSource gameoverMusic;

    public void EndGame(string gameOverMessage){
        if(!gameHasEnded){
            gameHasEnded = true;
            Debug.Log("GameOver: " + gameOverMessage);
            gameOverBackground.gameObject.SetActive(true);
            if(gameOverMessage=="Your duck is lost forever in the depths"){
                deathMessages[0].gameObject.SetActive(true);
            }else if(gameOverMessage=="You have fallen"){
                deathMessages[1].gameObject.SetActive(true);
            }else if(gameOverMessage=="You have been crushed"){
                deathMessages[2].gameObject.SetActive(true);
            }else if(gameOverMessage=="Your duck has been crushed"){
                deathMessages[3].gameObject.SetActive(true);
            }else if(gameOverMessage=="Your platform has suffered a catastrophic failure. Proboable cause: Rocks"){
                deathMessages[4].gameObject.SetActive(true);
            }

            gameoverMusic.enabled = true;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Start() {
        gameoverMusic = GetComponent<AudioSource>();
    }
}






