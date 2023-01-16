using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace sillymutts
{
	
/*
 * 
 */
	public class GameObjectUtility
	{ 
	/**
	 * ReCycleGameObject to ObjectPool
	 */
		private static Dictionary<ReCycleGameObject, ObjectPool>
			pools = new Dictionary<ReCycleGameObject, ObjectPool>();


		public static void ClearPools()
		{
			foreach (KeyValuePair<ReCycleGameObject, ObjectPool> entry in pools)
			{
				entry.Value.Clear();
				//pools = new Dictionary<ReCycleGameObject, ObjectPool>();
			}

			pools = new Dictionary<ReCycleGameObject, ObjectPool>();

		}

		/**
	 * Get ObjectPool associated with GameObject (ReCycleGameObject)
	*/
		private static ObjectPool GetObjectPool(ReCycleGameObject prefabGameObject)
		{
			ObjectPool pool = null;
			// some techical details, the key in the dictionary 
			// must exist otherwise asking this dictionary for the 
			// object associated with this key will through an exception
			if (pools.ContainsKey(prefabGameObject))
			{
				pool = pools[prefabGameObject];
			}
			else
			{
				// worthy tecnique
				// 1> create an empty game object and with name
				// 2> add component to empty game object
				// return that component
				var poolContainer = new GameObject(prefabGameObject.gameObject.name + "ObjectPool");
				// add the script
				pool = poolContainer.AddComponent<ObjectPool>();
				// set the prrefab
				pool.prefab = prefabGameObject;
				// add to the dictionary
				pools.Add(prefabGameObject, pool);
			}

			return pool;
		}

		public static GameObject Instantiate(GameObject prefab, Vector3 pos)
		{

			GameObject instance = null;
			// is this game object pooled
			var recycled = prefab.GetComponent<ReCycleGameObject>();
			if (recycled)
			{
				// yes it is , get the pool
				ObjectPool pool = GetObjectPool(recycled);
				instance = pool.NextGameObject(pos).gameObject;
			}
			else
			{
				// no, create ana new instance
				instance = GameObject.Instantiate(prefab);
				instance.transform.position = pos;
			}

			//GameObjectUtility.ReferenceEquals(instance.gameObject);

			return instance;
		}

		// TODO
		public static GameObject Instantiate(GameObject prefab, Vector3 pos, Quaternion rotation)
		{

			GameObject instance = null;
			// is this game object pooled
			var recycled = prefab.GetComponent<ReCycleGameObject>();
			if (recycled)
			{
				// yes it is , get the pool
				ObjectPool pool = GetObjectPool(recycled);
				instance = pool.NextGameObject(pos).gameObject;
			}
			else
			{
				// no, create ana new instance
				instance = GameObject.Instantiate(prefab);
				instance.transform.position = pos;
			}

			instance.transform.rotation = rotation;
			//GameObjectUtility.ReferenceEquals(instance.gameObject);

			return instance;
		}


		/*
		 * All GameOject are destroyed throught this method
		 */
		public static void Destroy(GameObject go)
		{

			// does the game object host this script
			var recycle = go.GetComponent<ReCycleGameObject>();
			if (recycle != null)
			{
				// polled object
				recycle.ShutDown();
			}
			else
			{
				// no pooled object
				GameObject.Destroy(go);
			}

		}


		public void ReSizeColliderToSpriteBounds(GameObject target)
		{

			var renderer = target.GetComponent<SpriteRenderer>();
			if (renderer != null)
			{
				var collider = target.GetComponent<BoxCollider>();
				if (collider != null)
				{
					collider.size = renderer.bounds.size;
				}
			}
		}
	}

}