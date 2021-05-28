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

    public float BGtextureScale = 10f;

    void Start(){
        renderer = GetComponent<Renderer> ();
        //Maintain 1:1 aspect ratio
        renderer.sharedMaterial.SetTextureScale(
            "_MainTex",
            new Vector2(transform.localScale.x, transform.localScale.y) / BGtextureScale
        );
    }

    void Update(){
        //the offset works in the opposite direction as normal translate, so minus
        offset -= scrollDirection * Time.deltaTime * scrollSpeed / BGtextureScale;
        offset.x = Mathf.Repeat(offset.x, 1);
        offset.y = Mathf.Repeat(offset.y, 1);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}