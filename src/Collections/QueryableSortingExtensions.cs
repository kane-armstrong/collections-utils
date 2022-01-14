using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Armsoft.Collections;

// Credit to Marc Gravell for most of this (https://stackoverflow.com/a/233505)
public static class QueryableSortingExtensions
{
    public static IOrderedQueryable<T> ApplySortSettings<T>(this IQueryable<T> source, SortSettings settings)
    {
        var ordered = settings.SortAscending
            ? source.OrderBy(settings.PropertyName)
            : source.OrderByDescending(settings.PropertyName);

        var sort = settings.ThenBy;
        while (sort != null)
        {
            ordered = sort.SortAscending
                ? ordered.ThenBy(sort.PropertyName)
                : ordered.ThenByDescending(sort.PropertyName);
            sort = sort.ThenBy;
        }

        return ordered;
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
    {
        return ApplyOrder(source, property, "OrderBy");
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
    {
        return ApplyOrder(source, property, "OrderByDescending");
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
    {
        return ApplyOrder(source, property, "ThenBy");
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
    {
        return ApplyOrder(source, property, "ThenByDescending");
    }

    private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
    {
        var props = property.Split('.');
        var type = typeof(T);
        var arg = Expression.Parameter(type, "x");
        Expression expr = arg;
        foreach (var prop in props)
        {
            // use reflection (not ComponentModel) to mirror LINQ
            var pi = type.GetProperty(
                prop,
                BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance
            );
            if (pi == null)
            {
                throw new InvalidOperationException(
                    $"Invalid sort by - the property '{property}' is not a valid property to sort by for type {typeof(T)}.");
            }
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;
        }

        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
        var lambda = Expression.Lambda(delegateType, expr, arg);

        var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), type)
            .Invoke(null, new object[] { source, lambda });
        return (IOrderedQueryable<T>)result;
    }
}
