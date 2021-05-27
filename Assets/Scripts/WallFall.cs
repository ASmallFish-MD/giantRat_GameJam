using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFall : MonoBehaviour
{
    private bool doFall = false;
    private float fallSpeed = 0.1f;
    private Vector3 fallVector;

    private CharacterController controller;


    void Start()
    {
        fallVector = new Vector3(0, -fallSpeed, 0);
        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doFall) {
            controller.Move(fallVector);
        }
    }
}
