
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayer : MonoBehaviour
{
    public Transform playerCamera;

    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.15f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraX;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // カメラ未設定の場合は子オブジェクトから探す
        if (playerCamera == null)
        {
            Camera cam = GetComponentInChildren<Camera>(true);
            if (cam != null)
                playerCamera = cam.transform;
        }

        if (playerCamera == null)
            Debug.LogError("PlayerCameraが見つかりません");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Escキーでカーソルを解除
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 左クリックでカーソルを固定
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // マウスで視点回転
        if (Mouse.current != null &&
            playerCamera != null &&
            Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            float mouseX = delta.x * mouseSensitivity;
            float mouseY = delta.y * mouseSensitivity;

            // 左右回転
            transform.Rotate(Vector3.up * mouseX);

            // 上下回転
            cameraX -= mouseY;
            cameraX = Mathf.Clamp(cameraX, -85f, 85f);

            playerCamera.localRotation =
                Quaternion.Euler(cameraX, 0f, 0f);
        }

        // WASD移動
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y += 1;
        if (Keyboard.current.sKey.isPressed) input.y -= 1;
        if (Keyboard.current.aKey.isPressed) input.x -= 1;
        if (Keyboard.current.dKey.isPressed) input.x += 1;

        input = Vector2.ClampMagnitude(input, 1f);

        Vector3 movement =
            transform.right * input.x +
            transform.forward * input.y;

        // 接地判定
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        // ジャンプ
        if (controller.isGrounded &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        movement = movement * moveSpeed +
                   Vector3.up * verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }
}
