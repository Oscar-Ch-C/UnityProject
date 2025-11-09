using UnityEngine;

public class EnemigoAI : MonoBehaviour
{
    [Header("Referencias")]
    public Transform targetJugador; // Arrastra al jugador (Aster) aquí

    [Header("Stats de Movimiento")]
    public float velocidad = 3f;
    public float rangoDeteccion = 10f;

    [Header("Stats de Combate")]
    public int danio = 1;
    public float cooldownAtaque = 1.5f; // Tiempo entre ataques
    private float tiempoSiguienteAtaque = 0f;

    private Rigidbody2D rb;
    private Vector3 escalaOriginal; // <-- 1. VARIABLE NUEVA

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaOriginal = transform.localScale; // <-- 2. LÍNEA NUEVA

        // Si no asignamos al jugador, lo busca por Tag
        if (targetJugador == null)
        {
            targetJugador = GameObject.FindGameObjectWithTag("Aster").transform;
        }
    }

    void Update()
    {
        if (targetJugador == null)
        {
            // Si no hay jugador (ej. murió y se destruyó), no hace nada
            rb.velocity = Vector2.zero;
            return;
        }

        // --- Lógica de Seguimiento ---
        float distancia = Vector2.Distance(transform.position, targetJugador.position);

        // Solo sigue al jugador si está dentro del rango de detección
        if (distancia <= rangoDeteccion)
        {
            // Calcula la dirección hacia el jugador
            Vector2 direccion = (targetJugador.position - transform.position).normalized;

            // Mueve el enemigo usando su Rigidbody (solo en eje X)
            rb.velocity = new Vector2(direccion.x * velocidad, rb.velocity.y);

            // --- 3. LÓGICA DE VOLTEO CORREGIDA ---
            if (direccion.x > 0)
            {
                // Mirando derecha (usa la X original positiva)
                transform.localScale = new Vector3(Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
            else if (direccion.x < 0)
            {
                // Mirando izquierda (usa la X original negativa)
                transform.localScale = new Vector3(-Mathf.Abs(escalaOriginal.x), escalaOriginal.y, escalaOriginal.z);
            }
        }
        else
        {
            // Si el jugador está muy lejos, el enemigo se queda quieto
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // --- Lógica de Ataque (al tocar) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Revisa si choca con el jugador Y si ya pasó el cooldown
        if (collision.gameObject.CompareTag("Aster") && Time.time > tiempoSiguienteAtaque)
        {
            // Resetea el cooldown
            tiempoSiguienteAtaque = Time.time + cooldownAtaque;

            // Busca el script del jugador para hacerle daño
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Calcula la dirección del rebote (desde el enemigo hacia el jugador)
                Vector2 direccionRebote = (collision.transform.position - transform.position).normalized;

                // Llama a la función pública del jugador para que reciba daño
                player.RecibeDanio(direccionRebote, danio);
            }
        }
    }
}