using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
/// <summary>
/// 代码来自 https://www.cnblogs.com/fode/p/10085407.html --Fode
/// </summary>
namespace Emit
{

    public class PropertyEmit
    {

        private PropertySetterEmit setter;
        private PropertyGetterEmit getter;
        public String PropertyName { get; private set; }
        public PropertyInfo Info { get; private set; }

        public PropertyEmit(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("属性不能为空");
            }

            if (propertyInfo.CanWrite)
            {
                setter = new PropertySetterEmit(propertyInfo);
            }

            if (propertyInfo.CanRead)
            {
                getter = new PropertyGetterEmit(propertyInfo);
            }

            this.PropertyName = propertyInfo.Name;
            this.Info = propertyInfo;
        }


        /// <summary>
        /// 属性赋值操作（Emit技术）
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetValue(Object instance,Object value)
        {
            this.setter?.Invoke(instance, value);
        }

        /// <summary>
        /// 属性取值操作(Emit技术)
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Object GetValue(Object instance)
        {
            return this.getter?.Invoke(instance);
        }

        private static readonly ConcurrentDictionary<Type, PropertyEmit[]> securityCache = new ConcurrentDictionary<Type, PropertyEmit[]>();

        /// <summary>
        /// 获取对象属性
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static PropertyEmit[] GetProperties(Type type)
        {
            return securityCache.GetOrAdd(type, t => t.GetProperties().Select(p => new PropertyEmit(p)).ToArray());
        }
    }
}
