using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NetCore.DataTables
{
    public class DataTablesBinder
    {
        /// <summary>
        /// Formatting to retrieve data for each column.
        /// </summary>
        protected readonly string COLUMN_DATA_FORMATTING = "columns[{0}][data]";

        /// <summary>
        /// Formatting to retrieve name for each column.
        /// </summary>
        protected readonly string COLUMN_NAME_FORMATTING = "columns[{0}][name]";

        /// <summary>
        /// Formatting to retrieve searchable indicator for each column.
        /// </summary>
        protected readonly string COLUMN_SEARCHABLE_FORMATTING = "columns[{0}][searchable]";

        /// <summary>
        /// Formatting to retrieve orderable indicator for each column.
        /// </summary>
        protected readonly string COLUMN_ORDERABLE_FORMATTING = "columns[{0}][orderable]";

        /// <summary>
        /// Formatting to retrieve search value for each column.
        /// </summary>
        protected readonly string COLUMN_SEARCH_VALUE_FORMATTING = "columns[{0}][search][value]";

        /// <summary>
        /// Formatting to retrieve search regex indicator for each column.
        /// </summary>
        protected readonly string COLUMN_SEARCH_REGEX_FORMATTING = "columns[{0}][search][regex]";

        /// <summary>
        /// Formatting to retrieve ordered columns.
        /// </summary>
        protected readonly string ORDER_COLUMN_FORMATTING = "order[{0}][column]";

        /// <summary>
        /// Formatting to retrieve columns order direction.
        /// </summary>
        protected readonly string ORDER_DIRECTION_FORMATTING = "order[{0}][dir]";

        /// <summary>
        /// Binds a new model with the DataTables request parameters.
        /// You should override this method to provide a custom type for internal binding to procees.
        /// </summary>
        /// <param name="controllerContext">The context for the controller.</param>
        /// <param name="bindingContext">The context for the binding.</param>
        /// <returns>Your model with all it's properties set.</returns>
        public virtual object BindModel(HttpContext controllerContext)
        {
            return Bind(controllerContext, typeof(DefaultDataTablesRequest));
        }

        /// <summary>
        /// Binds a new model with both DataTables and your custom parameters.
        /// You should not override this method unless you're using request methods other than GET/POST.
        /// If that's the case, you'll have to override ResolveNameValueCollection too.
        /// </summary>
        /// <param name="controllerContext">The context for the controller.</param>
        /// <param name="bindingContext">The context for the binding.</param>
        /// <param name="modelType">The type of the model which will be created. Should implement IDataTablesRequest.</param>
        /// <returns>Your model with all it's properties set.</returns>
        protected virtual object Bind(HttpContext controllerContext, Type modelType)
        {
            var request = controllerContext.Request;
            var model = (IDataTablesRequest)Activator.CreateInstance(modelType);
            QueryString queryParameters;
            FormCollection requestParameters;
            if (request.Method.ToLower().Equals("get"))
            {
                queryParameters = request.QueryString;
                var queryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(queryParameters.Value);
                requestParameters = new FormCollection(queryString);
            }
            else
            {
                requestParameters = (FormCollection)request.Form;
            }

            // Populates the model with the draw count from DataTables.
            model.Draw = int.Parse(requestParameters["draw"][0]);
            try
            {
                model.Route = requestParameters["Route"][0];
                model.TaxablepersonId = requestParameters["TaxablepersonId"][0];
                model.ServiceHost = requestParameters["ServiceHost"][0];
            }
            catch
            {
            }

            // Populates the model with page info (server-side paging).
            model.Start = int.Parse(requestParameters["start"][0]);
            model.Length = int.Parse(requestParameters["length"][0]);

            // Populates the model with search (global search).
            var searchValue = requestParameters["search[value]"];
            var searchRegex = bool.Parse(requestParameters["search[regex]"]);
            model.Search = new Search(searchValue, searchRegex);

            // Get's the column collection from the request parameters.
            var columns = GetColumns(requestParameters);

            // Parse column ordering.
            ParseColumnOrdering(requestParameters, columns);

            // Attach columns into the model.
            model.Columns = new ColumnCollection(columns);

            // Map aditional properties into your custom request.
            MapAditionalProperties(model, requestParameters);

            // Returns the filled model.
            return model;
        }

        /// <summary>
        /// Map aditional properties (aditional fields sent with DataTables) into your custom implementation of IDataTablesRequest.
        /// You should override this method to map aditional info (non-standard DataTables parameters) into your custom
        /// implementation of IDataTablesRequest.
        /// </summary>
        /// <param name="requestModel">The request model which will receive your custom data.</param>
        /// <param name="requestParameters">Parameters sent with the request.</param>
        protected virtual void MapAditionalProperties(IDataTablesRequest requestModel, FormCollection requestParameters) { }

        /// <summary>
        /// Resolves the NameValueCollection from the request.
        /// Default implementation supports only GET and POST methods.
        /// You may override this method to support other HTTP verbs.
        /// </summary>
        /// <param name="request">The HttpRequestBase object that represents the MVC request.</param>
        /// <returns>The NameValueCollection with request variables.</returns>
        protected virtual dynamic ResolveNameValueCollection(HttpRequest request)
        {
            if (request.Method.ToLower().Equals("get")) return request.QueryString;
            else if (request.Method.ToLower().Equals("post")) return request.Form;
            else throw new ArgumentException(String.Format("The provided HTTP method ({0}) is not a valid method to use with DataTablesBinder. Please, use HTTP GET or POST methods only.", request.Method), "method");
        }

        protected virtual List<Column> GetColumns(FormCollection collection)
        {
            try
            {
                var columns = new List<Column>();

                // Loop through every request parameter to avoid missing any DataTable column.
                for (int i = 0; i < collection.Count; i++)
                {
                    string columnData;
                    string columnName;
                    try
                    {
                        columnData = collection[String.Format(COLUMN_DATA_FORMATTING, i)][0];
                        columnName = collection[String.Format(COLUMN_NAME_FORMATTING, i)][0];
                    }
                    catch
                    {
                        columnData = null;
                        columnName = null;
                    }

                    if (columnData != null && columnName != null)
                    {
                        var columnSearchable = bool.Parse(collection[String.Format(COLUMN_SEARCHABLE_FORMATTING, i)][0]);
                        var columnOrderable = bool.Parse(collection[String.Format(COLUMN_ORDERABLE_FORMATTING, i)][0]);
                        var columnSearchValue = collection[String.Format(COLUMN_SEARCH_VALUE_FORMATTING, i)][0];
                        var columnSearchRegex = bool.Parse(collection[String.Format(COLUMN_SEARCH_REGEX_FORMATTING, i)][0]);

                        columns.Add(new Column(columnData, columnName, columnSearchable, columnOrderable, columnSearchValue, columnSearchRegex));
                    }
                    else break; // Stops iterating because there's no more columns.
                }

                return columns;
            }
            catch
            {
                // Returns an empty column collection to avoid null exceptions.
                return new List<Column>();
            }
        }

        /// <summary>
        /// Configure column's ordering.
        /// This method is provided as an option for you to override the default behavior.
        /// </summary>
        /// <param name="collection">The request value collection.</param>
        /// <param name="columns">The column collection as returned from GetColumns method.</param>
        protected virtual void ParseColumnOrdering(FormCollection collection, IEnumerable<Column> columns)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                int orderColumn;
                string orderDirection;
                try
                {
                    orderColumn = int.Parse(collection[String.Format(ORDER_COLUMN_FORMATTING, i)][0]);
                    orderDirection = collection[String.Format(ORDER_DIRECTION_FORMATTING, i)][0];
                }
                catch
                {
                    orderColumn = -1;
                    orderDirection = null;
                }

                if (orderColumn > -1 && orderDirection != null)
                    columns.ElementAt(orderColumn).SetColumnOrdering(i, orderDirection);
            }
        }
    }
}