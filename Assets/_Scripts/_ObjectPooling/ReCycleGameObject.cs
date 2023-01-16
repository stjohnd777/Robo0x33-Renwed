using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface IRecycle {

	void Restart();	

	void ShutDown();
}
/**
 * Attached to the game object, indicating this game object is
 * recycled pooled object
 * 
 * 
 */

namespace sillymutts
{
	public class ReCycleGameObject : MonoBehaviour
	{

		private IList<IRecycle> recycleComponents;

		public void Awake()
		{

			var components = GetComponents<MonoBehaviour>();
			recycleComponents = new List<IRecycle>();
			foreach (var c in components)
			{
				if (c is IRecycle)
				{
					recycleComponents.Add(c as IRecycle);
				}
			}

			//Debug(name +": Found" + recycleComponents.Count +" Recyclables components" );
		}

		public void Restart()
		{

			gameObject.SetActive(true);

			foreach (var component in recycleComponents)
			{
				component.Restart();
			}
		}

		public void ShutDown()
		{

			gameObject.SetActive(false);

			foreach (var component in recycleComponents)
			{
				component.ShutDown();
			}
		}

	}

}
