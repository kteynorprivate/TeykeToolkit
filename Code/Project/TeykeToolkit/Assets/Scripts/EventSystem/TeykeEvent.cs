using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeykeEventInfo
{
	public GameObject sender;
	public Dictionary<string, Object> info;
	
	public TeykeEventInfo(GameObject sender)
	{
		this.sender = sender;
		info = null;
	}
	public TeykeEventInfo(GameObject sender, Dictionary<string, Object> info)
	{
		this.sender = sender;
		this.info = info;
	}
}

public delegate void TeykeEvent(TeykeEventInfo eventInfo);

public static partial class TeykeEvents
{
	public static event TeykeEvent UnitCreated;
	public static event TeykeEvent UnitDestroyed;
	
	public static void EmitUnitCreated(GameObject createdUnit)
	{
		if(UnitCreated != null)
			UnitCreated(new TeykeEventInfo(createdUnit));
	}
	public static void EmitUnitDestroyed(GameObject destroyedUnit)
	{
		if(UnitDestroyed != null)
			UnitDestroyed(new TeykeEventInfo(destroyedUnit));
	}
}