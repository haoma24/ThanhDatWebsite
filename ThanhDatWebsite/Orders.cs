//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThanhDatWebsite
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
        }
    
        public string OrderID { get; set; }
        public string CustomerID { get; set; }
        public string BranchID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PaymentID { get; set; }
        public string DeliveryMethodID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string Status { get; set; }
    
        public virtual Branches Branches { get; set; }
        public virtual DeliveryMethod DeliveryMethod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Customers Customers { get; set; }
    }
}
