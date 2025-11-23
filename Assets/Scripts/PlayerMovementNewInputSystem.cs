using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovementNewInputSystem : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Range(1, 10)]
    [SerializeField] private float moveSpeed; // Velocidad horizontal
    [Range(1, 10)]
    [SerializeField] private float jumpForce; // Fuerza del salto

    [Space]
    public TMP_Text inputDeviceText; // Texto para mostrar si se usa teclado o gamepad

    // Inputs
    private Vector2 movementInput; // Valor de movimiento horizontal y vertical
    private bool jumpInput; // Detecta salto
    private bool dashInput; // Detecta dash (ahora asignado al botón de Attack)
    [HideInInspector] public bool interactInput; // Detecta interacción con cofres

    // Acciones del sistema de Input
    private InputSystem_Actions playerControls;

    // Componentes
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    private GameObject playerFeet; // Punto para raycast de suelo
    private Animator animator;

    // Layer de suelo y distancia de raycast
    private LayerMask groundLayer;
    private float rayDistance = 0.5f;
    private bool wasGrounded = true;

    // Salto doble
    private int jumpCount = 0;
    private int maxJumps = 1;

    // Configuración del Dash
    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashCooldown = 1f;

    private bool isDashing = false; // Controla si actualmente está haciendo dash
    private bool canDash = true; // Controla si se puede dash

    // Flag para ejecutar el salto en FixedUpdate
    private bool performJump = false;

    private void Awake()
    {
        // Obtener componentes necesarios
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        playerFeet = GameObject.Find("PlayerFeet");
        animator = GetComponentInChildren<Animator>();

        // Inicializar sistema de Input
        playerControls = new InputSystem_Actions();
        playerControls.Enable();

        // Mostrar dispositivo actual
        UpdateDeviceText();
    }

    private void Update()
    {
        GetInputs(); // Leer inputs
        MovePlayer(); // Movimiento horizontal
        FlipSprite(); // Voltear sprite según dirección
        CalculateJump(); // Control de salto y doble salto
        UpdateDeviceText(); // Actualiza texto de dispositivo
        InteractChest(); // Detectar input de interactuar

        // Ejecutar dash si se presiona botón y está disponible
        if (dashInput && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        Jump(); // Ejecutar salto en FixedUpdate para física
    }

    private void OnDrawGizmos()
    {
        // Dibujar raycast hacia abajo desde los pies para depuración
        Gizmos.color = Color.red;
        if (playerFeet != null)
        {
            Vector3 start = playerFeet.transform.position;
            Vector3 end = start + Vector3.down * rayDistance;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }

    // ---------------- INPUTS ----------------
    private void GetInputs()
    {
        movementInput = playerControls.Player.Move.ReadValue<Vector2>(); // Lectura del movimiento
        jumpInput = playerControls.Player.Jump.triggered; // Lectura del salto
        dashInput = playerControls.Player.Attack.triggered; // Dash asignado al botón Attack
        interactInput = playerControls.Player.Interact.triggered; // Interactuar con cofres
    }

    // ---------------- MOVIMIENTO ----------------
    private void MovePlayer()
    {
        if (isDashing) return; // Bloquear movimiento durante dash

        transform.position += Vector3.right * movementInput.x * moveSpeed * Time.deltaTime;
        animator.SetFloat("Speed", Mathf.Abs(movementInput.x)); // Actualizar animación
    }

    private void FlipSprite()
    {
        // Voltear sprite según dirección de movimiento
        if (movementInput.x > 0) playerSprite.flipX = false;
        else if (movementInput.x < 0) playerSprite.flipX = true;
    }

    private bool IsGrounded()
    {
        // Raycast hacia abajo para comprobar si el jugador está en el suelo
        return Physics2D.Raycast(playerFeet.transform.position, Vector3.down, rayDistance, groundLayer);
    }

    // ---------------- SALTO ----------------
    private void CalculateJump()
    {
        bool grounded = IsGrounded();

        if (grounded)
            jumpCount = 0; // Reset de saltos cuando toca suelo

        if (jumpInput && jumpCount < maxJumps)
        {
            performJump = true;
            jumpCount++;
            animator.SetBool("IsJumping", true);
        }

        if (grounded && !wasGrounded)
            animator.SetBool("IsJumping", false);

        wasGrounded = grounded;
    }

    private void Jump()
    {
        if (isDashing) return; // No saltar durante dash

        if (performJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // Aplicar salto
            performJump = false;
        }
    }

    // ---------------- DASH ----------------
    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        animator.SetBool("IsDashing", true);

        float dashTime = 0.2f;
        float elapsed = 0f;
        float direction = playerSprite.flipX ? -1f : 1f; // Dirección del dash

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // Ignorar gravedad durante dash

        while (elapsed < dashTime)
        {
            rb.linearVelocity = new Vector2(direction * dashDistance / dashTime, 0f);

            // Detener dash si colisiona con pared
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * direction, 0.1f, groundLayer);
            if (hit.collider != null) break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;

        animator.SetBool("IsDashing", false);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // ---------------- OTROS ----------------
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void UpdateDeviceText()
    {
        // Detectar si se usa teclado o gamepad
        if (Keyboard.current != null && Keyboard.current.anyKey.isPressed)
            inputDeviceText.text = "Using Keyboard";
        else if (Gamepad.current != null && Gamepad.current.leftStick.ReadValue().magnitude > 0)
            inputDeviceText.text = "Using Gamepad";
    }

    private void InteractChest()
    {
        interactInput = playerControls.Player.Interact.ReadValue<float>() > 0.5f;
    }
}
