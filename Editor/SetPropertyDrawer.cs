// Copyright (c) 2014 Luminary LLC
// Licensed under The MIT License (See LICENSE for full text)
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomPropertyDrawer(typeof(SetPropertyAttribute))]
public class SetPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		SetPropertyAttribute setProperty = attribute as SetPropertyAttribute;

		// Rely on the default inspector GUI
		EditorGUI.BeginChangeCheck ();
		EditorGUI.PropertyField(position, property, label);

		// Update only when necessary
		if (EditorGUI.EndChangeCheck())
		{
			object parent = GetParentObjectOfProperty(property.propertyPath, property.serializedObject.targetObject);
			Type type = parent.GetType();
			PropertyInfo pi = type.GetProperty(setProperty.Name);
			if (pi == null)
			{
				Debug.LogError("Invalid property name: " + setProperty.Name + "\nCheck your [SetProperty] attribute");
			}
			else
			{
				// When a SerializedProperty is modified the actual field does not have the current value set (i.e.  
				// FieldInfo.GetValue() will return the prior value that was set) until after this OnGUI call has 
				// completed. Therefore, we need to schedule a delayed call to set the property value.

				// Use FieldInfo instead of the SerializedProperty accessors as we'd have to deal with every 
				// SerializedPropertyType and use the correct accessor
				EditorApplication.delayCall += () => pi.SetValue(parent, fieldInfo.GetValue(parent), null);
			}
		} 
	}
	
	private object GetParentObjectOfProperty(string path, object obj)
	{
		string[] fields = path.Split('.');

		// We've finally arrived at the final object that contains the property
		if (fields.Length == 1)
		{
			return obj;
		}

		// We may have to walk public or private fields along the chain to finding our container object, so we have to allow for both
		FieldInfo fi = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);		
		var child = fi.GetValue(obj);

		// If we have a list, then there is no need to keep walking the object chain -- return the parent.
		var childType = child.GetType();
		if (childType.IsArray || childType.IsGenericType && (childType.GetGenericTypeDefinition() == typeof(List<>)))
		{
			return obj;
		}


		// Keep searching for our object that contains the property
		return GetParentObjectOfProperty(string.Join(".", fields, 1, fields.Length - 1), child);
	}
}
