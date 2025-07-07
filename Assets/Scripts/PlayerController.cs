using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public GameObject bulletPrefab; // Prefab da bala
    public Transform firePoint;     // Ponto de disparo (objeto vazio filho da nave)

    public float fireRate = 0.25f;  // Tempo entre tiros
    private float nextFireTime = 0f;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float halfWidth;
    private float halfHeight;

    private void Start()
    {
        Camera cam = Camera.main;

        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minBounds = bottomLeft;
        maxBounds = topRight;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
            halfHeight = sr.bounds.size.y / 2f;
        }
        else
        {
            halfWidth = 0.5f;
            halfHeight = 0.5f;
        }
    }

    private void Update()
    {
        // Movimento do jogador
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Limita posição na tela
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        // Disparo ao pressionar espaço, respeitando o tempo de fireRate
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }
}
