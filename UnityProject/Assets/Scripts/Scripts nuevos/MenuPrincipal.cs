using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void IniciarJuego()
    {
        SceneManager.LoadScene("Nivel2_Castillo");
    }
    

    public void Salir()
    {
        Application.Quit();
    }
}
