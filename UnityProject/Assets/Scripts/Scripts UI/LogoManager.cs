using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour
{
    public float waitTime = 4.5f; // debe ser un poco más que la animación

    void Start()
    {
        // Cambié "LoadMenu" por "LoadNextScene"
        Invoke("LoadNextScene", waitTime);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("IntroScene"); // Nombre exacto de tu escena de intro
    }
}
