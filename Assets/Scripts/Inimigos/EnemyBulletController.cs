using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 7f;

    void Update()
    {
        // Move para baixo
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destroi se sair da tela
        if (transform.position.y < -6f) // ajuste o valor conforme sua câmera
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se atingiu o jogador
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }

            Destroy(gameObject); // destrói a bala
        }
    }
}
