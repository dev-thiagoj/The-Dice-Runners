using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;

// TIRAR OS HARDCODES

public class PlayerController : Singleton<PlayerController>
{
    #region === VARIABLES ===

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
    public float jumpForce = 8;
    float _currJumpForce;
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
    public ForceFieldManager forceField;
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

    [Header("Look At EndGame")]
    public RotationLookAt rotationLook;
    public string targetName;

    public bool _isAlive = true;
    #endregion

    private void OnValidate()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();
        playerMeshs = GetComponentsInChildren<MeshRenderer>();
        playerSkinned = GetComponentsInChildren<SkinnedMeshRenderer>();
        if (rotationLook == null) rotationLook = GetComponent<RotationLookAt>();
        if (forceField == null) forceField = GetComponentInChildren<ForceFieldManager>();
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
        _currJumpForce = jumpForce;
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

        if (_isAlive == false && IsGrounded()) Dead();
        if (!canRun) animator.SetTrigger("Idle");
        if (canRun && IsGrounded()) animator.SetTrigger("Run");
    }

    public void Inputs()
    {
        if (Input.GetKeyUp(KeyCode.S) && !_turboOn) TurboPlayer();
        if (Input.GetKey(KeyCode.W)) Walk();
        if (Input.GetKeyUp(KeyCode.W)) BackRun();
    }

    void FindLookAtTarget()
    {
        rotationLook.target = GameObject.Find("FemaleCharacter").GetComponent<Transform>();
    }

    #region === MOVEMENTS ===

    public void InvokeStartRun()
    {
        Invoke(nameof(StartRun), 5);
        Invoke(nameof(StartExpressionsCoroutine), 4);
    }

    public void StartExpressionsCoroutine()
    {
        StartCoroutine(ExpressionsCoroutine(0));
    }

    public void StartRun()
    {
        canRun = true;
        //Actions.startTutorial();
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
                _vSpeed = _currJumpForce;
                _currSideSpeed = 0;
                animator.SetTrigger("Jump");
                SFXPool.Instance.Play(SFXType.JUMP_02);
                Invoke(nameof(BackRun), 2);
            }
        }
    }

    public void Walk()
    {
        _currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        _currJumpForce = 0;
        animator.speed = .5f;
    }

    public void BackRun()
    {
        _currRunSpeed = runSpeed;
        _currSideSpeed = sideSpeed;
        _currJumpForce = jumpForce;
        animator.speed = 1;
    }
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
    #endregion

    #region === HEALTH ===
    public void Dead()
    {
        _isAlive = true;
        canRun = false;
        characterController.detectCollisions = false;
        StartCoroutine(ExpressionsCoroutine(1));
        OnDead();
    }

    public void OnDead()
    {
        SFXPool.Instance.Play(SFXType.DEATH_03);
        animator.SetTrigger("Die");
        //if(IsGrounded()) animator.SetTrigger("Die");
        Invoke(nameof(ShowEndGameScreen), 5);
    }

    public void DieOnGround()
    {
        if (_isAlive == false && IsGrounded())
        {
        }
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
        else ItemManager.Instance.WithoutTurboWarning();
    }

    public IEnumerator TurboCoroutine()
    {
        if (!_turboOn)
        {
            _turboOn = true;
            SFXPool.Instance.Play(SFXType.USE_TURBO_06);
            _currRunSpeed = turboSpeed;
            yield return new WaitForSeconds(turboTime);
        }

        _currRunSpeed = runSpeed;
        _turboOn = false;
        StopCoroutine(TurboCoroutine());
    }

    public void MagneticOn(bool b = false)
    {
        if (b == true)
        {
            StartCoroutine(MagneticCoroutine());
            forceField.StartParticleField();
            SFXPool.Instance.Play(SFXType.USE_MAGNETIC_08);
        } 
            
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
        for (int i = 0; i < blinksTimes; i++)
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

    private void OnEnable()
    {
        Actions.findFemaleAnim += FindLookAtTarget;
    }

    private void OnDisable()
    {
        Actions.findFemaleAnim -= FindLookAtTarget;
    }
}