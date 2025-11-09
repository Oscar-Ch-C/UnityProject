using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barravida : MonoBehaviour
{

    public Image rellenoBarraVida;
    private PlayerController playerController;
    private float vidaMaxima;
    // Start is called before the first frame update
    void Start()
    {
        playerController=GameObject.FindGameObjectWithTag("Aster").GetComponent<PlayerController>();
        vidaMaxima=playerController.vida;
    }

    // Update is called once per frame
    void Update()
    {
        rellenoBarraVida.fillAmount=playerController.vida/vidaMaxima;
    }
}
