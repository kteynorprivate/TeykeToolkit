using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// Heavily based on http://wiki.unity3d.com/index.php?title=CSharpMessenger

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);

/// <summary>
/// A Messenger structure for events that require no parameters
/// </summary>
public static class Messenger
{
	private static Dictionary<string, Delegate> registeredEvents = new Dictionary<string, Delegate>();
	
	/// <summary>
	/// Registers a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void RegisterListener(string eventName, Callback handler)
	{
		// if the eventName does not exist in the registry, add it.
		if(!registeredEvents.ContainsKey(eventName))
			registeredEvents.Add(eventName, null);
		
		// add the handler for the event.
		registeredEvents[eventName] = (Callback)registeredEvents[eventName] + handler;
	}
	/// <summary>
	/// Unregisters a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void UnregisterListener(string eventName, Callback handler)
	{
		// exit if the event does not exist in the registry.
		if(!registeredEvents.ContainsKey(eventName)) return;
		
		registeredEvents[eventName] = (Callback)registeredEvents[eventName] - handler;
		
		// if the event is empty, remove it from the registry.
		if(registeredEvents[eventName] == null)
			registeredEvents.Remove(eventName);
	}
	
	/// <summary>
	/// Invoke the event with stored in the register by key eventName.
	/// </summary>
	/// <param name='eventName'>
	/// The name of the event in the register to fire.
	/// </param>
	public static void Invoke(string eventName)
	{
		// exit if the event has no registered listeners (will not exist in the dictionary).		
		Delegate d;
		if(registeredEvents.TryGetValue(eventName, out d))
		{
			Callback callback = (Callback)d;
			if(callback != null)
				callback();
		}
	}
}

/// <summary>
/// A Messenger structure for events that require one parameter of type T
/// </summary>
public static class Messenger<T>
{
	// register for all the events
	private static Dictionary<string, Delegate> registeredEvents = new Dictionary<string, Delegate>();
	
	/// <summary>
	/// Registers a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void RegisterListener(string eventName, Callback<T> handler)
	{
		// if the eventName does not exist in the registry, add it.
		if(!registeredEvents.ContainsKey(eventName))
			registeredEvents.Add(eventName, null);
		
		// add the handler for the event.
		registeredEvents[eventName] = (Callback<T>)registeredEvents[eventName] + handler;
	}
	/// <summary>
	/// Unregisters a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void UnregisterListener(string eventName, Callback<T> handler)
	{
		// exit if the event does not exist in the registry.
		if(!registeredEvents.ContainsKey(eventName)) return;
		
		registeredEvents[eventName] = (Callback<T>)registeredEvents[eventName] - handler;
		
		// if the event is empty, remove it from the registry.
		if(registeredEvents[eventName] == null)
			registeredEvents.Remove(eventName);
	}
	
	/// <summary>
	/// Invoke the event with stored in the register by key eventName with the provided parameter.
	/// </summary>
	/// <param name='eventName'>
	/// The name of the event in the register to fire.
	/// </param>
	/// <param name='arg'>
	/// The argument of type T to pass to all the registered event listeners.
	/// </param>
	public static void Invoke(string eventName, T arg)
	{		
		// exit if the event has no registered listeners (will not exist in the dictionary).		
		Delegate d;
		if(registeredEvents.TryGetValue(eventName, out d))
		{
			Callback<T> callback = (Callback<T>)d;
			Debug.Log(callback);
			if(callback != null)
				callback(arg);
		}
	}
}

/// <summary>
/// A Messenger structure for events that require two parameters of type T and U
/// </summary>
public static class Messenger<T, U>
{
	// register for all the events
	private static Dictionary<string, Delegate> registeredEvents = new Dictionary<string, Delegate>();
	
	/// <summary>
	/// Registers a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void RegisterListener(string eventName, Callback<T, U> handler)
	{
		// if the eventName does not exist in the registry, add it.
		if(!registeredEvents.ContainsKey(eventName))
			registeredEvents.Add(eventName, null);
		
		// add the handler for the event.
		registeredEvents[eventName] = (Callback<T, U>)registeredEvents[eventName] + handler;
	}
	/// <summary>
	/// Unregisters a listener.
	/// </summary>
	/// <param name='eventName'>
	/// Dictionary key name of the event.
	/// </param>
	/// <param name='handler'>
	/// Event handler that will be called when the event is fired.
	/// </param>
	public static void UnregisterListener(string eventName, Callback<T, U> handler)
	{
		// exit if the event does not exist in the registry.
		if(!registeredEvents.ContainsKey(eventName)) return;
		
		registeredEvents[eventName] = (Callback<T, U>)registeredEvents[eventName] - handler;
		
		// if the event is empty, remove it from the registry.
		if(registeredEvents[eventName] == null)
			registeredEvents.Remove(eventName);
	}
	
	/// <summary>
	/// Invoke the event with stored in the register by key eventName with the provided parameter.
	/// </summary>
	/// <param name='eventName'>
	/// The name of the event in the register to fire.
	/// </param>
	/// <param name='arg1'>
	/// The argument of type T to pass to all the registered event listeners.
	/// </param>
	/// <param name='arg2'>
	/// The argument of type U to pass to all the registered event listeners.
	/// </param>
	public static void Invoke(string eventName, T arg1, U arg2)
	{
		// exit if the event has no registered listeners (will not exist in the dictionary).		
		Delegate d;
		if(registeredEvents.TryGetValue(eventName, out d))
		{
			Callback<T, U> callback = (Callback<T, U>)d;
			if(callback != null)
				callback(arg1, arg2);
		}
	}
}