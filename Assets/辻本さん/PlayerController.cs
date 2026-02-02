using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Normal,
        Ghost
    }

    [Header("基本移動設定")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashAcceleration = 10f;

    [Header("ジャンプ設定")]
    [SerializeField] private float jumpSpeed = 7f;

    [Header("重力設定")]
    [SerializeField] private float gravity = 15f;
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private float initFallSpeed = 2f;

    [Header("カメラ設定")]
    [SerializeField] private Camera targetCamera;

    [Header("壁登り設定")]
    [SerializeField] private float climbSpeed = 1f;
    [SerializeField] private float wallCheckDistance = 0.6f;

    [Header("壁判定")]
    [SerializeField] private float wallSphereRadius = 0.4f;


    [Header("ゴースト設定")]
    [SerializeField] private float ghostSpeed = 5f;
    [SerializeField] private float ghostVerticalSpeed = 3f;

    private Transform _transform;
    private CharacterController characterController;
    private Vector2 inputMove;
    private float verticalVelocity;
    private float turnVelocity;
    private bool isGroundedPrev;
    private bool isClimbing;

    private bool isDashing;
    private float currentSpeed;

    private RaycastHit wallHit;
    private State currentState = State.Normal;

    // アニメーション
    private Animator animator;

    // 入力
    public void OnMove(InputAction.CallbackContext context)
    {

        inputMove = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (currentState == State.Ghost) return; // ゴースト中はジャンプ禁止
        if (!context.performed) return;
        if (!characterController.isGrounded) return;

        //if (!context.performed || !characterController.isGrounded) return;
        verticalVelocity = jumpSpeed;
        animator.SetTrigger("Jump");
        //Debug.Log("OnJump");
    }

    private void Awake()
    {
        _transform = transform;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (targetCamera == null)
            targetCamera = Camera.main;

        currentSpeed = speed;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy || !characterController.enabled)
            return;

        switch (currentState)
        {
            case State.Normal:
                UpdateNormal();
                break;

            case State.Ghost:
                UpdateGhost();
                break;
        }

        UpdateAnimator();
    }


    private void UpdateAnimator()
    {
        Vector3 horizontalVelocity = characterController.velocity;
        horizontalVelocity.y = 0f;

        // 移動量
        animator.SetFloat("Speed", horizontalVelocity.magnitude);

        // 接地
        animator.SetBool("IsGrounded", characterController.isGrounded);

        // 落下
        animator.SetFloat("yVelocity", verticalVelocity);

        // 壁登り
        animator.SetBool("IsClimbing", isClimbing);
        animator.SetFloat("ClimbSpeed", Mathf.Abs(inputMove.y));
    }
    // 通常
    private void UpdateNormal()
    {
        bool isGrounded = characterController.isGrounded;
        bool isTouchingWall = false;

        float verticalInput = inputMove.y;
        bool hasClimbInput = Mathf.Abs(verticalInput) > 0.1f;

        // SphereCastの始点を頭付近に変更
        Vector3 start = _transform.position + Vector3.up * (characterController.height * 0.5f);

        if (Physics.SphereCast(start, wallSphereRadius, _transform.forward, out wallHit, wallCheckDistance))
        {
            float wallDot = Vector3.Dot(wallHit.normal, Vector3.up);
            if (wallDot < 0.5f)
                isTouchingWall = true;
        }

        // 壁登り処理
        if (isTouchingWall && Input.GetMouseButton(0) && hasClimbInput)
        {
            isClimbing = true;
            verticalVelocity = 0f;

            // 壁法線取得
            if (Physics.SphereCast(start, wallSphereRadius, _transform.forward, out wallHit, wallCheckDistance))
            {
                Vector3 wallNormal = wallHit.normal;

                // 上方向も含めた登る方向
                Vector3 climbDir = Vector3.Cross(Vector3.Cross(wallNormal, Vector3.up), wallNormal).normalized;
                //Vector3 move = climbDir * (verticalInput * climbSpeed) + Vector3.up * (verticalInput * climbSpeed)- wallNormal * 0.2f;

                Vector3 move = Vector3.up * (verticalInput * climbSpeed) - wallNormal * 0.3f;
                characterController.Move(move * Time.deltaTime);

                // 壁向き回転
                Quaternion targetRot = Quaternion.LookRotation(-wallNormal);
                _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRot, Time.deltaTime * 20f);
            }

            // 壁登り中に登りきったかチェック
            if (CheckClimbOver(wallHit))
            {
                isClimbing = false;
                verticalVelocity = jumpSpeed * 0.5f; // 少し押し上げて自然に着地
            }

            return;
        }
        else
        {
            isClimbing = false;
        }

        // ダッシュ処理
        if (Keyboard.current.leftShiftKey.isPressed && !isClimbing && isGrounded)
            isDashing = true;
        else
            isDashing = false;

        float targetSpeed = isDashing ? dashSpeed : speed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * dashAcceleration);

        // 重力処理
        if (!isClimbing)
        {
            if (isGrounded && !isGroundedPrev)
                verticalVelocity = -initFallSpeed;
            else if (!isGrounded)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                if (verticalVelocity < -fallSpeed)
                    verticalVelocity = -fallSpeed;
            }
        }
        isGroundedPrev = isGrounded;

        // 移動
        float cameraAngleY = targetCamera.transform.eulerAngles.y;
        Vector3 moveVelocity = new Vector3(inputMove.x * currentSpeed, verticalVelocity, inputMove.y * currentSpeed);
        moveVelocity = Quaternion.Euler(0, cameraAngleY, 0) * moveVelocity;
        characterController.Move(moveVelocity * Time.deltaTime);

        // 向き
        if (inputMove != Vector2.zero)
        {
            float targetAngleY = -Mathf.Atan2(inputMove.y, inputMove.x) * Mathf.Rad2Deg + 90f;
            targetAngleY += cameraAngleY;
            float angleY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetAngleY, ref turnVelocity, 0.1f);
            _transform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }


    // ゴースト
    private void UpdateGhost()
    {
        Vector3 move = new Vector3(inputMove.x, 0, inputMove.y);

        // カメラ基準で移動方向を変換
        float cameraY = targetCamera.transform.eulerAngles.y;
        move = Quaternion.Euler(0, cameraY, 0) * move;
        move.Normalize();

        // 上下移動
        float upDown = 0f;
        if (Keyboard.current.spaceKey.isPressed)
            upDown += 1f;
        if (Keyboard.current.leftCtrlKey.isPressed)
            upDown -= 1f;

        move.y = upDown * ghostVerticalSpeed;

        // 実際の移動
        characterController.Move(move * ghostSpeed * Time.deltaTime);
    }

    private bool CheckClimbOver(RaycastHit wallHit)
    {
        Vector3 headPos = _transform.position + Vector3.up * (characterController.height - 0.1f);
        if (Physics.Raycast(headPos, Vector3.up, 0.5f))
        {
            return false;
        }

        Vector3 forwardPos = _transform.position + (-wallHit.normal * 0.2f) + Vector3.up * (characterController.height * 0.5f);
        if (Physics.Raycast(forwardPos, Vector3.down, out RaycastHit groundHit, characterController.height))
        {
            if (Vector3.Dot(groundHit.normal, Vector3.up) > 0.7f)
            {
                return true;

            }
        }

        return false;
    }

    // 状態切り替え
    public void SetState(State newState)
    {
        currentState = newState;
        //Debug.Log($"State changed to: {currentState}");
    }

    public State GetState()
    {
        return currentState;
    }

    private void OnDrawGizmosSelected()
    {
        if (_transform == null) _transform = transform;
        if (characterController == null) characterController = GetComponent<CharacterController>();

        Vector3 start = _transform.position + Vector3.up * (characterController.height * 0.8f);

        Vector3 end = start + _transform.forward * wallCheckDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(start, wallSphereRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(end, wallSphereRadius);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(start, end);

        if (Physics.SphereCast(start, wallSphereRadius, _transform.forward, out wallHit, wallCheckDistance))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(wallHit.point, wallHit.normal);
        }
    }

}