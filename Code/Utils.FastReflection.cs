using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
namespace Cultivation_Way.Utils
{
    /// <summary>
    /// Thanks for idea and some codes from 
    /// https://mattwarren.org/2016/12/14/Why-is-Reflection-slow/
    /// </summary>
    static class FastReflection
    {
        
        private static Dictionary<Type,Dictionary<string, Delegate>> methods = new Dictionary<Type, Dictionary<string, Delegate>>();
        private static Dictionary<Type,Dictionary<string, Delegate > > setters = new Dictionary<Type, Dictionary<string, Delegate>>();
        private static Dictionary<Type,Dictionary<string, Func<object, object> > > getters = new Dictionary<Type, Dictionary<string, Func<object, object>>>();

        public static Action<ExtendedActor, float, bool, AttackType, BaseSimObject, bool> actor_getHit =
            (Action<ExtendedActor, float, bool, AttackType, BaseSimObject, bool>)GetFastMethod(typeof(ExtendedActor),"getHit");
        public static Func<ExtendedActor, UnitProfession, bool> actor_isProfession =
            (Func<ExtendedActor, UnitProfession, bool>)GetFastMethod(typeof(ExtendedActor), "isProfession");
        public static Action<ExtendedBuilding, float, bool, AttackType, BaseSimObject, bool> building_getHit =
            (Action<ExtendedBuilding, float, bool, AttackType, BaseSimObject, bool>)GetFastMethod(typeof(ExtendedBuilding), "getHit");
        public static Action<ExtendedBuilding> building_create = 
            (Action<ExtendedBuilding>)GetFastMethod(typeof(ExtendedBuilding), "create");
        public static Action<ExtendedBuilding> building_finishScaleTween = 
            (Action<ExtendedBuilding>)GetFastMethod(typeof(BuildingTweenExtension), "finishScaleTween",true);
        public static Action<ExtendedBuilding, WorldTile, BuildingAsset, BuildingData> building_setBuilding =
            (Action<ExtendedBuilding, WorldTile, BuildingAsset, BuildingData>)GetFastMethod(typeof(ExtendedBuilding), "setBuilding");
        public static Func<MapBox, WorldTile, BuildingAsset, City, BuildPlacingType, bool> mapbox_canBuildFrom =
            (Func<MapBox, WorldTile, BuildingAsset, City, BuildPlacingType, bool>)GetFastMethod(typeof(MapBox), "canBuildFrom");
        public static Action<MapBox, WorldTile, int, MapObjectType> mapbox_getObjectsInChunks =
            (Action<MapBox, WorldTile, int, MapObjectType>)GetFastMethod(typeof(MapBox), "getObjectsInChunks");

