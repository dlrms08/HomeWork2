using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerManager : MonoBehaviour
{
    private Joystick joystick;
    private ObjPool bulletPool;
    public Boundary boundary;
    public Transform shotSpawn;     // the turret (bullet spawn location)

    public float speed;
    public float rotSpeed;
    public float fireRate = 0.5f;
    private float nextFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        bulletPool = GameObject.Find("BulletPool").GetComponent<ObjPool>();
    }

    // Update is called once per frame
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
            GameObject enemy = FindNearestObjectByTag("Enemy");
            if (enemy == null)
                return;

            if (!enemy.GetComponent<EnemyManager>().canAttack)
                return;

            transform.LookAt(enemy.transform);
            //Vector3 dir = enemy.transform.position;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);


            if ((Time.time > nextFire))
            {
                nextFire = Time.time + fireRate;
                //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GameObject obj = bulletPool.GetPooledObject();
                obj.transform.position = shotSpawn.position;
                obj.transform.rotation = shotSpawn.rotation;
                obj.SetActive(true);
                GetComponent<AudioSource>().Play();
            }
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
                    0.0f,
            Mathf.Clamp(transform.position.z, boundary.zMin, boundary.zMax));


            
    }

    private GameObject FindNearestObjectByTag(string tag)
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();

        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {
                return Vector3.Distance(transform.position, obj.transform.position);
            })
        .FirstOrDefault();

        return neareastObject;
    }


}
