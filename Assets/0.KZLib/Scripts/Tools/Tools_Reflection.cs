using System;
using System.Collections.Generic;
using System.Reflection;

public static partial class Tools
{
	/// <summary>
	/// 이름으로 타입 찾기
	/// </summary>
	public static Type FindType(string _typeFullName)
	{
		foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach(var type in assembly.GetTypes())
			{
				if(type.FullName.IsEmpty() || type.FullName.IsEqual(_typeFullName))
				{
					return type;
				}
			}
		}

		return null;
	}

	public static IEnumerable<Type> FindAllDerivedTypes(Type _type)
	{
		return FindDerivedTypes(_type,AppDomain.CurrentDomain.GetAssemblies());
	}

	public static IEnumerable<Type> FindDerivedTypes(Type _type,IEnumerable<Assembly> _assemblies)
	{
		foreach(var assembly in _assemblies)
		{
			foreach(var data in FindDerivedTypes(_type,assembly))
			{
				yield return data;
			}
		}
	}
	
	public static IEnumerable<Type> FindDerivedTypes(Type _type,Assembly _assembly)
	{
		return FindDerivedTypes(_type,_assembly.GetTypes());
	}
	
	public static IEnumerable<Type> FindDerivedTypes(Type _type,IEnumerable<Type> _types)
	{
		foreach(var type in _types)
		{
			if(_type.IsAssignableFrom(type))
			{
				yield return type;
			}
		}
	}
	
	public static int GetTypeDepth(Type _type)
	{
		var depth = 0;
		var type = _type;

		while(type != null)
		{
			depth++;
			type = type.BaseType;
		}

		return depth;
	}
	
	public static IEnumerable<Type> GetBaseTypes(Type _type)
	{
		var type = _type;

		while(type != null)
		{
			yield return type;

			type = type.BaseType;
		}
	}
	
	public static IEnumerable<Type> GetBaseTypesAndInterfaces(Type _type)
	{
		var type = _type;

		while(type != null)
		{
			yield return type;

			type = type.BaseType;
		}

		foreach(var data in _type.GetInterfaces())
		{
			yield return data;
		}
	}
	
	public static Type GetValueType(MemberInfo _info)
	{
		var field = _info as FieldInfo;

		if(field != null)
		{
			return field.FieldType;
		}

		var property = _info as PropertyInfo;

		if(property != null)
		{
			return property.PropertyType;
		}
		
		throw new NotSupportedException(string.Format("지원하지 않는 타입 입니다.[ 이름 : {0} 타입 : {1}]",_info.Name,_info.GetType()));
	}

	public static object GetValue(MemberInfo _info,object _object)
	{
		var field = _info as FieldInfo;

		if(field != null)
		{
			return field.GetValue(_object);
		}

		var property = _info as PropertyInfo;

		if(property != null)
		{
			return property.GetValue(_object);
		}

		throw new NotSupportedException(string.Format("지원하지 않는 타입 입니다.[ 이름 : {0} 타입 : {1}]",_info.Name,_info.GetType()));
	}
	
	public static void SetValue(MemberInfo _info,object _object,object _value)
	{
		var field = _info as FieldInfo;

		if(field != null)
		{
			field.SetValue(_object,_value);

			return;
		}

		var property = _info as PropertyInfo;

		if(property != null)
		{
			property.SetValue(_object,_value);

			return;
		}

		throw new NotSupportedException(string.Format("지원하지 않는 타입 입니다.[ 이름 : {0} 타입 : {1}]",_info.Name,_info.GetType()));
	}
	
	public static IEnumerable<TAttribute> GetAttributes<TAttribute>(ICustomAttributeProvider _info,bool _inherit = false) where TAttribute : Attribute
	{
		if(_info.IsDefined(typeof(TAttribute),_inherit))
		{
			foreach(var customAttribute in _info.GetCustomAttributes(typeof(TAttribute),_inherit))
			{
				yield return (TAttribute) customAttribute;
			}
		}
	}

	public static bool IsDefined<TAttribute>(ICustomAttributeProvider _info,bool _inherit = false) where TAttribute : Attribute
	{
		return _info.IsDefined(typeof(TAttribute),_inherit);
	}
	
	public static TAttribute GetAttribute<TAttribute>(ICustomAttributeProvider _info,bool _inherit = false) where TAttribute : Attribute
	{
		var attributes = _info.GetCustomAttributes(typeof(TAttribute),_inherit);

		return (attributes != null && attributes.Length > 0) ? (TAttribute) attributes[0] : null;
	}

	public static object GetValueInMember(string _name,object _object)
	{
		if(_name.IsEmpty())
		{
			return null;
		}

		var type = _object.GetType();

		var propertyInfo = type.GetProperty(_name,BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		if(propertyInfo != null)
		{
			return propertyInfo.GetValue(_object);
		}

		var fieldInfo = type.GetField(_name,BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		if(fieldInfo != null)
		{
			return fieldInfo.GetValue(_object);
		}

		return null;
	}

	public static void ExecuteMethod(object _object,string _name)
	{
		var info = _object.GetType().GetMethod(_name);

		info?.Invoke(_object,null);
	}
}