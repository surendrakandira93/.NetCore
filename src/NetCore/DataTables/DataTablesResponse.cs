using System.Collections;

namespace NetCore.DataTables
{
    public class DataTablesResponse
    {
        /// <summary>
        /// Gets the draw counter for DataTables.
        /// </summary>
        public int draw { get; private set; }

        /// <summary>
        /// Gets the data collection.
        /// </summary>
        public IEnumerable data { get; private set; }

        /// <summary>
        /// Gets the total number of records (without filtering - total dataset).
        /// </summary>
        public long recordsTotal { get; private set; }

        /// <summary>
        /// Gets the resulting number of records after filtering.
        /// </summary>
        public long recordsFiltered { get; private set; }

        /// <summary>
        /// Creates a new DataTables response object with it's elements.
        /// </summary>
        /// <param name="draw">The draw counter as received from the DataTablesRequest.</param>
        /// <param name="data">The data collection (data page).</param>
        /// <param name="recordsFiltered">The resulting number of records after filtering.</param>
        /// <param name="recordsTotal">The total number of records (total dataset).</param>
        public DataTablesResponse(int draw, IEnumerable data, long recordsFiltered, long recordsTotal)
        {
            this.draw = draw;
            this.data = data;
            this.recordsFiltered = recordsFiltered;
            this.recordsTotal = recordsTotal;
        }
    }
}