using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;
    public Button menuButton;

    private bool gameOverActivo = false;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
     if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
     if(reiniciarButton != null)
        {
            reiniciarButton.onClick.AddListener(ReiniciarEscena);
            
        }
     if(menuButton != null)
        {
            menuButton.onClick.AddListener(IrAlMenu);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarEscena();
        }
        if(Input.GetKeyDown(KeyCode.Escape)|| Input.GetKeyDown(KeyCode.M))
        {
            IrAlMenu();
        }

    }

    public void GameOver()
    {
        if(gameOverActivo) return;
        {
            gameOverActivo = true;
            
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if(gameOverText != null)
        {
            gameOverText.text = "Game Over \n\n Reiniciar";
        }
    }

    public void ReiniciarEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void IrAlMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
