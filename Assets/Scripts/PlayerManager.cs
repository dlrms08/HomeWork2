using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    private Joystick joystick;
    private ObjPool explosionPool;
    private ObjPool bulletPool;
    private LevelManager levelManager;

    public float maxHp;
    private float currentHp;
    private GameObject hpBarObj;
    private Image hpSlider;

    public Transform shotSpawn;     // the turret (bullet spawn location)
    public Transform[] shotPositions;

    public float speed;
    public float rotSpeed;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        explosionPool = GameObject.Find("ExplosionPool").GetComponent<ObjPool>();
        bulletPool = GameObject.Find("BulletPool").GetComponent<ObjPool>();
        hpBarObj = GameObject.Find("PlayerCanvas/PlayerHp");
        hpSlider = hpBarObj.transform.GetChild(0).GetComponent<Image>();
        currentHp = maxHp;
        hpSlider.fillAmount = currentHp / maxHp;
        levelManager = FindObjectOfType<LevelManager>();
        SetUIPos();
    }

    // Update is called once per frames
    void Update()
    {
        float x = joystick.Horizontal();
        float y = joystick.Vertical();

        // if keyboard direction key is pressed
        if (x != 0 || y != 0)
        {
            Vector3 dir = new Vector3(x, 0, y);
            transform.position += new Vector3(x, 0.0f, y) * speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
        }
        else
        {
            GameObject enemy = GameSupport.FindNearestObjectByTag("Enemy", transform);
            if (enemy == null)
            {
                GameManager.instance.GameOver();
                return;
            }

            if (!enemy.GetComponent<EnemyManager>().canAttack)
                return;

            transform.LookAt(enemy.transform);

            if ((Time.time > nextFire))
            {
                nextFire = Time.time + fireRate;
                if (GameManager.instance.skills[0])
                    FireDouble();
                else
                    FireBasic();

                if (GameManager.instance.skills[1])
                    FireThree();
            }
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, levelManager.GetLevelInfo().xMin, levelManager.GetLevelInfo().xMax),
                    0.0f,
            Mathf.Clamp(transform.position.z, levelManager.GetLevelInfo().zMin, levelManager.GetLevelInfo().zMax));


        SetUIPos();
            
    }

    //기본샷 발사 
    private void FireBasic()
    {
        MakeBullet(shotPositions[0]);
        GetComponent<AudioSource>().Play();
    }

    //더블샷 발사 
    private void FireDouble()
    {
        for(int i = 1; i < 3; i++)
        {
            MakeBullet(shotPositions[i]);
        }
        GetComponent<AudioSource>().Play();

    }

    //3웨이 발사 
    private void FireThree()
    {
        for (int i = 3; i < 5; i++)
        {
            MakeBullet(shotPositions[i]);
        }
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
        hpBarObj.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 0, -1.5f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && other.GetComponent<BulletScript>().bulletType != BulletScript.BulletType.Player)
        {
            other.gameObject.SetActive(false);   

            currentHp--;
            LifeCheck();
        }
    }

    void LifeCheck()
    {
        Debug.Log(currentHp / maxHp);
        hpSlider.fillAmount = currentHp / maxHp;
        if (currentHp <= 0)
        {
            GameManager.instance.GameOver();
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
}
