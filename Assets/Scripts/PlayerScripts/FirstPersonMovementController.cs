using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovementController : MonoBehaviour
{
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0, 1)]
    public float airControlPercent;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    int jumpAmount;

    //Animator animator;
    public Transform cameraT;
    CharacterController controller;

    public Transform playerBottom;

    public GameObject grappleHook;
    GameObject curGrappleHook;
    public Transform grappleShootPoint;
    public GameObject grappleLine;
    LineRenderer grappleLineRenderer;
    GameObject curGrappleLine;

    Vector3 grapplePoint;
    public float grappleSpeed;
    public float gappleHookSpeed;

    void Start()
    {
        //animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        // animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cameraT.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                grapplePoint = Vector3.negativeInfinity;
                StartCoroutine(grapple(hit.point));
            }
        }
        else if (Input.GetMouseButton(1) && grapplePoint != Vector3.negativeInfinity)
        {
            grappleLineRenderer.SetPosition(0, grappleShootPoint.position);

            Vector3 moveTowards = grapplePoint - transform.position;
            controller.Move(new Vector3(moveTowards.x * grappleSpeed, moveTowards.y * grappleSpeed, moveTowards.z * grappleSpeed));
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Destroy(curGrappleLine);
            Destroy(curGrappleHook);
        }
        else
        {
            Move(inputDir, running);
        }
    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            //float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * (inputDir.magnitude);
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = (transform.right * inputDir.x + transform.forward * inputDir.y) * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            jumpAmount = 0;
        }

        if (jumpAmount <= 1)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
        else if (jumpAmount == 2)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * (jumpHeight * 4));
            velocityY = jumpVelocity;
        }

        jumpAmount++;
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    IEnumerator grapple(Vector3 hitPoint)
    {
        if (curGrappleLine != null) Destroy(curGrappleLine);

        curGrappleLine = Instantiate(grappleLine);
        curGrappleLine.transform.parent = this.transform;
        grappleLineRenderer = curGrappleLine.GetComponent<LineRenderer>();

        curGrappleHook = Instantiate(grappleHook);
        curGrappleHook.transform.position = grappleShootPoint.position;
        curGrappleHook.transform.LookAt(hitPoint);

        grappleLineRenderer.SetVertexCount(2);
        grappleLineRenderer.SetPosition(0, grappleShootPoint.position);
        grappleLineRenderer.SetPosition(1, curGrappleHook.transform.position);
        grappleLineRenderer.enabled = true;

        while (true)
        {
            curGrappleHook.transform.position = Vector3.MoveTowards(curGrappleHook.transform.position, hitPoint, gappleHookSpeed * Time.deltaTime);

            grappleLineRenderer.SetPosition(1, curGrappleHook.transform.position);

            if (Vector3.Distance(curGrappleHook.transform.position, hitPoint) < .01f) break;
            yield return null;
        }

        grapplePoint = hitPoint;
    }
}
