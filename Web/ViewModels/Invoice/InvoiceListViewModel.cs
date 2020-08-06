using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Enums;
using Core.Extension;

namespace Web.ViewModels {
    public class InvoiceListViewModel {

        public long Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 1)]
        public string No { get; set; }

        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Due date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Payment date")]
        public DateTime? PaymentDate { get; private set; }

        [Display(Name = "Balance amount")]
        [DataType(DataType.Currency)]
        public string BalanceAmount { get; private set; }

        [DataType(DataType.Currency)]
        public string Amount { get; set; }


        [NotMapped]
        public decimal BalanceAmountDecimal { get; set; }

        [NotMapped]
        public decimal AmountDecimal { get; set; }

        public string Status =>
             BalanceAmountDecimal switch
             {
                 var balance when balance > 0 && balance == AmountDecimal =>
                     PayStatusEnum.Unpaid.GetAttribute<DisplayAttribute>().Name,
                 var balance when balance > 0 =>
                     PayStatusEnum.PartiallyPaid.GetAttribute<DisplayAttribute>().Name,
                 _ => PayStatusEnum.Paid.GetAttribute<DisplayAttribute>().Name
             };

        public bool IsDraft { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
