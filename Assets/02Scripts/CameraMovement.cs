using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("카메라 인칭")]
    [SerializeField] Transform currentTarget;
    [SerializeField] Transform firstView;
    [SerializeField] Transform thirdView;
    [Header("카메라 회전속도")]
    [SerializeField] float cameraSpeed;

    private float rotX;
    private float rotY;

    [Header("제한 각도")]
    [SerializeField] float minY;
    [SerializeField] float maxY;

    [Header("마우스 민감도")]
    [SerializeField] float sensitivityX = 100f;
    [SerializeField] float sensitivityY = 100f;

    [Header("대상과 카메라 거리")]
    [SerializeField] float minDistance = 1.0f;
    [SerializeField] float maxDistance = 5.0f;
    [SerializeField] float finalDistance;

    [Header("이동 방향")]
    [SerializeField] Vector3 dirNormalized;
    [SerializeField] Vector3 finalDir;

    [Header("메인 카메라")]
    [SerializeField] Transform realCamera;

    [Header("1인칭 온오프")]
    [SerializeField] bool isFirstView;
    [SerializeField] UnityEngine.GameObject crossHair;
    private void Start()
    {
        realCamera = Camera.main.transform;
        dirNormalized = realCamera.localPosition.normalized;
        currentTarget = thirdView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstView = !isFirstView;
            currentTarget = isFirstView ? firstView : thirdView;
            crossHair.SetActive(isFirstView);
        }

        rotX += Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        rotY -= Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        rotY = Mathf.Clamp(rotY, minY, maxY);
        if (rotX >= 360 || rotX <= -360) rotX = 0;
        

        Quaternion rot = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = rot;
    }
    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, cameraSpeed * Time.fixedDeltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;
        if (isFirstView)
        {
            finalDistance = 0;
        }
        else
        {
            if (Physics.Linecast(transform.position, finalDir, out hit))
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                finalDistance = maxDistance;
            }
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, cameraSpeed * Time.fixedDeltaTime);


    }
}
