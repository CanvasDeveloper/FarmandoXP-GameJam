using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [HideInInspector] public float speedBullet = 4f;
    [SerializeField] private float lifeTime = 4f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IE_LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speedBullet * Time.deltaTime);
    }

    private IEnumerator IE_LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
