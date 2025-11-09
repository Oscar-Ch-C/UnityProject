using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 5f;
    public float detectionRange = 5f; // Rango para detectar al jugador
    public Transform player; // Referencia al jugador
    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar si el jugador está cerca
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            FollowPlayer();
        }
        else
        {
            StopMoving();
        }

        // Revisar si está en el suelo para saltar
        CheckGrounded();

        // Animación de caminar
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // Controla la animación de caminar
    }

    void FollowPlayer()
    {
        // Movimiento horizontal
        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        // Animación: Flip horizontal dependiendo de la dirección
        if (direction > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        else if (direction < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        // Saltar si es necesario (solo si está cerca del jugador y puede saltar)
        if (isGrounded && Random.value < 0.01f) // Probabilidad de salto aleatorio
        {
            Jump();
        }
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void CheckGrounded()
    {
        // Verificar si el enemigo está en el suelo (usando un raycast o colisión)
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
    }
}
