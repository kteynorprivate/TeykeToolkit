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
		public Vector3 tpos;
	
		void Start () 
		{

		}
	
		public void Fire(GameEntity t, float d)
		{
			target = t;
			tpos = target.transform.position;
			damage = d;
		}
	
		void Update () 
		{
			if (target != null) 
			{
				tpos = target.transform.position;
			}
			transform.LookAt(tpos);				
			

			transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));

			float dst = (transform.position - tpos).sqrMagnitude;
			if (dst < radius * 2 || dst > 10000) 	// TODO: fix this magic number. find a better way to check if the target is borked.
			{
				// projectile hit
				if(target != null) target.ApplyDamage(damage);
				Destroy(this.gameObject);
			}
		}

		public static void FireNew(Projectile proj, Vector3 origin, GameEntity t, float d)
		{
			Projectile newProj = Instantiate(proj) as Projectile;
			newProj.transform.position = origin;
			newProj.Fire(t, d);
		}
	}
}
