using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController01 : MonoBehaviour {

    public int speed;
    Rigidbody2D rig;
    public Boundary limite;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (transform.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (-transform.up * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (transform.right * speed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.right * speed * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        rig.position = new Vector2(Mathf.Clamp(rig.position.x, limite.xMin, limite.xMax), Mathf.Clamp(rig.position.y, limite.yMin, limite.yMax));

        if (Input.GetKey(KeyCode.LeftControl))
        {

        }


    }
}
