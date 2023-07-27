using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

public class PlayerController : MonoBehaviour
{
    [Header("*Components")]
    [SerializeField] GameSystem GameSystem;
    [SerializeField] GameManager GameManager;
    [SerializeField] ObjectManager ObjectManager;

    [Header("*Player")]
    [SerializeField] public Camera mainCamera;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Rigidbody playerRigid;

    [Header("*Stat")]
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] public IntReactiveProperty seedValue = new IntReactiveProperty();
    [SerializeField] public IntReactiveProperty damage = new IntReactiveProperty();
    [SerializeField] int maxHp;
    IntReactiveProperty CurrentHP = new IntReactiveProperty();

    [HideInInspector] public Vector3 playerLookVector;
    public bool isAction;

    float rayLength;
    bool isJump;
    Vector3 right, forward, rightMovement, forwardMovement, finalMovement, direction;

    private void Awake()
    {
        CurrentHP.Value = maxHp;

        forward = mainCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        #region UniRX
        this.UpdateAsObservable()
            .BatchFrame(0, FrameCountType.FixedUpdate)
            .Subscribe(x =>
            {
                if (direction != Vector3.zero)
                {
                    transform.position += finalMovement * playerSpeed * Time.deltaTime;
                }

                if(transform.position.y <= -10)
                {
                    // 체력 깎이고 마지막 체크포인트로 이동
                    CurrentHP.Value -= 1;
                    GameSystem.CheckPoint(this.gameObject);
                }
            });

        CurrentHP
            .ThrottleFirst(System.TimeSpan.FromSeconds(0.5f))
            .Subscribe(hp =>
            {
                if (hp > 0)
                {
                    SetHPbar(hp);
                    return;
                }
                SetHPbar(0);
                GameSystem.isGameOver.Value = true;
            });
        #endregion
    }

    #region Input Action
    private void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Mouse"].performed += OnMouse;
        playerInput.actions["Mouse"].canceled += OnMouse;
        playerInput.actions["Jump"].started += OnJump;
        playerInput.actions["Shoot"].started += OnShoot;
        playerInput.actions["Eat"].started += OnEat;
        playerInput.actions["Action"].started += OnAction;
        playerInput.actions["Action"].canceled += OnAction;
    }
    private void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Mouse"].performed -= OnMouse;
        playerInput.actions["Mouse"].canceled -= OnMouse;
        playerInput.actions["Jump"].started -= OnJump;
        playerInput.actions["Shoot"].started -= OnShoot;
        playerInput.actions["Eat"].started -= OnEat;
        playerInput.actions["Action"].started -= OnAction;
        playerInput.actions["Action"].canceled -= OnAction;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        var value = ctx.ReadValue<Vector2>();

        rightMovement = right * value.x;
        forwardMovement = forward * value.y;
        finalMovement = rightMovement + forwardMovement;
        direction = Vector3.Normalize(finalMovement);
    }

    private void OnMouse(InputAction.CallbackContext ctx)
    {
        Ray cameraRay = mainCamera.ScreenPointToRay(ctx.ReadValue<Vector2>());
        Plane groupPlane = new Plane(Vector3.up, Vector3.zero);

        if (groupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            playerLookVector = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
            transform.LookAt(playerLookVector);
        }
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if(!isJump)
        {
            playerRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJump = true;
        }
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if(seedValue.Value > 0)
        {
            Bullet bullet = ObjectManager.BulletObjectPool();
            bullet.tag = "PlayerBullet";
            seedValue.Value -= 1;
        }
    }

    private void OnEat(InputAction.CallbackContext ctx)
    {
        if (seedValue.Value > 0)
        {
            CurrentHP.Value += 1;
            seedValue.Value -= 1;
        }
    }

    private void OnAction(InputAction.CallbackContext ctx)
    {
        Debug.Log("OnAction");
        if(ctx.started)
        {
            isAction = true;
            Debug.Log(isAction);
        }
        else
        {
            isAction = false;
        }
    }
    #endregion

    protected virtual void SetHPbar(int currentHP)
    {
        //Entity 따로 만들어서 Enemy와 PlayerController 겹치는 부분 합치기
        float HpBar_X_Scale = (float)currentHP / maxHp; // GameSystem에서 체력 받아올 것.
        HpBar_X_Scale = Mathf.Clamp(HpBar_X_Scale, 0, 1);
        GameSystem.health.transform.localScale = new Vector3(HpBar_X_Scale, 1f, 1f);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GoldenSeed"))
        {
            other.gameObject.SetActive(false);
            Debug.Log("황금 해바라기씨를 얻었다!");
            GameSystem.isGameClear.Value = true;
            GameManager.goldenSeedValue.Value += 1;
        }

        if(other.gameObject.CompareTag("Enemy"))
        {
            // 몬스터 여러마리 생기면 타입별로 데미지 받아오기.
            CurrentHP.Value -= 1;
        }

        if(other.gameObject.CompareTag("CheckPoint"))
        {
            for(int i = 0; i < GameSystem.checkPoint.Length; i++)
            {
                if(GameSystem.checkPoint[i].gameObject == other.gameObject)
                {
                    GameSystem.checking[i] = true;
                }
            }
        }

        if(other.gameObject.CompareTag("SunFlower"))
        {
            seedValue.Value = 50;
        }
    }
}
