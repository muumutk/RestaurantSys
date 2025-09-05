using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSys.Models
{
    public partial class StockBatchWarningLog
    {

        public int StockBatchWarningLogID { get; set; }
        
        public DateTime WarningSentDate { get; set; }

        [ForeignKey("StockBatch")]
        public int? BatchID { get; set; }

        [ForeignKey("Employee")]
        public string? EmployeeID { get; set; } = null!;

        public virtual StockBatch? StockBatch { get; set; }

        public virtual Employee? Employee { get; set; }

    }
}
