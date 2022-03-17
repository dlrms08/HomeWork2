using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public float maxLife;
    private float currentLife;
    public float fireRate = 3.0f;
    private float nextFire = 0.0f;
    public Transform shotSpawn;

    private ObjPool explosionPool;
    private ObjPool bulletPool;
    private ObjPool hpBarPool;
    private GameObject hpBarObj;
    private Image hpSlider;

    public float speed;
    public float rotSpeed;
    public bool canAttack;
    Quaternion q;

    private void Awake()
    {
        explosionPool = GameObject.Find("ExplosionPool").GetComponent<ObjPool>();
        bulletPool = GameObject.Find("EnemyBulletPool").GetComponent<ObjPool>();
        hpBarPool = GameObject.Find("EnemyCanvas").GetComponent<ObjPool>();
        shotSpawn = transform.GetChild(0).transform;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentLife = maxLife;

        hpBarObj = hpBarPool.GetPooledObject();
        hpBarObj.SetActive(true);
        hpSlider = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        LifeCheck();
        canAttack = false;
        CheckDirection();
        SetUIPos();
    }

    // Update is called once per frame
    void Update()
    {
        SetUIPos();
        canAttack = GameSupport.OnScreen(transform);

        if (!canAttack)
            return;

        if ((Time.time > nextFire))
        {
            nextFire = Time.time + fireRate;
            Fire();
        }
        else
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet") && other.GetComponent<BulletScript>().bulletType != BulletScript.BulletType.Enemy)
        {
            if(!GameManager.instance.skillInfos[2].enable)
                other.gameObject.SetActive(false);
            else
            {
                if(!other.gameObject.GetComponent<BulletScript>().LifeCheck())
                {
                    other.gameObject.SetActive(false);
                }
            }

            currentLife--;
            LifeCheck();
        }
    }

    void CheckDirection()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go == null)
            return;

        Vector3 vectorToTarget = go.transform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg) - 180;

        q = Quaternion.AngleAxis(angle, Vector3.up);

        Invoke(nameof(CheckDirection), .3f);
            
    }

    void LifeCheck()
    {
        Debug.Log(currentLife / maxLife);
        hpSlider.fillAmount = currentLife / maxLife;
        if (currentLife <= 0)
        {
            GameManager.instance.AddExp(1);
            DoExplosion();
        }
    }

    void DoExplosion()
    {
        GameObject obj = explosionPool.GetPooledObject();
        if (obj == null)
            return;

        obj.transform.position = transform.position;
        obj.SetActive(true);
        hpBarObj.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Fire()
    {
        MakeBullet(shotSpawn);
        GetComponent<AudioSource>().Play();
    }

    //총알 생성 
    private void MakeBullet(Transform t)
    {
        GameObject obj = bulletPool.GetPooledObject();
        if (obj == null)
            return;

        obj.transform.position = t.position;
        obj.transform.rotation = t.rotation;
        obj.SetActive(true);
    }

    void SetUIPos()
    {
        hpBarObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0, 1.5f));
    }
}
