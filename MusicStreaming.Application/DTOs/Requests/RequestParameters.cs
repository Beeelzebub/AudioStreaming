namespace MusicStreaming.Application.DTOs.Requests
{
    public class RequestParameters
    {
        private const int _maxPageSize = 50;
        
        private int pageSize;

        public int Page { get; set; } = 1;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > _maxPageSize ? _maxPageSize : value;
        }

        public string SearchString { get; set; } = string.Empty;
    }
}
