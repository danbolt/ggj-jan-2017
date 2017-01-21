// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueStore.cs" company="TODO: NAME">
//   Copyright (c) 2016 TODO: NAME. All rights reserved.
// </copyright>
// <summary>
//   Defines the implementation of the ValueStore class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the implementation of the ValueStore class.
/// </summary>
public class ValueStore
{
	#region Fields

	private Dictionary<System.Type, Dictionary<string, object>> storedData;

	#endregion

	#region Constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="ValueStore"/> class.
	/// </summary>
	public ValueStore()
	{
		this.storedData = new Dictionary<System.Type, Dictionary<string, object>>();
	}

	#endregion

	#region Methods - Public

	/// <summary>Creates a new value in the dataStore</summary>
	/// <typeparam name="T">Type of value to create</typeparam>
	/// <param name="dataKey">the data key to use to create the value.</param>
	public void CreateValue<T>(string dataKey)
	{
		this.CreateValue<T>(dataKey, default(T));
	}

	/// <summary>Creates a new value in the dataStore</summary>
	/// <typeparam name="T">Type of value to create</typeparam>
	/// <param name="dataKey">the data key to use to create the value.</param>
	/// <param name="dataValue">the data value to store.</param>
	public void CreateValue<T>(string dataKey, T dataValue)
	{
		Dictionary<string, object> dataDictionary;
		if (this.storedData.ContainsKey(typeof(T)))
		{
			dataDictionary = this.storedData[typeof(T)];
		}
		else
		{
			dataDictionary = new Dictionary<string, object>();
		}

		if (!dataDictionary.ContainsKey(dataKey))
		{
			dataDictionary.Add(dataKey, dataValue);
			this.storedData[typeof(T)] = dataDictionary;
		}
	}

	/// <summary>Removes a value from the data store</summary>
	/// <typeparam name="T">Type of value to destroy</typeparam>
	/// <param name="dataKey">the data key of the value to be removed</param>
	public void DestroyValue<T>(string dataKey)
	{
		if (this.storedData.ContainsKey(typeof(T)))
		{
			Dictionary<string, object> dataDictionary = this.storedData[typeof(T)];
			if (!dataDictionary.ContainsKey(dataKey))
			{
				dataDictionary.Remove(dataKey);
				this.storedData[typeof(T)] = dataDictionary;
			}
		}
	}

	/// <summary>Gets the value associated with a datakey from the dataStore.</summary>
	/// <typeparam name="T">Type of value to get</typeparam>
	/// <param name="dataKey">the datakey of the value to be searched.</param>
	/// <returns>the value as found in the data store.</returns>
	public T GetValue<T>(string dataKey)
	{
		T result = default(T);

		if (this.storedData.ContainsKey(typeof(T)))
		{
			Dictionary<string, object> dataDictionary = this.storedData[typeof(T)];
			if (dataDictionary.ContainsKey(dataKey))
			{
				result = (T)dataDictionary[dataKey];
			}
			else
			{
				Debug.Log("No such value exists!");
			}
		}

		return result;
	}

	/// <summary>Sets a new value for a given dataKey in the dataStore.</summary>
	/// <typeparam name="T">Type of Value to set</typeparam>
	/// <param name="dataKey">the data key to set the value to.</param>
	/// <param name="newValue">the new value to store.</param>
	public void SetValue<T>(string dataKey, T newValue)
	{
		if (this.storedData.ContainsKey(typeof(T)))
		{
			Dictionary<string, object> dataDictionary = this.storedData[typeof(T)];
			if (dataDictionary.ContainsKey(dataKey))
			{
				dataDictionary[dataKey] = newValue;
				this.storedData[typeof(T)] = dataDictionary;
			}
			else
			{
				Debug.Log("No such value exists!  It must be created first");
			}
		}
		else
		{
			Debug.Log("No such value exists!  It must be created first");
		}
	}

	/// <summary>
	/// Clears the datastore.
	/// </summary>
	public void Reset()
	{
		this.storedData.Clear();
	}

	#endregion
}
