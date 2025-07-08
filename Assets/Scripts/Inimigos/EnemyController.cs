using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speedY = 2f;
    public float speedX = 1.5f;
    public int maxHealth = 3;

    public GameObject enemyBulletPrefab; // Prefab da bala do inimigo
    public Transform firePoint;          // Ponto de disparo da bala

    public float fireRate = 2f;          // Intervalo entre disparos
    private float nextFireTime = 0f;

    private int currentHealth;
    private int directionX;

    private float minX, maxX;

    private void Start()
    {
        currentHealth = maxHealth;
        directionX = Random.value < 0.5f ? -1 : 1;

        // Calcula os limites da tela
        Camera cam = Camera.main;
        Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        float halfWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2f;

        minX = leftBottom.x + halfWidth;
        maxX = rightTop.x - halfWidth;
    }

    private void Update()
    {
        // Movimento
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

        // Tiro
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }

        // Destroi se sair da tela
        if (pos.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - 1f)
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
