using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SpriteRotationHandler
{
    public GameObject sprite;
    public Vector3 startRotation;
    public Vector3 targetRotation;
}

[RequireComponent(typeof(InputReference), typeof(HealthSystem))]
public class PlayerController : MonoBehaviour
{
    private static readonly int AttackParam = Animator.StringToHash("attack");
    private static readonly int IsWalkParam = Animator.StringToHash("isWalk");
    private static readonly int IsDieParam = Animator.StringToHash("isDie");
    private static readonly int TakeDamageParam = Animator.StringToHash("takeDamage");
    private static readonly int DirectionXParam = Animator.StringToHash("directionX");
    private static readonly int DirectionYParam = Animator.StringToHash("directionY");

    [Header("Player")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float moveSpeed = 5f;
    
    //[SerializeField] private bool isLookingLeft;

    [Header("Rotate Sprites")]
    [SerializeField] private SpriteRotationHandler playerSpriteRotation;

    [Header("Bullets")]
    public BulletController bulletPrefab;

    [SerializeField] private Transform gunPivot;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int maxBullets = 10;
    [SerializeField] public float delayToReload = 0.2f;

    private int _currentBullets;
    private bool _isCanShoot = true;
    
    private Camera _main;
    private Vector3 _mouseWorldPosition;
    private Vector2 _targetDirection;

    private InputReference _inputReference;
    private Rigidbody2D _rigidbody2D;

    private IDamageable _health;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _health = GetComponent<IDamageable>();
    }

    private void Start()
    {
        _main = Camera.main;
        _currentBullets = maxBullets;
    }

    private void OnEnable()
    {
        _health.OnTakeDamage += ChangeToTakeDamageAnimation;
        _health.OnDie += ChangeToDieAnimation;
    }

    private void OnDisable()
    {
        _health.OnTakeDamage -= ChangeToTakeDamageAnimation;
        _health.OnDie -= ChangeToDieAnimation;
    }    

    private void Update()
    {
        if (_health.IsDie)
            return;

        PauseInputTrigger();

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

        _mouseWorldPosition = _main.ScreenToWorldPoint(_inputReference.MousePosition);
        _targetDirection = _inputReference.Movement;

        ShootInputTrigger();

        UpdateAnimator();

        if (!IsMoving())
            return;

        UpdateGunRotation();
        UpdatePlayerScale();
    }

    private void FixedUpdate()
    {
        if (_health.IsDie)
            return;

        _rigidbody2D.velocity = _targetDirection * moveSpeed;
    }

    private bool IsMoving() => _targetDirection != Vector2.zero;

    private void UpdatePlayerScale()
    {
        transform.localScale = new Vector3(Mathf.Sign(_targetDirection.x), 1, 1).normalized;
    }

    private void UpdateGunRotation()
    {
        gunPivot.up = _targetDirection;

        //float angle = Mathf.Atan2(_targetDirection.y, _targetDirection.x) * Mathf.Rad2Deg;
        //gunPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void PauseInputTrigger()
    {
        if (_inputReference.PauseButton.IsPressed)
        {
            GameManager.Instance.PauseResume();
        }
    }

    private void ShootInputTrigger()
    {
        if (_inputReference.ShootButton.IsPressed && _currentBullets > 0)
        {
            if (!_isCanShoot)
                return;

            _currentBullets--;
            StartCoroutine(IE_CanShoot());

            playerAnimator.SetTrigger(AttackParam);
        }
    }

    private IEnumerator IE_CanShoot()
    {
        _isCanShoot = false;

        BulletController bullet = Instantiate(bulletPrefab, gunPivot.position, gunPivot.rotation);
        bullet.speedBullet = bulletSpeed;
        bullet.transform.up = gunPivot.up;

        yield return new WaitForSeconds(delayToReload);
        

        _isCanShoot = true;
    }

    public void AddBullets(int count)
    {
        _currentBullets += count;

        if(_currentBullets >= maxBullets)
        {
            _currentBullets = maxBullets;
        }
    }

    private void ChangeToDieAnimation(IDamageable value)
    {
        playerAnimator.SetBool(IsDieParam, true);
    }

    private void ChangeToTakeDamageAnimation(Vector3 value)
    {
        playerAnimator.SetTrigger(TakeDamageParam);
    }

    private void UpdateAnimator()
    {
        playerAnimator.SetBool(IsWalkParam, _rigidbody2D.velocity != new Vector2(0, 0));

        if (!IsMoving())
            return;

        playerAnimator.SetFloat(DirectionXParam, _targetDirection.x);
        playerAnimator.SetFloat(DirectionYParam, _targetDirection.y);

        //olhando para esquerda, mouse na direita
        //if (isLookingLeft && _mouseWorldPosition.x > transform.position.x)
        //{
        //    isLookingLeft = false;
        //    playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.startRotation);
        //}

        //if(!isLookingLeft && _mouseWorldPosition.x < transform.position.x)
        //{
        //    isLookingLeft = true;
        //    playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.targetRotation);
        //}
    }
}
