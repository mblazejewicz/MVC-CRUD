using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Web;
using WebApplicationMVC_CRUD.Helpers;
using WebApplicationMVC_CRUD.Models;

namespace WebApplicationMVC_CRUD.Helpers
{
    public static class LinqExtensions
    {
        public static bool HasProperty(this Type obj, string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName.Trim()))
                return obj.GetProperty(propertyName) != null;
            return false;
        }

        public static T NParse<T>(object value)
        {
            try { return (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value.ToString()); }
            catch { return default(T); }
        }

        public static string Truncate(this string value, int maxLength)
        {
            return value.Truncate(maxLength, "");
        }

        public static string Truncate(this string value, int maxLength, string suffix)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : (value.Substring(0, maxLength - suffix.Length) + suffix);
        }
        /// <summary>
        /// Warunkowy WHERE, zostanie wykonany jeśli warunek jest spełniony
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            else
                return source;
        }

        public static PagedList<TResult> ToPagedList<TSource, TResult>(this IQueryable<TSource> source, ISearchCondition conditions,Func<TSource, TResult> selectPredicate)
        {
            var total = source.Count();
            var ordereData = !String.IsNullOrEmpty(conditions.sort) ? source.OrderBy(conditions.sort + " " + conditions.sortdir) : source;
            var querableItems = ordereData.Skip((conditions.page - 1) * conditions.pageSize)
                         .Take(conditions.pageSize);
            IEnumerable<TResult> items = querableItems.Select(selectPredicate).ToList();
            return new PagedList<TResult>(conditions) { Content = items, TotalRecords= total };
        }
    }


        public static class ObjectExtensions
        {
            public static object GetPropertyValue(this object objTo, string propertyName)
            {
                //returns value of property Name
                return objTo.GetType().GetProperty(propertyName).GetValue(objTo, null);
            }

            public static void CopyObject(this object objTo, object objFrom)
            {
                Type tObjFrom = objFrom.GetType();
                Type tObjTo = objTo.GetType();

                var listPropObj1 = tObjTo.GetProperties();
                foreach (var item in listPropObj1)
                {
                    if (tObjFrom.GetProperty(item.Name) != null)
                    {
                        var value = objFrom.GetPropertyValue(item.Name);
                        tObjTo.GetProperty(item.Name).SetValue(objTo, value);
                    }
                }
            }
        }

        public static class ObjectToDictionaryHelper
        {
            public static IDictionary<string, object> ToDictionary(this object source)
            {
                return source.ToDictionary<object>();
            }

            public static IDictionary<string, T> ToDictionary<T>(this object source)
            {
                if (source == null)
                    ThrowExceptionWhenSourceArgumentIsNull();

                var dictionary = new Dictionary<string, T>();
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                    AddPropertyToDictionary<T>(property, source, dictionary);
                return dictionary;
            }

            private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
            {
                object value = property.GetValue(source);
                if (IsOfType<T>(value))
                    dictionary.Add(property.Name, (T)value);
            }

            private static bool IsOfType<T>(object value)
            {
                return value is T;
            }

            private static void ThrowExceptionWhenSourceArgumentIsNull()
            {
                throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
            }
        }
   }