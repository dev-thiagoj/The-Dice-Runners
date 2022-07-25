using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;

// TIRAR OS HARDCODES

public class PlayerController : Singleton<PlayerController>
{
    public CharacterController characterController;
    public Animator animator;
    public AudioSource audioSource;
    public List<AudioClip> sfxPlayer;

    [Header("Movement")]
    public float runSpeed = 5;
    public float sideSpeed = 5;
    float _currRunSpeed;
    float _currSideSpeed;
    [Range(1, 4)]
    public float walkSpeed = 3;
    public bool canRun = false;

    [Header("Jump")]
    float _vSpeed;
    public float jumpForce = 5;
    public float gravity = 9.8f;
    float distToGround;
    float spaceToGround = .3f;

    [Header("Turbo PowerUp")]
    public float turboSpeed;
    public int maxTurbos = 3;
    public int _currTurbo;
    public float turboTime;
    bool _turboOn = false;

    [Header("Magnetic Powerup")]
    public Transform magneticCollider;
    public float magneticSize;
    public float magneticTime;
    public bool _hasMagnetic = false;

    [Header("Invencible Powerup")]
    public MeshRenderer[] playerMeshs;
    public SkinnedMeshRenderer[] playerSkinned;
    public int blinksTimes = 100;
    public bool isInvencible = false;

    [Header("Bounds")]
    private float range = 5.6f;

    [Header("Expressions")]
    public Transform[] expressions;
    public float animationDuration;
    public Ease ease;
    int _index;

    public bool _isAlive = true;

    private void OnValidate()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();
        playerMeshs = GetComponentsInChildren<MeshRenderer>();
        playerSkinned = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        _currTurbo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            IsGrounded();
            Move();
            if (IsGrounded()) Jump();
            Bounds();
            if (IsGrounded()) Inputs();
        }

        if (!canRun) animator.SetTrigger("Idle");
        if (canRun && IsGrounded()) animator.SetTrigger("Run");
    }

    public void Inputs()
    {
        if (Input.GetKeyUp(KeyCode.S) && !_turboOn) TurboPlayer();
        if (Input.GetKey(KeyCode.W)) Walk();
        if (Input.GetKeyUp(KeyCode.W)) BackRun();
    }


    #region === MOVEMENTS ===

    public void InvokeStartRun()
    {
        Invoke(nameof(StartRun), 3);
        StartCoroutine(ExpressionsCoroutine(0));
    }

    public void StartRun()
    {
        canRun = true;
    }

    public void Move()
    {
        if (isInvencible) _currRunSpeed = 6;

        var move = new Vector3((Input.GetAxis("Horizontal")) * -_currSideSpeed, 0, _currRunSpeed);
        _vSpeed -= gravity * Time.deltaTime;
        move.y = _vSpeed;

        characterController.Move(move * Time.deltaTime);
    }

    public void Jump()
    {

        if (characterController.isGrounded)
        {
            _vSpeed = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vSpeed = jumpForce;
                animator.SetTrigger("Jump");
                SFXPool.Instance.Play(SFXType.JUMP_02);
            }
        }
    }

    public void Walk()
    {
        _currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        animator.speed = .5f;
    }
    #endregion

    bool IsGrounded()
    {
        Debug.DrawRay(transform.position, -Vector2.up, Color.magenta, distToGround + spaceToGround);
        return Physics.Raycast(transform.position, -Vector2.up, distToGround + spaceToGround);
    }

    void Bounds()
    {
        if (transform.position.x > range)
        {
            characterController.transform.position = new Vector3(range, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < -range)
        {
            characterController.transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }

    #region === DEBUGS ===

    [NaughtyAttributes.Button]
    public void StopRun()
    {
        runSpeed = 0;
    }

    [NaughtyAttributes.Button]
    public void BackRun()
    {
        _currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        animator.speed = 1;
    }

    [NaughtyAttributes.Button]
    public void ExpressionsDebug()
    {
        _index = 0;
        StartCoroutine(ExpressionsCoroutine(0));
    }

    #endregion

    #region === HEALTH ===
    public void Dead()
    {
        if (!isInvencible)
        {
            EnableRagDoll();
            _isAlive = false;
            canRun = false;
            SFXPool.Instance.Play(SFXType.DEATH_03);
            StartCoroutine(ExpressionsCoroutine(1));
            OnDead();
        }
    }

    public void OnDead()
    {
        runSpeed = 0;
        characterController.detectCollisions = false;
        animator.SetTrigger("Die");
        Invoke(nameof(ShowEndGameScreen), 5);
    }

    public void EnableRagDoll()
    {
        //rigidbody.isKinematic = true;
        //rigidbody.detectCollisions = false;
    }

    public void ShowEndGameScreen()
    {
        GameManager.Instance.EndGame();
    }
    #endregion

    #region === POWERUPS ===
    public void TurboPlayer()
    {
        if (_currTurbo < maxTurbos)
        {
            StartCoroutine(TurboCoroutine());
            _currTurbo++;
            ItemManager.Instance.RemoveTurbo();
        }
        else Debug.Log("Without turbo");
    }

    public IEnumerator TurboCoroutine()
    {
        _turboOn = true;
        _currRunSpeed = turboSpeed;
        SFXPool.Instance.Play(SFXType.USE_TURBO_06);
        yield return new WaitForSeconds(turboTime);
        _currRunSpeed = runSpeed;
        _turboOn = false;
        StopCoroutine(TurboCoroutine());
    }

    public void MagneticOn(bool b = false)
    {
        if (b == true) StartCoroutine(MagneticCoroutine());
    }

    public IEnumerator MagneticCoroutine()
    {
        magneticCollider.transform.DOScaleX(5, 1);
        yield return new WaitForSeconds(magneticTime);
        magneticCollider.transform.DOScaleX(1, 1);
        MagneticOn(false);
        StopCoroutine(MagneticCoroutine());
    }

    [NaughtyAttributes.Button]
    public void StartDebugCoroutine()
    {
        StartCoroutine(InvencibleCoroutine());
    }

    public IEnumerator InvencibleCoroutine()
    {
        for(int i = 0; i < blinksTimes; i++)
        {
            foreach (var mesh in playerMeshs)
            {
                mesh.enabled = false;
            }

            foreach (var skinned in playerSkinned)
            {
                skinned.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var mesh in playerMeshs)
            {
                mesh.enabled = true;
            }

            foreach (var skinned in playerSkinned)
            {
                skinned.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion

    #region === EXPRESSIONS ===

    public IEnumerator ExpressionsCoroutine(int index)
    {
        expressions[index].transform.DOScale(0.5f, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        yield return new WaitForEndOfFrame();
    }

    #endregion
}
