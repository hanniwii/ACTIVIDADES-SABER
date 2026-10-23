using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                  // Arrastra aqui al Player
    public float smoothTime = 0.15f;
    public Vector2 offset = new Vector2(3f, 1.5f); // Adelanta la camara hacia la derecha
    public bool followY = true;
    public float minX = -4.28f;               // La camara no se va mas a la izquierda que esto

    private Vector3 velocity;

    void LateUpdate()
    {
        if (target == null) return;

        float targetX = Mathf.Max(target.position.x + offset.x, minX);
        float targetY = followY ? target.position.y + offset.y : transform.position.y;

        Vector3 desired = new Vector3(targetX, targetY, transform.position.z); // conserva Z = -10
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}
