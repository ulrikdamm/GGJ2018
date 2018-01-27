using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energia : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        { 
        Destroy(gameObject);
        Gestor.gestor.player001.value += 1;
        }
    }
}
