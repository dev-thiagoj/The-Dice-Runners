using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using DG.Tweening;

public class PlayerController : Singleton<PlayerController>
{
    public Rigidbody rigidbody;
    public Animator animator;
    public AudioSource audioSource;
    public List<AudioClip> sfxPlayer;

    [Header("Movement")]
    public float runSpeed = 5;
    public float sideSpeed = 5;
    float _currSpeed;
    public float jumpForce = 5;
    public bool canRun = false;

    [Header("Jump Animation")]
    public float scaleX = .9f;
    public float scaleY = 1.1f;
    public float delayBetweensJumps = 1.5f;

    [Header("Turbo")]
    public float turboSpeed;
    public int maxTurbos = 3;
    public int _currTurbo;
    public float turboTime;
    public bool _isJumping = false;


    [Header("Bounds")]
    private float range = 5;

    public bool _isAlive = true;

    private void OnValidate()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (audioSource == null) audioSource = GetComponentInChildren<AudioSource>();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currSpeed = runSpeed;
        _currTurbo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //OnDead();
        if (Input.GetKeyUp(KeyCode.S)) TurboPlayer();
        if (rigidbody.velocity.z == 0) animator.SetTrigger("Idle");
    }

    private void FixedUpdate()
    {
        if (canRun)
        {
            Movement();
            if (rigidbody.velocity.z > 0) animator.SetTrigger("Run");
            Jump();
        }
    }

    [NaughtyAttributes.Button]
    public void StopRun()
    {
        runSpeed = 0;
    }

    [NaughtyAttributes.Button]
    public void BackRun()
    {
        runSpeed = 5;
    }

    public void Movement()
    {
        //Move Forward
        transform.position += Vector3.forward * runSpeed * Time.deltaTime;
        //SFXPool.Instance.Play(SFXType.FOOTSTEPS);

        //Move Sides
        float horizontalInputs = Input.GetAxis("Horizontal");
        float verticalInputs = Input.GetAxis("Vertical");

        if (_isJumping == false) transform.Translate(Vector3.right * -sideSpeed * Time.deltaTime * horizontalInputs);

        //Bound
        if (transform.position.x > range)
        {
            transform.position = new Vector3(range, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -range)
        {
            transform.position = new Vector3(-range, transform.position.y, transform.position.z);
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isJumping == false)
        {
            rigidbody.velocity = Vector3.up * jumpForce;
            //animator.SetTrigger("Jump");
            _isJumping = true;
            Invoke(nameof(NotJumping), delayBetweensJumps);

            transform.DOScaleX(scaleX, .2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
            transform.DOScaleY(scaleY, .2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBack);
        }
    }

    public void NotJumping()
    {
        _isJumping = false;
        animator.SetTrigger("Run");
    }

    public void TurboPlayer()
    {
        if (_currTurbo < maxTurbos && _isJumping == false)
        {
            StartCoroutine(TurboCoroutine());
            _currTurbo++;
            ItemManager.Instance.RemoveTurbo();
            Debug.Log("Turbo");
        }
        else Debug.Log("Without turbo");
    }

    public void Dead()
    {
        _isAlive = false;
        canRun = false;
        OnDead();
    }

    public void OnDead()
    {
        runSpeed = 0;
        EnableRagDoll();
        animator.SetTrigger("Die");
        Invoke(nameof(ShowEndGameScreen), 2);
    }

    public void ShowEndGameScreen()
    {
        GameManager.Instance.EndGame();
    }

    public void EnableRagDoll()
    {
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
    }

    public IEnumerator TurboCoroutine()
    {
        runSpeed = turboSpeed;
        yield return new WaitForSeconds(turboTime);
        runSpeed = _currSpeed;
        StopCoroutine(TurboCoroutine());
    }
}
