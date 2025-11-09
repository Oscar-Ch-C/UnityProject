using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("Salud")]
    public int vida = 3;

    [Header("Físicas")]
    public float fuerzaRebote = 6f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private bool recibiendoDanio;
    public bool muerto;

    private Rigidbody2D rb;
    public Animator animator;


    public VidaUI vidaUI;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaUI.ActualizarVidas(vida);
    }

    void Update()
    {
        if (muerto) return;

        // Detectar suelo
        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);

        // Movimiento horizontal
        float inputX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputX * velocidad, rb.velocity.y);
        FlipSprite(inputX);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }

        // Actualizar animaciones
        animator.SetBool("run", inputX != 0 && enSuelo);
        animator.SetBool("saltar", !enSuelo);
        animator.SetBool("dead", muerto);
        // Idle se maneja automáticamente
    }

    private void FlipSprite(float inputX)
    {
        if (inputX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (inputX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }


    public float tiempoInvulnerable = 0.5f; // medio segundo

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (recibiendoDanio || muerto) return;

        recibiendoDanio = true;
        vida -= cantDanio;

        if (vidaUI != null)
            vidaUI.ActualizarVidas(vida);

        if (vida <= 0)
        {
            muerto = true;
            rb.velocity = Vector2.zero;
            animator.SetBool("dead", true);
            if (GameManager.instance != null)
                GameManager.instance.GameOver();
        }
        else
        {
            Vector2 rebote = (transform.position - (Vector3)direccion).normalized;
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);

            // Desactiva daño después de un tiempo
            Invoke(nameof(DesactivaDanio), tiempoInvulnerable);
        }
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
    }


    // Detectar colisiones con enemigos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direccionImpacto = collision.transform.position;
            RecibeDanio(direccionImpacto, 1); // Quita 1 de vida
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
