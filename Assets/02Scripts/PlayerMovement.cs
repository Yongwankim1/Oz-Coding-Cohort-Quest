using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //카메라가 보는 방향을 위해
    [SerializeField] Animator m_Animator;
    [SerializeField] Camera m_Camera;
    [SerializeField] Rigidbody rb;

    [SerializeField] int walkSpeed = 5; //걷는 속도
    [SerializeField] int runSpeed = 8; //뛰는 속도
    [SerializeField] float jumpForced = 3f; //점프 힘

    [SerializeField] float horizontal;
    [SerializeField] float vertical;

    [SerializeField] Vector3 moveDirection;

    [SerializeField] bool isRun; //뛰는 상태

    [SerializeField] float jumpCoolTime;
    [SerializeField] bool isJump;//점프 상태
    [SerializeField] bool isGround;//땅에 있는 상태
    
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_Camera = Camera.main;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && !isJump)
        {
            Jump();
            m_Animator.SetTrigger("Jump");
        }
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        isRun = Input.GetKey(KeyCode.LeftShift);

        Vector3 cameraFoward = m_Camera.transform.forward;
        Vector3 cameraRight = m_Camera.transform.right;

        cameraFoward.y = 0f;
        cameraRight.y = 0f;

        cameraFoward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraFoward * vertical + cameraRight * horizontal).normalized;

        float speed = 0f;

        if (moveDirection != Vector3.zero)
        {
            speed = isRun ? 1f : 0.5f;
        }

        m_Animator.SetFloat("Speed", speed);
    }

    private void FixedUpdate()
    {
        int moveSpeed = isRun ? runSpeed : walkSpeed;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        if(moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, moveSpeed * Time.fixedDeltaTime));
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForced, ForceMode.Impulse);
        isGround = false;
        isJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            StartCoroutine(JumpCoolTime());
            m_Animator.SetBool("isGround", isGround);
        }
    }

    IEnumerator JumpCoolTime()
    {
        yield return new WaitForSeconds(jumpCoolTime);
        isJump = false;
    }
}
