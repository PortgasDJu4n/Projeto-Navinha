using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;
    public int maxHealth = 3;
    private int currentHealth;
    public GameObject explosionPrefab;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform spriteTransform;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = transform.Find("Sprite");
        if (spriteTransform == null)
            Debug.LogError("Filho 'Sprite' não encontrado no Player!");
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Jogador tomou dano! Vida restante: " + currentHealth);
        if (currentHealth <= 0)
            ExplodeAndDie();
    }

    private void ExplodeAndDie()
    {
        if (explosionPrefab != null && spriteTransform != null)
        {
            Vector3 pos = spriteTransform.position;
            pos.z = 0f;
            GameObject explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
            explosion.transform.localScale *= 1.5f;
        }

        if (spriteTransform != null)
        {
            SpriteRenderer sr = spriteTransform.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
        }

        foreach (var col in GetComponents<Collider2D>())
            col.enabled = false;

        enabled = false;
        Destroy(gameObject, 1.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            // O player leva dano e explode se morrer
            TakeDamage(1);

            // O inimigo explode e morre
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
                enemy.ExplodeAndDie();
        }
    }
}
