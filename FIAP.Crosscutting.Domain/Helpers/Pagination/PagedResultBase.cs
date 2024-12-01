using System.Text.Json.Serialization;

namespace FIAP.Crosscutting.Domain.Helpers.Pagination
{
    public abstract class PagedResultBase
    {
        /// <summary>
        /// Página atual
        /// </summary>
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total de registros retornados por página
        /// </summary>
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }

        /// <summary>
        /// Total de registros solicitados por página
        /// </summary>
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total de registros cadastrados
        /// </summary>
        [JsonPropertyName("total_records")]
        public Int64 TotalRecords { get; set; }

        /// <summary>
        /// Índice do primeiro registro da página
        /// </summary>
        [JsonPropertyName("first_record_on_page")]
        public int FirstRecordOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        /// <summary>
        /// Índice do último registro da página
        /// </summary>
        [JsonPropertyName("last_record_on_page")]
        public long LastRecordOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, TotalRecords); }
        }
    }
}
