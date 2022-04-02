using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
/// <summary>
/// 代码来自 https://www.cnblogs.com/fode/p/10085407.html --Fode
/// </summary>
namespace Emit
{
    /// <summary>
    /// Emit 动态构造 Get方法
    /// </summary>
    public  class PropertyGetterEmit
    {

        private readonly Func<Object, Object> getter;
        public PropertyGetterEmit(PropertyInfo propertyInfo)
        {
            //Objcet value = Obj.GetValue(Object instance);
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }
            this.getter = CreateGetterEmit(propertyInfo);

        }

        public Object Invoke(Object instance)
        {
            return getter?.Invoke(instance);
        }

        private Func<Object, Object> CreateGetterEmit(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            MethodInfo getMethod = property.GetGetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertyGetter", typeof(Object),
                new Type[] { typeof(Object) },
                property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!getMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, getMethod, null);

            if (property.PropertyType.IsValueType)
                il.Emit(OpCodes.Box, property.PropertyType);
            il.Emit(OpCodes.Ret);
            return (Func<Object, Object>)dm.CreateDelegate(typeof(Func<Object, Object>));
        }
    }
}
