using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Data.DTOs
{
    public class ConfirmPaymentPayload
    {
        public string razorpay_payment_id { get; }
        public string razorpay_order_id { get; }
        public string razorpay_signature { get; }
    }
}