        public static T GetValue<T>(this object instance,string fieldName,Type instanceType = null)
        {
            if (instanceType == null)
            {
                instanceType = instance.GetType();
            }
            Func<object, object> getter;
            Dictionary<string, Func<object, object>> instanceGetters;
            if(!getters.TryGetValue(instanceType, out instanceGetters))
            {
                instanceGetters = new Dictionary<string, Func<object, object>>();
                getters.Add(instanceType, instanceGetters);
            }
            if(!instanceGetters.TryGetValue(fieldName,out getter))
            {
                getter = createNewGetter(instanceType, fieldName);
                instanceGetters.Add(fieldName, getter);
            }
            
            return (T)getter(instance);
        }
        public static void SetValue<TI,TF>(this TI instance, string fieldName,TF pValue, Type instanceType = null)
        {
            if (instanceType == null)
            {
                instanceType = typeof(TI);
            }
            Delegate setter;
            Dictionary<string, Delegate> instanceSetters;
            if (!setters.TryGetValue(instanceType, out instanceSetters))
            {
                instanceSetters = new Dictionary<string, Delegate>();
                setters.Add(instanceType, instanceSetters);
            }
            if (!instanceSetters.TryGetValue(fieldName, out setter))
            {
                setter = createNewSetter<TI,TF>(instanceType, fieldName);
                instanceSetters.Add(fieldName, setter);
            }
            ((Action<TI,TF>)setter)(instance,pValue);
        }
        public static Delegate GetFastMethod(this object instance,string methodName, Type instanceType = null)
        {
            if (instanceType == null)
            {
                instanceType = instance.GetType();
            }
            Delegate method;
            Dictionary<string, Delegate> instanceMethods;
            if (!methods.TryGetValue(instanceType, out instanceMethods))
            {
                instanceMethods = new Dictionary<string, Delegate>();
                methods.Add(instanceType, instanceMethods);
            }
            if (!instanceMethods.TryGetValue(methodName, out method))
            {
                MethodInfo methodInfo = instanceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                method = createMethodDelegate(methodInfo);
                instanceMethods.Add(methodName, method);
            }
            return method;
        }
        public static Delegate GetFastMethod(Type instanceType, string methodName,bool isStatic = false)
        {
            if (isStatic)
            {
                return createMethodDelegate(instanceType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic));
            }
            return createMethodDelegate(instanceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic));
        }
        private static Func<object, object> createNewGetter(Type instanceType,string fieldName)
        {
            FieldInfo field = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            #region 错误
            //FileNotFoundException: Could not load file or assembly 'Sigil, Version=5.0.0.0, Culture=neutral, PublicKeyToken=2d06c3494341c8ab' or one of its dependencies.
            //Emit<Func<object, object>> getterEmit = Emit<Func<object, object>>
            //                    .NewDynamicMethod(name:$"Get{instanceType.Name}{fieldName}")
            //                    .LoadArgument(0)
            //                    .CastClass(instanceType)
            //                    .LoadField(field)
            //                    .Return();
            //return getterEmit.CreateDelegate();

            //PlatformNotSupportedException: Operation is not supported on this platform.
            //
            //try
            //{
            //    DynamicMethod getterMethod = new DynamicMethod($"Get{instanceType.Name}{fieldName}", typeof(Object), new System.Type[] { instanceType });
            //    ILGenerator ilGenerator = getterMethod.GetILGenerator();
            //    ilGenerator.Emit(OpCodes.Ldarg_0);
            //    ilGenerator.Emit(OpCodes.Castclass, instanceType);
            //    ilGenerator.Emit(OpCodes.Ldfld, field);
            //    ilGenerator.Emit(OpCodes.Ret);
            //    return (Func<object, object>)getterMethod.CreateDelegate(typeof(Func<object, object>));
            //}
            //catch (NotSupportedException e)
            //{
            #endregion
            try
            {
                    ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                    UnaryExpression instanceCast =
                        !field.DeclaringType.IsValueType ?
                            Expression.TypeAs(instance, field.DeclaringType) :
                            Expression.Convert(instance, field.DeclaringType);
                    Func<object, object> GetDelegate =
                        Expression.Lambda<Func<object, object>>(
                            Expression.TypeAs(
                                Expression.Field(instanceCast, field),
                                typeof(object)),
                            instance)
                        .Compile();
                    return GetDelegate;
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogError("Expression Tree-Getter");
                    return null;
                }
            //}
        }
        private static Delegate createNewSetter<TI,TF>(Type instanceType,string fieldName)
        {
            FieldInfo field = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ParameterExpression instance = Expression.Parameter(typeof(TI), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TF), fieldName);
            return Expression.Lambda<Action<TI,TF>>(Expression.Assign(Expression.Field(instance, field),parameter),instance,parameter).Compile();
        }
        private static Delegate createMethodDelegate(MethodInfo methodInfo)
        {
            //try
            //{
                List<ParameterExpression> paramExpressions = methodInfo.GetParameters().Select((p, i) =>
                {
                    return Expression.Parameter(p.ParameterType, p.Name);
                }).ToList();

                MethodCallExpression callExpression;
                if (methodInfo.IsStatic)
                {
                    callExpression = Expression.Call(methodInfo, paramExpressions);
                }
                else
                {
                    ParameterExpression instanceExpression = Expression.Parameter(methodInfo.ReflectedType, "instance");
                    callExpression = Expression.Call(instanceExpression, methodInfo, paramExpressions);
                    paramExpressions.Insert(0, instanceExpression);
                }
                LambdaExpression lambdaExpression = Expression.Lambda(callExpression, paramExpressions);
                return lambdaExpression.Compile();
            //}
            //catch (Exception)
            //{
            //    UnityEngine.Debug.LogError("Expression Tree-Method");
            //    return null;
            //}
        }

        
    }
}
