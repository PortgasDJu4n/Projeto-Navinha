using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Move a bala para cima no espaço mundial (eixo Y)
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}
