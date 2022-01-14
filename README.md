# Collections Utilities

[![Build](https://github.com/kane-armstrong/collections-utils/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/kane-armstrong/collections-utils/actions/workflows/build.yml)

This repository contains the source code for the Armsoft.Collections package on NuGet.

The library currently supports:

* Pagination
* Sorting

With a focus on extending the `IQueryable` interface. I've used this code often with 
REST API projects to streamline the development of paginated/sorted endpoints that use
either Entity Framework with LINQ-to-SQL, or Dapper/ADO.NET with raw SQL queries.

* Sorting by multiple properties in any direction:
  * For LINQ-to-SQL (e.g. Entity Framework) - `QueryableSortingExtensions.ApplySortSettings<T>(this IQueryable<T> source, SortSettings settings)`
  * For raw SQL queries (e.g. Dapper) - `SortSettingsExtensions.ToSqlOrderBy(this SortSettings sort)`
* Paging:
  * For LINQ-to-SQL (e.g. Entity Framework) - `QueryablePagingExtensions.Paginate<T>(this IOrderedQueryable<T> query, int pageIndex, int pageSize)`
  * For raw SQL queries (e.g. Dapper) - `PageSettingsExtensions.ToSqlPaginate(this PageSettings sort)`
