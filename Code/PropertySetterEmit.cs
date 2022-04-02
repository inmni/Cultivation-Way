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
    /// Emit动态构造Set方法
    /// </summary>
    public class PropertySetterEmit
    {
        private readonly Action<Object, Object> setFunc;
        public PropertySetterEmit(PropertyInfo propertyInfo)
        {
            //Obj.Set(Object instance,Object value)
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }
            this.setFunc = CreatePropertySetter(propertyInfo);

        }

        private Action<Object, Object> CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(Object), typeof(Object) }, property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
                il.EmitCall(OpCodes.Call, setMethod, null);

            il.Emit(OpCodes.Ret);
            return (Action<Object, Object>)dm.CreateDelegate(typeof(Action<Object, Object>));
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
                il.Emit(OpCodes.Unbox_Any, type);
            else
                il.Emit(OpCodes.Castclass, type);
        }
 




        public void Invoke(Object instance,Object value)
        {
            this.setFunc?.Invoke(instance, value);
        }
    }
}
