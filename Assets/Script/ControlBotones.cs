using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBotones : MonoBehaviour {

	
	public void Botones(int numeroEscena)
    {
        SceneManager.LoadScene(numeroEscena);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
