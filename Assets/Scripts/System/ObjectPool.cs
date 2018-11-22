using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace System
{
    public class ObjectPool : MonoBehaviour,IPunPrefabPool
	{
		public static ObjectPool Current;			//A public static reference to itself (make's it visible to other objects without a reference)
		public GameObject[] Prefabs;				//Collection of prefabs to be poooled
		public List<GameObject>[] PooledObjects;	//The actual collection of pooled objects
		public int[] AmountToBuffer;				//The amount to pool of each object. This is optional
		public int DefaultBufferAmount = 10;		//Default pooled amount if no amount abaove is supplied
		public bool CanGrow = true;					//Whether or not the pool can grow. Should be off for final builds
	
		GameObject _containerObject;					//A parent object for pooled objects to be nested under. Keeps the hierarchy clean
	
	
		void Awake ()
		{
			PhotonNetwork.PrefabPool = this;
			//Ensure that there is only one object pool
			if (Current == null)
				Current = this;
			else
				Destroy(gameObject);
			//Create new container
			_containerObject = new GameObject("ObjectPool");
			//Create new list for objects
			PooledObjects = new List<GameObject>[Prefabs.Length];
			
			int index = 0;
			//For each prefab to be pooled...
			foreach ( GameObject objectPrefab in Prefabs )
			{
				//...create a new array for the objects then...
				PooledObjects[index] = new List<GameObject>(); 
				//...determine the amount to be created then...
				int bufferAmount;
				if(index < AmountToBuffer.Length) 
					bufferAmount = AmountToBuffer[index];
				else
					bufferAmount = DefaultBufferAmount;
				//...loop the correct number of times and in each iteration...
				for ( int i = 0; i < bufferAmount; i++)
				{
					//...create the object...
					GameObject obj = (GameObject)Instantiate(objectPrefab);
					//...give it a name...
					obj.name = objectPrefab.name;
					//...and add it to the pool.
					PoolObject(obj);
				}
				//Go to the next prefab in the collection
				index++;
			}
		}
	
		
	
		public void PoolObject ( GameObject obj )
		{
			//Find the correct pool for the object to go in to
			for ( int i=0; i<Prefabs.Length; i++)
			{
				if(Prefabs[i].name == obj.name)
				{
					//Deactivate it...
					obj.SetActive(false);
					//..parent it to the container...
					obj.transform.parent = _containerObject.transform;
					//...and add it back to the pool
					PooledObjects[i].Add(obj);
					return;
				}
			}
		}

		public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
		{
			//Loop through the collection of prefabs...
			for(int i=0; i<Prefabs.Length; i++)
			{
				//...to find the one of the correct type
				GameObject prefab = Prefabs[i];
				if(prefab.name == prefabId)
				{
					
					//If there are any left in the pool...
					if(PooledObjects[i].Count > 0)
					{
						
						//...get it...
						GameObject pooledObject = PooledObjects[i][0];
						pooledObject.name = Prefabs[i].name;
						pooledObject.transform.position = position;
						pooledObject.transform.rotation = rotation;
						pooledObject.GetComponent<PhotonView>().didAwake = true;
						pooledObject.SetActive(true);
						//...remove it from the pool...
						PooledObjects[i].RemoveAt(0);
						//...remove its parent...
						pooledObject.transform.parent = null;
						//...and return it
						return pooledObject;
						
					}
					//Otherwise, if the pool is allowed to grow...
					else if(CanGrow) 
					{
						//...write it to the log (so we know to adjust our values...
						Debug.Log("pool grew when requesting: " + prefabId + ". consider expanding default size.");
						//...create a new one...
						GameObject obj = (GameObject)UnityEngine.Resources.Load(prefabId, typeof(GameObject));
						//...give it a name...
						obj.name = Prefabs[i].name;
						obj.transform.position = position;
						obj.transform.rotation = rotation;
						
						//...and return it.
						return obj;
					}
					//If we found the correct collection but it is empty and can't grow, break out of the loop
					break;
					
				}
			}
	
			return null;
		}

		public void Destroy(GameObject gameObject)
		{
			for ( int i=0; i<Prefabs.Length; i++)
			{
				if(Prefabs[i].name == gameObject.name)
				{
					//Deactivate it...
					
					gameObject.GetComponent<PhotonView>().didAwake = false;
					gameObject.SetActive(false);
					//..parent it to the container...
					gameObject.transform.parent = _containerObject.transform;
					//...and add it back to the pool
					PooledObjects[i].Add(gameObject);
					return;
				}
			}
		}
	}
}