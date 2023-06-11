using Kendo.Mvc.UI;
using GamingWeb.Custom.DatabaseHelpers;
using GamingWeb.Custom.Models;
using GamingWeb.Custom.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingWeb.Custom.Helpers
{
    public class PaginationHelper<T>
    {
        #region Parameter description
        /*
         1. DataSourceRequest request (required)
         2. string TableName (required) -> Requires the table name, you can add a where clause in the same string but must change the value of dhe optional param string WhereOrAnd to "AND"
         3. string WhereOrAnd (optional) -> This params is required only if you add a where caluse in the TableName param
         */
        #endregion
        public async Task<ListResponse<T>> GetPagginatedList(DataSourceRequest request, string TableName, string WhereOrAnd = "WHERE")
        {
            var query = $"Select * from {TableName} ";
            var countQuery = $"Select COUNT(*) from {TableName} ";

            string filters = "";
            foreach (var filter in request.Filters)
            {
                filters += KendoGridHelper.ApplyFilter<T>(filter);
            }
            string sort = KendoGridHelper.ApplySort<T>(request.Sorts, "Id desc");
            if (filters.Trim().Length > 0)
                filters = WhereOrAnd +" "+ filters;
            if (sort.Trim().Length > 0)
                sort = " ORDER BY " + sort;
            var skip = (request.Page - 1) * request.PageSize;
            string pagination = " OFFSET " + skip + " ROWS FETCH NEXT " + request.PageSize + " ROWS ONLY";
            var total = await new Query().SelectSingle<int>(countQuery + filters);
            var result = await new Query().Select<T>(query + filters + sort + pagination);
            
            //if (result.HasError || total.HasError)
            //{
            //    return null;
            //}
            
            ListResponse<T> response = new ListResponse<T>();
            response.Data = result.Result;
            response.Total = total.Result;
            response.HasError = (result.HasError || total.HasError) ? true : false;
            response.ErrorMessage = result.ErrorMessage;

            return response;
        }
    }
}
