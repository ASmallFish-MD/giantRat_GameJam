using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code from
//https://gist.github.com/satanas/5766d9d9d34f94be25cb0f85ffc50ad1
public class BackgroundScroller : MonoBehaviour {
    public float scrollSpeed;
    public Vector2 scrollDirection;
    private Renderer renderer;
    private Vector2 offset = new Vector2(0, 0);

    void Start(){
        renderer = GetComponent<Renderer> ();
    }

    void Update(){
        offset += scrollDirection * Time.deltaTime * scrollSpeed;
        offset.x = Mathf.Repeat(offset.x, 1);
        offset.y = Mathf.Repeat(offset.y, 1);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}