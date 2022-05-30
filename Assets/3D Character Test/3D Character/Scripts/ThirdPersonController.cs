using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float animationTransitionTime;
    [SerializeField] private Transform constraint;
    [SerializeField] private float bendAmount;
    [SerializeField] private Transform followPoint;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform foot;
    [SerializeField] private float footRadius;
    [SerializeField] private bool _isGrounded;
    
    private Vector3 _movement;
    private Rigidbody rb;
    private Animator _characterAnimator;
    private float _lerpingSpeedX;
    private float _lerpingSpeedY;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int _jump = Animator.StringToHash("Jump");
    private float _performJump;
    private static readonly int jumpEnd = Animator.StringToHash("Jump End");

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _characterAnimator = GetComponent<Animator>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Animate(Movement());
    }

    private void Animate(Vector2 axis)
    {
        _lerpingSpeedX = Mathf.Lerp(_lerpingSpeedX, axis.x, animationTransitionTime * Time.deltaTime);
        _lerpingSpeedY = Mathf.Lerp(_lerpingSpeedY, axis.y, animationTransitionTime * Time.deltaTime);

        _characterAnimator.SetFloat(X, _lerpingSpeedX);
        _characterAnimator.SetFloat(Y, _lerpingSpeedY);
    }

    private void Jump(Vector3 move)
    {

        move.y = 2f;
    }

    private Vector2 Movement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
            
        if (Mathf.Abs(moveX) > 0f || Mathf.Abs(moveY) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, followPoint.localRotation, 10f * Time.deltaTime);
            
            float mouseY = Input.GetAxis("Mouse X");
            
            if (mouseY > 3f) return new Vector2(moveX, moveY);
            
            constraint.localRotation =
                Quaternion.Lerp(constraint.localRotation, Quaternion.Euler(0f, 0f, mouseY * -bendAmount),
                    3f * Time.deltaTime);
        }
        else
            constraint.localRotation = Quaternion.identity;
        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.velocity = Vector3.up * jumpForce;
            _characterAnimator.SetTrigger(_jump);
            _isGrounded = false;
            _characterAnimator.ResetTrigger(jumpEnd);
        }
        
        _movement = new Vector3(moveX, 0f, moveY);
        Vector3 localMovement = transform.TransformDirection(_movement) * speed;
        Vector3 vel = rb.velocity;
        vel.x = localMovement.x;
        vel.z = localMovement.z;
        rb.velocity = vel;
        
        return new Vector2(moveX, moveY);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isGrounded = true;
        _characterAnimator.SetTrigger(jumpEnd);
        if (Physics.SphereCast(foot.position, footRadius, -transform.up, out RaycastHit hit, 10))
            _isGrounded = true;
    }
}
