using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace sillymutts
{



	public class ObjectPool : MonoBehaviour
	{

		public ReCycleGameObject prefab;

		private List<ReCycleGameObject> poolInstances = new List<ReCycleGameObject>();

		public void Clear()
		{

			foreach (ReCycleGameObject rgo in poolInstances)
			{
				GameObject.Destroy(rgo);
			}

			poolInstances = new List<ReCycleGameObject>();
		}

		/*
		 * Create an instance of the prefab, a clone of the prefab
		 */
		private ReCycleGameObject Create(Vector3 pos)
		{
			// ovoid the circular vis calling GameObjectUtily, directly instanciate
			var clone = GameObject.Instantiate(prefab);
			clone.transform.position = pos;
			// all object live under the pool in the hierarchy
			clone.transform.parent = transform;
			poolInstances.Add(clone);
			return clone;
		}

		/*
		 * Get object from the pool if one is avalible
		 */
		public ReCycleGameObject NextGameObject(Vector3 pos)
		{

			ReCycleGameObject instance = null;

			// look in the pool for an avalible game object
			foreach (var go in poolInstances)
			{
				if (go.gameObject.activeSelf != true)
				{
					instance = go;
					instance.transform.position = pos;
				}
			}

			// did we find one
			if (instance == null)
			{
				// no, scale up to demand and crate another object
				instance = Create(pos);
			}

			// activate the game object
			instance.Restart();
			return instance;
		}
	}

}
