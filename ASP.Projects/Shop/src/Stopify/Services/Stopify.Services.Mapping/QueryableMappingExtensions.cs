﻿namespace Stopify.Services.Mapping
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public static class QueryableMappingExtensions
    {
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(membersToExpand);
        }

        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(parameters);
        }


        public static TDestination To<TDestination>(this object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Mapper.Map<TDestination>(source);
        }

    }
}