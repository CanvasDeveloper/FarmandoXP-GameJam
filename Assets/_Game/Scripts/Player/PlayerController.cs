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

[RequireComponent(typeof(InputReference))]
public class PlayerController : MonoBehaviour
{

    [Header("Player Status")]
    public GameObject patrono;
    public int playerLevel = 1;
    public float moveSpeed = 5f;



    [Header("Rotate Sprites")]
    [SerializeField] private SpriteRotationHandler playerSpriteRotation;
    [SerializeField] private SpriteRotationHandler gunSpriteRotation;


    [Header("Bullets")]
    //Para fins de teste
    public GameObject bulletPrefab;
    public Transform[] bulletPoisiton;
    [SerializeField] private Transform gunPivot;

    //Ambos ainda colocados aqui para registro de variaveis
    public float bulletSpeed = 5f;
    public float BulletDamage = 5f;
    [SerializeField] private float reloadWeapon = 5f;
    [SerializeField] public float delayWeapon = 5f;

    private bool isCanShoot = true;
    private Camera main;

    private Vector3 mouseWorldPosition;
    [SerializeField] private bool isLookingLeft;

    private InputReference _inputReference;
    private Rigidbody2D _rigidbody2D;

    private IDamageable health;

    public Animator player;
    public Animator gun;

    [SerializeField] private bool isStunned;
    [SerializeField] private float stunnedTime = 1;
    [SerializeField] private float stunForce = 15;

    private void Awake()
    {
        _inputReference = GetComponent<InputReference>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        health = GetComponent<IDamageable>();
    }

    private void Start()
    {
        main = Camera.main;
    }

    private void Stun(Vector3 direction)
    {
        //isStunned = true;

        var dir = transform.position - direction;

        _rigidbody2D.AddForce(dir.normalized * stunForce);

        StartCoroutine(IE_Stun());
    }

    private IEnumerator IE_Stun()
    {
        yield return new WaitForSeconds(stunnedTime);

    }

    private void Update()
    {
        if (health.IsDie)
            return;

        if (_inputReference.PauseButton.IsPressed)
        {
            //Preguica :)
            Debug.Log("Pause");
            GameManager.Instance.PauseResume();
        }

        if (GameManager.Instance && GameManager.Instance.Paused)
            return;

        if (isStunned)
            return;

        mouseWorldPosition = main.ScreenToWorldPoint(_inputReference.MousePosition);

        UpdatePlayerRotation();
        UpdateSpriteSide();

        if (_inputReference.ShootButton.IsPressed)
        {
            if (!isCanShoot)
                return;
           

            Debug.Log("apertou mouse");
            StartCoroutine(IE_CanShoot());
            gun.SetTrigger("attack");
        }
    }
    
    private void FixedUpdate()
    {
        if (isStunned || health.IsDie)
            return;

        _rigidbody2D.velocity = _inputReference.Movement * moveSpeed;
        if(_rigidbody2D.velocity !=  new Vector2(0,0))
        {
            player.SetBool("isWalk", true);
        }
        else
        {
            player.SetBool("isWalk", false);
        } 
    }

    /// <summary>
    /// Rotaciona o player
    /// </summary>
    private void UpdatePlayerRotation()
    {
        Vector3 targetDirection = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        gunPivot.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    /// <summary>
    /// Flipa a arma e o player
    /// </summary>
    private void UpdateSpriteSide()
    {
        //olhando para esquerda, mouse na direita
        if (isLookingLeft && mouseWorldPosition.x > transform.position.x)
        {
            isLookingLeft = false;
            playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.startRotation);
            gunSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(gunSpriteRotation.startRotation);
        }

        if(!isLookingLeft && mouseWorldPosition.x < transform.position.x)
        {
            isLookingLeft = true;
            playerSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(playerSpriteRotation.targetRotation);
            gunSpriteRotation.sprite.transform.localRotation = Quaternion.Euler(gunSpriteRotation.targetRotation);
        }
    }

    private IEnumerator IE_CanShoot()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(reloadWeapon);
        isCanShoot = true;
    }
}
