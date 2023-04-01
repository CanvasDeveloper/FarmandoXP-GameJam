using System;
using System.Collections;
using System.Data;
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

    [Header("Player")]
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool isLookingLeft;

    [Header("Rotate Sprites")]
    [SerializeField] private SpriteRotationHandler playerSpriteRotation;

    [Header("Bullets")]
    public GameObject bulletPrefab;

    [SerializeField] private Transform gunPivot;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int maxBullets = 10;
    [SerializeField] public float delayToReload = 0.2f;

    private int _currentBullets;
    private bool _isCanShoot = true;
    
    private Camera _main;
    private Vector3 _mouseWorldPosition;

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

    private void ChangeToDieAnimation(IDamageable value)
    {
        animatorPlayer.SetBool(IsDieParam, true);
    }

    private void ChangeToTakeDamageAnimation(Vector3 value)
    {
        animatorPlayer.SetTrigger(TakeDamageParam);
    }

    private void Update()
    {
        if (_health.IsDie)
            return;

        if (_inputReference.PauseButton.IsPressed)
        {
            GameManager.Instance.PauseResume();
        }

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

        _mouseWorldPosition = _main.ScreenToWorldPoint(_inputReference.MousePosition);

        UpdatePlayerRotation();
        UpdateSpriteSide();

        if (_inputReference.ShootButton.IsPressed && _currentBullets > 0)
        {
            if (!_isCanShoot)
                return;

            StartCoroutine(IE_CanShoot());

            animatorPlayer.SetTrigger(AttackParam);
        }
    }
    
    private void FixedUpdate()
    {
        if (_health.IsDie)
            return;

        _rigidbody2D.velocity = _inputReference.Movement * moveSpeed;

        animatorPlayer.SetBool(IsWalkParam, _rigidbody2D.velocity != new Vector2(0, 0));
    }

    public void AddBullets(int count)
    {
        _currentBullets += count;

        if(_currentBullets >= maxBullets)
        {
            _currentBullets = maxBullets;
        }
    }

    /// <summary>
    /// Rotaciona o player
    /// </summary>
    private void UpdatePlayerRotation()
    {
        Vector3 targetDirection = _mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        gunPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    /// <summary>
    /// Flipa a arma e o player
    /// </summary>
    private void UpdateSpriteSide()
    {
        //olhando para esquerda, mouse na direita
        if (isLookingLeft && _mouseWorldPosition.x > transform.position.x)
        {
            isLookingLeft = false;
            playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.startRotation);
        }

        if(!isLookingLeft && _mouseWorldPosition.x < transform.position.x)
        {
            isLookingLeft = true;
            playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.targetRotation);
        }
    }

    private IEnumerator IE_CanShoot()
    {
        _isCanShoot = false;

        yield return new WaitForSeconds(delayToReload);
        
        _isCanShoot = true;
    }
}
