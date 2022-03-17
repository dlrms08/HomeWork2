using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public enum BulletType
    {
        Player,
        Enemy
    };

    public BulletType bulletType = BulletType.Player;
    public float speed;
    public float rotSpeed;
    public int maxlife;
    private int currentLife;

    Quaternion q;

    private void Start()
    {
       
    }

    // Use this for initialization
    void OnEnable()
    {
        currentLife = maxlife;

        if (GameManager.instance.skills[3] && bulletType != BulletType.Enemy)
            CheckDirection();    
    }

    void CheckDirection()
    {
        GameObject go = GameSupport.FindNearestObjectByTag("Enemy", transform);
        if (go == null)
            return;

        Vector3 vectorToTarget = go.transform.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg);

        q = Quaternion.AngleAxis(angle, Vector3.up);

        Invoke(nameof(CheckDirection), .3f);

    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotSpeed);

        if (!GameSupport.OnScreen(transform))
            gameObject.SetActive(false);
    }

    public bool LifeCheck()
    {
        currentLife--;
        if(currentLife > 0)
        {
            return true;
        }
        
        return false;
        
    }
}
