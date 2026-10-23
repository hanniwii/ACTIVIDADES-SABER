using UnityEngine;
using UnityEngine.InputSystem; // Unity 6: Input System nuevo

[RequireComponent(typeof(Rigidbody2D))]
public class playermovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 8f;          // Sube o baja este valor segun la altura que necesites

    [Header("Reaparicion")]
    public float fallLimitY = -10f;       // Si cae por debajo de esta altura, reaparece

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpRequested;
    private float moveInput;
    private bool canMove = true;
    private Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Sin rebote y sin friccion (evita que "se pegue" a plataformas y paredes)
        PhysicsMaterial2D mat = new PhysicsMaterial2D("SinRebote");
        mat.friction = 0f;
        mat.bounciness = 0f;
        rb.sharedMaterial = mat;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.sharedMaterial = mat;

        // Configuracion recomendada para este tipo de juego
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep; // para que el chequeo de suelo siempre funcione
    }

    void Update()
    {
        if (!canMove || Keyboard.current == null) { moveInput = 0f; return; }

        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput += 1f;

        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame ||
            Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }

        if (transform.position.y < fallLimitY) Respawn();
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            jumpRequested = false;
            isGrounded = false;
            return;
        }

        // Movimiento horizontal
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Salto: solo si esta realmente parado sobre algo
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        jumpRequested = false;

        // Se vuelve a calcular en los callbacks de colision de este paso de fisica
        isGrounded = false;
    }

    // Suelo = cualquier colision cuya normal apunte hacia ARRIBA.
    // Asi no cuenta tocar plataformas por debajo ni paredes, y funciona sobre la caja "Tierra".
    private void OnCollisionEnter2D(Collision2D collision) { CheckGround(collision); }
    private void OnCollisionStay2D(Collision2D collision) { CheckGround(collision); }

    private void CheckGround(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.7f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    // La llama la meta cuando ganas
    public void Win()
    {
        canMove = false;
        moveInput = 0f;
    }

    public void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
    }
}