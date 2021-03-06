﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController02 : MonoBehaviour {

    public int speed;
    Rigidbody2D rig;
    public Boundary limite;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += (transform.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += (-transform.up * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += (transform.right * speed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += (transform.right * speed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        rig.position = new Vector2(Mathf.Clamp(rig.position.x, limite.xMin, limite.xMax), Mathf.Clamp(rig.position.y, limite.yMin, limite.yMax));

        if (Input.GetKey(KeyCode.RightControl))
        {
            
        }
    
	}
}
