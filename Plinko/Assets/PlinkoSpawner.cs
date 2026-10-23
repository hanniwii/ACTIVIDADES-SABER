using UnityEngine;

public class PlinkoSpawner : MonoBehaviour
{
    public GameObject fichaPrefab;
    public float limiteX = 4f;
    public float alturaY = 4.5f;

    void Update()
    {
        // Hacer que el spawner siga la posición X del mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float posX = Mathf.Clamp(mousePos.x, -limiteX, limiteX);
        transform.position = new Vector3(posX, alturaY, 0);

        // Instanciar ficha al hacer clic izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(fichaPrefab, transform.position, Quaternion.identity);
        }
    }
}
