using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.02f;        // độ lắc khi di chuột
    public float swaySmooth = 6f;           // độ mượt

    [Header("Bob Settings")]
    public float walkBobAmount = 0.05f;     // lắc khi đi bộ
    public float runBobAmount = 0.1f;       // lắc khi chạy
    public float bobSpeed = 8f;             // tốc độ lắc

    [Header("Zoom Settings")]
    public Transform adsPosition;           // vị trí khi zoom
    public Transform hipPosition;           // vị trí bình thường
    public float adsSpeed = 10f;            // tốc độ zoom

    private Vector3 bobOffset;
    private Vector3 initialLocalPos;
    private CharacterController controller;
    private Camera cam;

    void Start()
    {
        initialLocalPos = transform.localPosition;
        controller = GetComponentInParent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {
        HandleSway();
        HandleBob();
        HandleADS();
    }

    void HandleSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        Vector3 sway = new Vector3(-mouseX, -mouseY, 0) * swayAmount;
        Vector3 targetPos = initialLocalPos + sway;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * swaySmooth);
    }

    void HandleBob()
    {
        if (controller.velocity.magnitude > 0.1f && controller.isGrounded)
        {
            float bobAmount = Input.GetKey(KeyCode.LeftShift) ? runBobAmount : walkBobAmount;
            float wave = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            bobOffset = new Vector3(0, wave, 0);
        }
        else
        {
            bobOffset = Vector3.zero;
        }

        transform.localPosition += bobOffset * Time.deltaTime;
    }

    void HandleADS()
    {
        Transform target = Input.GetMouseButton(1) ? adsPosition : hipPosition;
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * adsSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * adsSpeed);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Input.GetMouseButton(1) ? 40f : 60f, Time.deltaTime * adsSpeed);
    }
}
