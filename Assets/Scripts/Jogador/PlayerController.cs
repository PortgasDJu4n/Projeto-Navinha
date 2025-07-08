using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public GameObject bulletPrefab;      // Prefab da bala do jogador
    public Transform firePoint;          // Ponto de onde sai o tiro

    public float fireRate = 0.25f;       // Intervalo entre tiros em segundos
    private float nextFireTime = 0f;

    public int maxHealth = 3;
    private int currentHealth;

    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Captura input de movimento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        // Tiro automático a cada fireRate segundos
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void FixedUpdate()
    {
        // Move o jogador usando Rigidbody2D para colisões funcionarem corretamente
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jogador tomou dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Jogador morreu!");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}
