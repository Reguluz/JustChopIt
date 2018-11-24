using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace System
{
    public class ObjectPool : MonoBehaviour,IPunPrefabPool
	{
		public  Dictionary<string, GameObject> prefabResoucePrefabCache = new Dictionary<string, GameObject>();
		private bool bOnce = false;
		
		public static ObjectPool Current;			//A public static reference to itself (make's it visible to other objects without a reference)
		
		public List<GameObject> Prefabs;				//Collection of prefabs to be poooled
		public List<GameObject>[] PooledObjects;	//The actual collection of pooled objects
		[HideInInspector]
		public int[] AmountToBuffer;				//The amount to pool of each object. This is optional
		private int DefaultBufferAmount = 5;		//Default pooled amount if no amount abaove is supplied
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
			InitialResoucesCache();


		}
	
		public void InitialResoucesCache()
		{
			string prefabTmpName = string.Empty;
			if (!bOnce)
			{
				bOnce = true;
				UnityEngine.Object[] all_resources = UnityEngine.Resources.LoadAll("", typeof(GameObject));
				for (int i = 0; i < all_resources.Length; i++)
				{
					GameObject Go = all_resources[i] as GameObject;
					prefabTmpName = Go.name;
					if (null != Go && !string.IsNullOrEmpty(prefabTmpName))
					{
						if (!prefabResoucePrefabCache.ContainsKey(prefabTmpName))
						{
							prefabResoucePrefabCache.Add(prefabTmpName, Go);
							Prefabs.Add(GetGameObjFromCache(prefabTmpName));
						}
						else
						{
							Debug.LogError(prefabTmpName + " have more than one prefab have the same name ,check all resoures folder.");
						}
					}
				}
			}
			PooledObjects = new List<GameObject>[Prefabs.Count];
			
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
					obj.GetComponent<NetOwnerFinder>()?.Init();
					//...and add it to the pool.
					Destroy(obj);
				}
				//Go to the next prefab in the collection
				index++;
			}
		}

		private GameObject GetGameObjFromCache(string prefabName)
		{  
			GameObject resourceGObj = null;
			if (!prefabResoucePrefabCache.TryGetValue(prefabName, out resourceGObj))
			{   
				Debug.LogError("please check ,if current " + prefabName + "not in resouce folder");
			}

			if (resourceGObj == null)
			{
				Debug.LogError("Could not Instantiate the prefab [" + prefabName + "]. Please verify this gameobject in a Resources folder.");
			}
			return resourceGObj;
		}
		

		public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
		{
			//Loop through the collection of prefabs...
			for(int i=0; i<Prefabs.Count; i++)
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
						GameObject obj = (GameObject)Instantiate(prefab);
						//...give it a name...
						obj.name = prefab.name;
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

		public void Destroy(GameObject gameObj)
		{
			for ( int i=0; i<Prefabs.Count; i++)
			{
				if(Prefabs[i].name == gameObj.name)
				{
					//Deactivate it...
					
					//gameObj.GetComponent<PhotonView>().didAwake = false;
					gameObj.SetActive(false);
					//..parent it to the container...
					gameObj.transform.parent = _containerObject.transform;
					//...and add it back to the pool
					PooledObjects[i].Add(gameObj);
					return;
				}
			}
		}


	}
}