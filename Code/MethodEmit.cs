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
    public class MethodEmit
    {
        private readonly Func<Object, Object[], Object> invoker;

        public MethodInfo MethodInfo { get; private set; }

        public String Name { get; private set; }
        public MethodEmit(MethodInfo method)
        {
            this.MethodInfo = method ?? throw new ArgumentNullException("method");
            this.Name = method.Name;
            this.invoker = MethodEmit.CreateInvoker(method);
        }

        /// <summary>
        /// Emit动态执行该方法(不允许静态方法)
        /// </summary>
        /// <param name="instance">在其上调用方法或构造函数的对象。 如果方法是静态的，则忽略此参数。 如果构造函数是静态的，则此参数必须是 null 或定义构造函数的类的实例。</param>
        /// <param name="parameters">调用方法或构造函数的参数列表。 此对象数组在数量、顺序和类型方面与要调用的方法或构造函数的参数相同。 如果不存在任何参数，则 parameters 应为
        ///     null。 如果由此实例表示的方法或构造函数采用了 ref 参数（在 Visual Basic 中为 ByRef），那么此参数不需要特殊属性来通过此函数调用此方法或构造函数。
        ///     此数组中未使用值显式初始化的任何对象都将包含该对象类型的默认值。 对于引用类型元素，此值为 null。 对于值类型元素，此值为 0、0.0 或 false，具体取决于特定的元素类型。</param>
        /// <returns>一个包含已调用方法的返回值或包含已调用构造函数的 null 的对象。 还可修改 parameters 数组的元素，其中这些元素使用 ref 或 out 关键字表示声明的参数。</returns>
        public Object Invoke(Object instance,Object[] parameters)
        {
            return this.invoker?.Invoke(instance, parameters);
        }

        private static Func<Object, Object[], Object> CreateInvoker(MethodInfo method)
        {
            ParameterInfo[] pi = method.GetParameters();

            DynamicMethod dm = new DynamicMethod("DynamicMethod",
                typeof(Object),  //return Type
                new Type[] { typeof(Object), typeof(Object[]) },
                typeof(MethodEmit),true);// paramType (Object instance,Object[] params)

            ILGenerator il = dm.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);

            for (int index = 0; index < pi.Length; index++)
            {
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldc_I4, index);

                Type parameterType = pi[index].ParameterType;
                if (parameterType.IsByRef)
                {
                    parameterType = parameterType.GetElementType();
                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Ldelem_Ref);
                        il.Emit(OpCodes.Unbox, parameterType);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldelema, parameterType);
                    }
                }
                else
                {
                    il.Emit(OpCodes.Ldelem_Ref);

                    if (parameterType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, parameterType);
                        il.Emit(OpCodes.Ldobj, parameterType);
                    }
                }
            }

            if ((method.IsAbstract || method.IsVirtual)
                && !method.IsFinal && !method.DeclaringType.IsSealed)
            {
                il.Emit(OpCodes.Callvirt, method);
            }
            else
            {
                il.Emit(OpCodes.Call, method);
            }

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, method.ReturnType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<Object, Object[], Object>)dm.CreateDelegate(typeof(Func<Object, Object[], Object>));
        }
    }
}
