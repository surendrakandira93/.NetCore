namespace NetCore.DataTables
{
    public interface IDataTablesRequest
    {
        /// <summary>
        /// Gets and sets the draw counter from client-side to give back on the server's response.
        /// </summary>
        int Draw { get; set; }

        /// <summary>
        /// Gets and sets the start record number (count) for paging.
        /// </summary>
        int Start { get; set; }

        /// <summary>
        /// Gets and sets the length of the page (max records per page).
        /// </summary>
        int Length { get; set; }

        /// <summary>
        /// Gets and sets the global search pagameters.
        /// </summary>
        Search Search { get; set; }

        /// <summary>
        /// Gets and sets the read-only collection of client-side columns with their options and configs.
        /// </summary>
        ColumnCollection Columns { get; set; }

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>
        /// The route.
        /// </value>
        string Route { get; set; }

        /// <summary>
        /// Gets or sets the taxableperson identifier.
        /// </summary>
        /// <value>
        /// The taxableperson identifier.
        /// </value>
        string TaxablepersonId { get; set; }

        /// <summary>
        /// Gets or sets the service host.
        /// </summary>
        /// <value>
        /// The service host.
        /// </value>
        string ServiceHost { get; set; }
    }
}