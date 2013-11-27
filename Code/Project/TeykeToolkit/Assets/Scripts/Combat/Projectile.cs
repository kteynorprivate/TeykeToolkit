using UnityEngine;
using System.Collections;
using Teyke;

namespace Teyke
{
	public class Projectile : MonoBehaviour 
	{
		public float speed;
		public float radius;
	
		private float damage;
		private GameEntity target;
	
		void Start () 
		{
	
		}
	
		public void Fire(GameEntity t, float d)
		{
			target = t;
			damage = d;
		}
	
		void Update () 
		{
			if (target == null) 
			{
				Destroy (this.gameObject);
				return;
			}

			transform.Translate ((target.transform.position - transform.position).normalized * speed * Time.deltaTime);
			transform.LookAt(target.transform.position);

			if ((transform.position - target.transform.position).sqrMagnitude < radius * 2) 
			{
				// projectile hit
				target.ApplyDamage(damage);
				Destroy(this.gameObject);
			}
		}

		public static void FireNew(Projectile proj, GameEntity t, float d)
		{
			Projectile newProj = Instantiate (proj) as Projectile;
			newProj.Fire(t, d);
		}
	}
}
