using System.Data;

namespace WebApi.Models.Commons
{
    public class DataTableFilter
    {
        public bool IsConvert { get; set; }
        public DataTable NewDataTable { get; set; }

        public DataTableFilter()
        {
            IsConvert = false;
            NewDataTable = null;
        }
    }
}
