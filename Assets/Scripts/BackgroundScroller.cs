using UnityEngine;
using System.Collections.Generic;

public class BackgroundScroller : MonoBehaviour
{
    public GameObject backgroundPrefab;  // Prefab do fundo
    public float scrollSpeed = 2f;

    private float spriteHeight;
    private List<GameObject> backgrounds = new List<GameObject>();
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        spriteHeight = backgroundPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        // Centraliza no centro da câmera
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);

        // Instancia dois fundos, um no centro e outro logo acima
        GameObject bg1 = Instantiate(backgroundPrefab, transform.position, Quaternion.identity, transform);
        backgrounds.Add(bg1);

        Vector3 pos2 = new Vector3(transform.position.x, transform.position.y + spriteHeight, transform.position.z);
        GameObject bg2 = Instantiate(backgroundPrefab, pos2, Quaternion.identity, transform);
        backgrounds.Add(bg2);
    }

    private void Update()
    {
        if (backgrounds.Count == 0)
            return;

        // Move todos os fundos para baixo
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
        }

        GameObject bgLowest = backgrounds[0];
        float cameraBottom = cam.transform.position.y - cam.orthographicSize;

        // Agora, verifica se o fundo mais baixo passou metade para fora da tela
        float triggerY = cameraBottom + (spriteHeight / 2);

        if (bgLowest.transform.position.y + spriteHeight < triggerY)
        {
            // Remove e destrói o fundo que saiu da tela
            Destroy(bgLowest);
            backgrounds.RemoveAt(0);

            if (backgrounds.Count == 0)
                return;

            GameObject bgHighest = backgrounds[backgrounds.Count - 1];
            Vector3 newPos = new Vector3(bgHighest.transform.position.x, bgHighest.transform.position.y + spriteHeight, bgHighest.transform.position.z);
            GameObject newBg = Instantiate(backgroundPrefab, newPos, Quaternion.identity, transform);
            backgrounds.Add(newBg);
        }
    }
}
