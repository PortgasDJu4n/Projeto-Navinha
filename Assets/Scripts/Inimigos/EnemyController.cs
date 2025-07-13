using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speedY = 2f;
    public float speedX = 1.5f;
    public int maxHealth = 3;

    public GameObject enemyBulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;
    public GameObject explosionPrefab;

    private int currentHealth;
    private int directionX;
    private float minX, maxX;

    private Transform spriteTransform;

    private void Start()
    {
        currentHealth = maxHealth;
        directionX = Random.value < 0.5f ? -1 : 1;

        Camera cam = Camera.main;
        Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        spriteTransform = transform.Find("EnemySprite");
        if (spriteTransform == null)
            Debug.LogError("Filho 'EnemySprite' não encontrado no inimigo!");

        float halfWidth = 0f;
        SpriteRenderer sr = spriteTransform != null ? spriteTransform.GetComponent<SpriteRenderer>() : null;
        if (sr != null)
            halfWidth = sr.bounds.size.x / 2f;

        minX = leftBottom.x + halfWidth;
        maxX = rightTop.x - halfWidth;
    }

    private void Update()
    {
        Move();
        HandleShooting();
        CheckOutOfBounds();
    }

    private void Move()
    {
        Vector3 pos = transform.position;
        pos.y -= speedY * Time.deltaTime;
        pos.x += directionX * speedX * Time.deltaTime;

        if (pos.x <= minX)
        {
            pos.x = minX;
            directionX = 1;
        }
        else if (pos.x >= maxX)
        {
            pos.x = maxX;
            directionX = -1;
        }

        transform.position = pos;
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
            Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void CheckOutOfBounds()
    {
        if (transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 1f)
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            ExplodeAndDie();
    }

    public void ExplodeAndDie()
    {
        if (explosionPrefab != null && spriteTransform != null)
        {
            Vector3 pos = spriteTransform.position;
            pos.z = 0f;
            Instantiate(explosionPrefab, pos, Quaternion.identity);
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
        if (collision.gameObject.CompareTag("Player"))
        {
            // O inimigo também explode e morre ao colidir com o player
            ExplodeAndDie();

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
                player.TakeDamage(1);
        }
    }
}
