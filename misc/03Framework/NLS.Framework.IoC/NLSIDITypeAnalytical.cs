using System;
using System.Collections.Generic;
using System.Reflection;

namespace NLS.Framework.IoC
{
    public sealed class NLSIDITypeAnalytical : INLSIDITypeAnalytical
    {
        /// <summary>
        /// 获取类型T实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            Type type = typeof(T);
            return (T)TypeAnalytical(type);
        }

        /// <summary>
        /// 分析传入类型对其进行构造或属性注入
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object TypeAnalytical(Type type)
        {
            //构造
            ConstructorInfo[] constructorInfos = type.GetConstructors();
            object instance = null;
            foreach (ConstructorInfo conInfo in constructorInfos)
            {
                if (conInfo.GetParameters().Length > 0)
                {
                    ParameterInfo[] paras = conInfo.GetParameters();
                    List<object> args = new List<object>();
                    foreach (ParameterInfo para in paras)
                    {
                        if (NLSIocContext.Context.NLSDITypeInfoManage.ContainsKey(para.ParameterType))
                        {
                            object par = TypeAnalytical(NLSIocContext.Context.NLSDITypeInfoManage.GetTypeInfo(para.ParameterType));//递归所有需要注入参数
                            args.Add(par);
                        }
                    }
                    instance = CreateInstance(type, args);
                    break;
                }
            }

            if (instance == null)
            {
                instance = CreateInstance(type);
            }

            //属性
            if (type.GetProperties().Length > 0)
            {
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (propertyInfo.GetCustomAttributes(typeof(NLSDITypeAttribute), false).Length > 0)
                    {
                        if (NLSIocContext.Context.NLSDITypeInfoManage.ContainsKey(propertyInfo.PropertyType))
                        {
                            object propertyvalue = TypeAnalytical(NLSIocContext.Context.NLSDITypeInfoManage.GetTypeInfo(propertyInfo.PropertyType));//递归所有需要注入属性
                            propertyInfo.SetValue(instance, propertyvalue, null);
                        }
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
        }
    }
}