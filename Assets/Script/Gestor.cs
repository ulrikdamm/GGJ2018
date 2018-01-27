using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gestor : MonoBehaviour {

    public static Gestor gestor;
    public Slider player001;

    public GameObject[] objeto;
    public Transform generadorObjeto;

	// Use this for initialization
	void Start () {
        gestor = this;
        InvokeRepeating("GenerarObjeto", 0, 3);
	}

    void GenerarObjeto()
    {
        Instantiate(objeto[Random.Range(0, objeto.Length)], generadorObjeto.position + new Vector3(Random.Range(-1.38f, 1.38f), Random.Range(-0.64f, 0.64f), 3.84f), Quaternion.identity);
    }
    
}
