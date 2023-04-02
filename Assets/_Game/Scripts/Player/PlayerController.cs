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
    private static readonly int IsWalkParam = Animator.StringToHash("isWalking");
    private static readonly int IsDieParam = Animator.StringToHash("isDie");
    private static readonly int TakeDamageParam = Animator.StringToHash("takeDamage");

    private static readonly int DashParam = Animator.StringToHash("dash");
    private static readonly int DirectionXParam = Animator.StringToHash("directionX");
    private static readonly int DirectionYParam = Animator.StringToHash("directionY");

    private static readonly int TottemDirectionXParam = Animator.StringToHash("tottemDirectionX");
    private static readonly int TottemDirectionYParam = Animator.StringToHash("tottemDirectionY");
    private static readonly int IsRechargingTottemParam = Animator.StringToHash("isRechargingTottem");

    public event Action<float, float> OnUpdateManaQuantity; //current / max

    [Header("Player")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private float dashDecharge = 0.5f;


    [Header("Bullets")]
    public BulletController bulletPrefab;
    [SerializeField] private Transform gunPivot;

    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float waitForAnimation = 0.2f;
    [SerializeField] private float maxBullets = 10;
    [SerializeField] private float requiredToShoot = 1;
    [SerializeField] public float delayToReload = 0.2f;

    [Header("Bullets Recover")]
    [SerializeField] private float percent = 0.2f;

    [SerializeField] private float currentBullets = 0;
    [SerializeField] private float removingBulletsDelay = 0.5f;

    [Header("Gun Pivots")]
    [SerializeField] private Transform gunDownLocation;
    [SerializeField] private Transform gunUpLocation;
    [SerializeField] private Transform gunSideLocation;

    private bool _isCanShoot = true;
    private bool _isDashing = false;
    private bool _isTriggeredMovementAudio = false;
    
    private Vector2 _targetDirection;

    private float _timeStopped;
    private float _timeRemovingBullets;
    private float _timeRecoveringBullets;

    private InputReference _inputReference;
    private Rigidbody2D _rigidbody2D;

    private IDamageable _health;
    private Tottem _currentTottem;

    private bool hasStartTriggerAudio = false;
    private bool hasEndTriggerAudio = false;

    FMOD.Studio.EventInstance instance;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _health = GetComponent<IDamageable>();
    }

    private void Start()
    {
        currentBullets = maxBullets;
        
        UpdateToSide();
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

        _targetDirection = _inputReference.Movement;

        TriggerOnWalkAudio();
        CalculateStoppedTime();

        RechargingInputTrigger();
        
        if (IsRechargingTottem())
            return;

        AutoRecoverMana();
        ShootInputTrigger();

        UpdateAnimator();
        
        if (!IsMoving())
            return;

        UpdatePlayerScale();
        DashInpuTrigger();
        UpdateGunRotation();
    }

    private void RechargingInputTrigger()
    {
        if (IsRechargingTottem())
        {
            _currentTottem.TriggerPlayerRecharged(this, true);

            var tottemDir = (_currentTottem.transform.position - transform.position).normalized;

            playerAnimator.SetFloat(TottemDirectionXParam, tottemDir.x);
            playerAnimator.SetFloat(TottemDirectionYParam, tottemDir.y);

            playerAnimator.SetBool(IsRechargingTottemParam, true);

            RemoveBulletsConstantly();

            if (!hasStartTriggerAudio)
            {
                hasStartTriggerAudio = true;

                instance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Fada/Cast Start");
                instance.start();
            }

            hasEndTriggerAudio = false;
        }
        else if (_currentTottem)
        {
            _currentTottem.TriggerPlayerRecharged(this, false);

            playerAnimator.SetBool(IsRechargingTottemParam, false);

            if (!hasEndTriggerAudio)
            {
                hasEndTriggerAudio = true;

                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Fada/Cast End", transform.position);
            }

            hasStartTriggerAudio = false;
        }
    }

    private void FixedUpdate()
    {
        if (_health.IsDie)
            return;

        if (IsRechargingTottem())
            _rigidbody2D.velocity = Vector2.zero;

        if (_isDashing)
            return;

        _rigidbody2D.velocity = _targetDirection * moveSpeed;
    }

    private void TriggerOnWalkAudio()
    {
        if (_targetDirection != Vector2.zero && !_isTriggeredMovementAudio)
        {
            _timeStopped = 0f;
            _isTriggeredMovementAudio = true;
            TriggerAudio();
        }
    }

    private void TriggerAudio()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Fada/Wings", transform.position);
    }

    private void CalculateStoppedTime()
    {
        if (_targetDirection == Vector2.zero)
        {
            _timeStopped += Time.deltaTime;

            if (_timeStopped > 0.2f)
                _isTriggeredMovementAudio = false;
        }
    }

    private bool IsMoving() => _targetDirection != Vector2.zero;

    private void UpdatePlayerScale()
    {
        transform.localScale = new Vector3(Mathf.Sign(_targetDirection.x), 1, 1);
    }

    #region GUNPOSITION AND ROTATION

    public void UpdateToDown() => UpdateGunPosition(gunDownLocation.position);
    public void UpdateToUp() => UpdateGunPosition(gunUpLocation.position);
    public void UpdateToSide() => UpdateGunPosition(gunSideLocation.position);

    public void UpdateGunPosition(Vector3 newPosition)
    {
        gunPivot.position = newPosition;
    }

    private void UpdateGunRotation()
    {
        gunPivot.up = _targetDirection;
    }

    #endregion

    public bool HasBullets() => currentBullets > 0;

    public bool IsPressingRechargeKey() => _inputReference.RechargeTottemButton.IsPressed;
    public bool IsRechargingTottem()
    {
        if (_currentTottem != null && _currentTottem.IsSubTotem() && _currentTottem.IsValidTotem())
            return false;

        return _currentTottem != null & IsPressingRechargeKey() && HasBullets() && !_currentTottem.IsCompletedTottem;
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
        if (_inputReference.ShootButton.IsPressed && currentBullets > requiredToShoot && !_isDashing)
        {
            if (!_isCanShoot)
                return;

            StartCoroutine(IE_CanShoot());

            playerAnimator.SetTrigger(AttackParam);
        }
    }
    private void DashInpuTrigger()
    {
        if(_inputReference.DashButton.IsPressed && !_isDashing && currentBullets > 0)
        {
            _isDashing = true;
            currentBullets -= dashDecharge;
            _rigidbody2D.AddForce(_targetDirection * dashForce, ForceMode2D.Impulse);

            StartCoroutine(IE_ResetDash());

            playerAnimator.SetTrigger(DashParam);
        }
    }

    private IEnumerator IE_ResetDash()
    {
        yield return new WaitForSeconds(dashDelay);
        _isDashing = false;
    }

    private IEnumerator IE_CanShoot()
    {
        _isCanShoot = false;

        yield return new WaitForSeconds(waitForAnimation);

        RemoveBullets(requiredToShoot);

        BulletController bullet = Instantiate(bulletPrefab, gunPivot.position, gunPivot.rotation);
        bullet.speedBullet = bulletSpeed;
        bullet.transform.up = gunPivot.up;

        yield return new WaitForSeconds(delayToReload);

        _isCanShoot = true;
    }

    public void AddMana(float amount)
    {
        if (currentBullets >= maxBullets)
            return;

        currentBullets += amount;

        if(currentBullets >= maxBullets)
            currentBullets = maxBullets;

        OnUpdateManaQuantity?.Invoke(currentBullets, maxBullets);
    }

    public void RemoveBulletsConstantly()
    {
        if(currentBullets <= 0)
            return;

        _timeRemovingBullets += Time.deltaTime;

        if (_timeRemovingBullets > removingBulletsDelay)
        {
            RemoveBullets(requiredToShoot);
            _timeRemovingBullets = 0;
        }
    }

    private void RemoveBullets(float amount)
    {
        currentBullets -= amount;

        if (currentBullets < 0)
            currentBullets = 0;

        OnUpdateManaQuantity?.Invoke(currentBullets, maxBullets);
    }

    private void AutoRecoverMana()
    {
        _timeRecoveringBullets += Time.deltaTime;

        if (_timeRecoveringBullets > 0.5f)
        {
            AddMana((percent * maxBullets)/2);

            _timeRecoveringBullets = 0;
        }
    }

    public void SetTottem(Tottem tottem)
    {
        _currentTottem = tottem;
    }

    #region ANIMATION

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
    }

    #endregion

    
}
