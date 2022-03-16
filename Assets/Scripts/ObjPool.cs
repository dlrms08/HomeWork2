using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjPool : MonoBehaviour {

	public GameObject pooledObject;
	public int pooledAmount = 20;
	public bool willGrow = true;
	public List<GameObject> pooledObjects;

    private void Start()
    {
		Init();
    }

    public void Init()
	{
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.transform.parent = transform;
			obj.SetActive (false);

			pooledObjects.Add (obj);
		}
	}

	public GameObject GetPooledObject()
	{
		for (int i = 0; i < pooledObjects.Count; i++) {	
			if (!pooledObjects [i].activeInHierarchy) {

				return pooledObjects[i];
			}
		}

		if(willGrow)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			obj.transform.parent = transform;
			pooledObjects.Add(obj);
			return obj;
		}

		return null;
	}

	public void ObjReset()
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			pooledObjects[i].SetActive (false);
		}
	}
	public void SetColor(Material mat)
    {
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			pooledObjects[i].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.color = mat.color;
		}
	}
}
	