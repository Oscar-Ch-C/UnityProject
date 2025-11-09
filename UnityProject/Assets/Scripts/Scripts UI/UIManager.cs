using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // 👉 Llamar desde botones con el nombre de la escena
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 👉 Llamar desde botones "Volver"
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Asegúrate que la escena del menú se llama así
    }

    // 👉 Botón de salir
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
