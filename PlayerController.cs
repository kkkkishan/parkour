using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpForce = 6f;
    public float gravityMultiplier = 2f;
    public int maxJumps = 1;
    public float boostMultiplier = 2f;
    public float boostDuration = 1.5f;

    Rigidbody rb;
    int jumpsRemaining;
    bool isGrounded;
    float currentSpeedMultiplier = 1f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        jumpsRemaining = maxJumps;
    }

    void Update() {
        Move();
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0) {
            Jump();
        }
    }

    void Move() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 camForward = Camera.main ? Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized : Vector3.forward;
        Vector3 camRight = Camera.main ? Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized : Vector3.right;
        Vector3 desired = (camForward * v + camRight * h).normalized;
        Vector3 vel = desired * moveSpeed * currentSpeedMultiplier;
        Vector3 newVel = new Vector3(vel.x, rb.velocity.y, vel.z);
        rb.velocity = newVel;
        // simple grounded check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        if (isGrounded) jumpsRemaining = maxJumps;
        // extra gravity for snappier falls
        if (rb.velocity.y < 0) rb.velocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
    }

    void Jump() {
        Vector3 v = rb.velocity;
        v.y = jumpForce;
        rb.velocity = v;
        jumpsRemaining--;
    }

    public void ApplyBoost(float multiplier, float duration) {
        StopAllCoroutines();
        StartCoroutine(BoostRoutine(multiplier, duration));
    }

    IEnumerator BoostRoutine(float multiplier, float duration) {
        currentSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        currentSpeedMultiplier = 1f;
    }

    // helper for other systems
    public Vector3 GetForwardDirection() {
        return Camera.main ? Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized : transform.forward;
    }
}
