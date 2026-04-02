using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    [SerializeField] Camera m_Camera;
    [SerializeField] Rigidbody rb;

    [Header("플레이어 이동관련")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float horizontal;
    [SerializeField] float vertical;
    [SerializeField] float jumpForced = 3f;

    [Header("이동방향")]
    [SerializeField] Vector3 moveDirection;

    [Header("상태")]
    [SerializeField] bool isRun;
    [SerializeField] bool isJump;
    [SerializeField] bool isGround;
    [SerializeField] bool isMove = true;
    [SerializeField] float jumpCoolTime = 0.2f;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_Camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && !isJump && isMove)
        {
            Jump();
            m_Animator.SetTrigger("Jump");
        }

        if (isMove)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            horizontal = 0f;
            vertical = 0f;
        }

        isRun = Input.GetKey(KeyCode.LeftShift);

        Vector3 cameraForward = m_Camera.transform.forward;
        Vector3 cameraRight = m_Camera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

        float inputMagnitude = new Vector2(horizontal, vertical).magnitude;
        float speed = 0f;

        if (inputMagnitude > 0.1f)
        {
            speed = isRun ? 1f : 0.5f;
        }

        m_Animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        float currentMoveSpeed = isRun ? runSpeed : walkSpeed;

        Vector3 nextPosition = rb.position + moveDirection * currentMoveSpeed * Time.fixedDeltaTime;
        if (isMove)
            rb.MovePosition(nextPosition);
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion nextRotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
            rb.MoveRotation(nextRotation);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForced, ForceMode.Impulse);
        isGround = false;
        isJump = true;
        StartCoroutine(JumpCoolTime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    public void CantMove() { isMove = false; }
    public void CanMove() {isMove = true; }
    IEnumerator JumpCoolTime()
    {
        yield return new WaitForSeconds(jumpCoolTime);
        isJump = false;
    }
}