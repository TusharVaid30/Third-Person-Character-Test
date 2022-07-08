using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputsController : MonoBehaviour
{
    public static UnityEvent jumpAction = new UnityEvent();
    public static bool jumping;
    
    private static SmolCharacter _smolCharacter;

    private void Awake()
    {
        _smolCharacter = new SmolCharacter();
    }

    private void Start()
    {
        _smolCharacter.Player.Jump.performed += Jump;
        _smolCharacter.Player.Jump.started += Jumping;
        _smolCharacter.Player.Jump.canceled += JumpEnd;
    }

    private void OnEnable()
    {
        _smolCharacter.Enable();
    }

    private void OnDisable()
    {
        _smolCharacter.Disable();
    }

    public static Vector2 Movement()
    {
        return _smolCharacter.Player.Movement.ReadValue<Vector2>();
    }

    public static Vector2 Mouse()
    {
        Vector2 mouse = new Vector2(_smolCharacter.Player.MouseX.ReadValue<float>(),
            _smolCharacter.Player.MouseY.ReadValue<float>());
        
        return mouse;
    }

    private static void Jump(InputAction.CallbackContext obj)
    {
        jumpAction.Invoke();
    }

    private static void Jumping(InputAction.CallbackContext obj)
    {
        jumping = true;
    }
    
    private static void JumpEnd(InputAction.CallbackContext obj)
    {
        jumping = false;
    }
}