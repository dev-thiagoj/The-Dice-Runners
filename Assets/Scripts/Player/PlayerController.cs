using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;

// TIRAR OS HARDCODES

public class PlayerController : Singleton<PlayerController>
{
    public CharacterController characterController;
    //public Rigidbody rigidbody;
    public Animator animator;
    //public ParticleSystem particleSystem;
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
    public float scaleX = .9f;
    public float scaleY = 1.1f;

    public float delayBetweensJumps = 1.5f;
    public bool _isJumping = false;

    [Header("Turbo")]
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

    [Header("Bounds")]
    private float range = 5.6f;

    public bool _isAlive = true;

    private void OnValidate()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        //if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();
        //if (particleSystem == null) particleSystem = GetComponentInChildren<ParticleSystem>();
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
            Move();
            Jump();
            Bounds();
            Inputs();
        }

        if (!canRun) animator.SetTrigger("Idle");
        if (canRun && characterController.isGrounded) animator.SetTrigger("Run");

        Debug.Log(_isJumping);
    }

    public void Inputs()
    {
        if (Input.GetKeyUp(KeyCode.S) && !_turboOn) TurboPlayer();
        if (Input.GetKey(KeyCode.W)) Walk();
        if (Input.GetKeyUp(KeyCode.W)) BackRun();
    }


    #region === MOVEMENTS ===

    public void Move()
    {

        var move = new Vector3((Input.GetAxis("Horizontal")) * -sideSpeed, 0, _currRunSpeed);
        _vSpeed -= gravity * Time.deltaTime;
        move.y = _vSpeed;

        characterController.Move(move * Time.deltaTime);
    }

    public void Jump()
    {
        //if (_isJumping) _isJumping = false;

        if (characterController.isGrounded)
        {
            _vSpeed = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _vSpeed = jumpForce;

                if (!_isJumping)
                {
                    _isJumping = true;
                    animator.SetTrigger("Jump");
                    SFXPool.Instance.Play(SFXType.JUMP_02);
                }
            }

            Invoke(nameof(NotJumping), 3);
        }

    }

    public void NotJumping()
    {
        _isJumping = false;
        //animator.SetTrigger("Run");
    }

    void Bounds()
    {
        //Bound
        if (transform.position.x > range)
        {
            characterController.transform.position = new Vector3(range, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < -range)
        {
            characterController.transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }


    #region === OLD MOVES === 
    /*public void Movement()
    {
        //Move Forward
        //transform.position += Vector3.forward.normalized * _currRunSpeed * Time.deltaTime;
        //characterController.SimpleMove(Vector3.forward * _currRunSpeed * Time.deltaTime);
        characterController.Move(_currRunSpeed * Time.deltaTime * Vector3.forward);


        //Move Sides
        float horizontalInputs = Input.GetAxis("Horizontal");
        float verticalInputs = Input.GetAxis("Vertical");


        //if (_isJumping == false) transform.Translate(Vector3.right.normalized * -_currSideSpeed * Time.deltaTime * horizontalInputs);
        if (_isJumping == false) characterController.Move(-_currSideSpeed * horizontalInputs * Time.deltaTime * Vector3.right);

        //Bound
        if (transform.position.x > range)
        {
            characterController.transform.position = new Vector3(range, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < -range)
        {
            characterController.transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }

    public void Jump()
    {

        if (characterController.isGrounded)
        {
            _vSpeed = 0;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                //characterController.velocity.y = Vector3.up * jumpForce * 50 * Time.deltaTime;

                _vSpeed = jumpForce;
                SFXPool.Instance.Play(SFXType.JUMP_02);
                //animator.SetTrigger("Jump");
                //_isJumping = true;
                //Invoke(nameof(NotJumping), delayBetweensJumps);

                transform.DOScaleX(scaleX, .2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
                transform.DOScaleY(scaleY, .2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
            }
        }

        _vSpeed -= gravity * Time.deltaTime;
        characterController.Move(Vector3.up * _vSpeed * Time.deltaTime);
    }*/
    #endregion


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

    public void Walk()
    {
        _currRunSpeed = walkSpeed;
        _currSideSpeed = walkSpeed;
        animator.speed = .5f;
    }
    #endregion

    #region === HEALTH ===
    public void Dead()
    {
        EnableRagDoll();
        _isAlive = false;
        canRun = false;
        SFXPool.Instance.Play(SFXType.DEATH_03);
        //particleSystem.Stop();
        OnDead();
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
        if (_currTurbo < maxTurbos && !_isJumping)
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
        //b = _hasMagnetic;
        if (b == true) StartCoroutine(MagneticCoroutine());
    }

    public IEnumerator MagneticCoroutine()
    {
        magneticCollider.transform.DOScaleX(5, 1);
        yield return new WaitForSeconds(magneticTime);
        magneticCollider.transform.DOScaleX(1, 1);
        MagneticOn(false);
        StopCoroutine(MagneticCoroutine());
        //_hasMagnetic = false;
    }

    #endregion
}
