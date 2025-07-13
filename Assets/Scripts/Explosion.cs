using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float lifeTime = 1.5f; // tempo total que a animação deve durar

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Obtém a duração original do primeiro clip
            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
            float originalDuration = clip.length;

            // Calcula a velocidade proporcional para que a animação dure o lifeTime
            float newSpeed = originalDuration / lifeTime;
            animator.speed = newSpeed;
        }

        // Destroi o objeto após o tempo total desejado
        Destroy(gameObject, lifeTime);
    }
}
