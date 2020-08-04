using System.ComponentModel.DataAnnotations;

namespace Core.Data.Enums {
    public enum PaymentMethodEnum {
        [Display(Name = "Other")]
        OTHER = 0,
        [Display(Name = "Cash")]
        CASH = 1,
        [Display(Name = "Check")]
        CHECK = 2,
        [Display(Name = "Credit Card")]
        CREDIT_CARD = 3,
        [Display(Name = "ACH")]
        ACH = 4,
        [Display(Name = "Mobile Payments")]
        MOBILE_PAYMENTS = 5,
        [Display(Name = "Bank Transfer")]
        BANK_TRANSFER = 6,
        [Display(Name = "Direct Deposit")]
        DIRECT_DEPOSIT = 7,
        [Display(Name = "Prepaid Cards")]
        PREPAID_CARDS = 8
    }
}
