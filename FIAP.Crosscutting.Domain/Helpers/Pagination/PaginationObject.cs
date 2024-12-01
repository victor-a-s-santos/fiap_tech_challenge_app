namespace FIAP.Crosscutting.Domain.Helpers.Pagination
{
    public class PaginationObject
    {
        public int Page { get; set; }
        public int Take { get; set; }
        public string OrderProperty { get; set; }
        public bool OrderDesc { get; set; }
    }
}
