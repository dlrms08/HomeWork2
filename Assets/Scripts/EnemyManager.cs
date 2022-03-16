using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int maxLife;
    private int currentLife;
    private ObjPool explosionPool;
    public bool canAttack;
    private Camera camera;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentLife = maxLife;
    }

    private void Start()
    {
        explosionPool = GameObject.Find("ExplosionPool").GetComponent<ObjPool>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        canAttack = onScreen;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            currentLife--;
            if(currentLife <=0)
            {
                DoExplosion();
            }    
        }
    }

    void DoExplosion()
    {
        GameObject obj = explosionPool.GetPooledObject();
        if (obj == null)
            return;

        obj.transform.position = transform.position;
        obj.SetActive(true);
        gameObject.SetActive(false);
    }

}
