﻿using System;
using System.Reflection;
using UnityEngine;

namespace BSOculusMREnabler
{
	public static class ReflectionUtil
	{
		public static void SetPrivateField(object obj, string fieldName, object value)
		{
			var prop = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			prop.SetValue(obj, value);
		}

		public static T GetPrivateField<T>(object obj, string fieldName)
		{
			var prop = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			var value = prop.GetValue(obj);
			return (T) value;
		}

		public static void SetPrivateProperty(object obj, string propertyName, object value)
		{
			var prop = obj.GetType()
				.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			prop.SetValue(obj, value, null);
		}

		public static void InvokePrivateMethod(object obj, string methodName, object[] methodParams)
		{
			var dynMethod = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
			dynMethod.Invoke(obj, methodParams);
		}

		public static Component CopyComponent(Component original, Type overridingType, GameObject destination)
		{
			var copy = destination.AddComponent(overridingType);
			var fields = original.GetType()
				.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);
			foreach (var field in fields)
			{
				field.SetValue(copy, field.GetValue(original));
			}

			return copy;
		}
	}
}