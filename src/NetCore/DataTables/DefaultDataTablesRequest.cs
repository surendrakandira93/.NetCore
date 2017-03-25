using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.DataTables
{
    public class DefaultDataTablesRequest : IDataTablesRequest
    {
        /// <summary>
        /// Gets/Sets the draw counter from DataTables.
        /// </summary>
        public virtual int Draw { get; set; }

        /// <summary>
        /// Gets/Sets the start record number (jump) for paging.
        /// </summary>
        public virtual int Start { get; set; }

        /// <summary>
        /// Gets/Sets the length of the page (paging).
        /// </summary>
        public virtual int Length { get; set; }

        /// <summary>
        /// Gets/Sets the global search term.
        /// </summary>
        public virtual Search Search { get; set; }

        /// <summary>
        /// Gets/Sets the column collection.
        /// </summary>
        public virtual ColumnCollection Columns { get; set; }

        /// <summary>
        /// Gets or sets the route.
        /// </summary>
        /// <value>
        /// The route.
        /// </value>
        public virtual string Route { get; set; }

        /// <summary>
        /// Gets or sets the taxableperson identifier.
        /// </summary>
        /// <value>
        /// The taxableperson identifier.
        /// </value>
        public virtual string TaxablepersonId { get; set; }

        /// <summary>
        /// Gets or sets the service host.
        /// </summary>
        /// <value>
        /// The service host.
        /// </value>
        public virtual string ServiceHost { get; set; }
    }
}