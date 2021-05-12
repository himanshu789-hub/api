﻿using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Product
    {
        public Product()
        {
            IncomingShipment = new HashSet<IncomingShipment>();
            Invoice = new HashSet<Invoice>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetail>();
            ProductFlavourQuantity = new HashSet<ProductFlavourQuantity>();
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public decimal PricePerCaret { get; set; }
        

        public byte CaretSize { get; set; }
        public virtual ICollection<IncomingShipment> IncomingShipment { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<OutgoingShipmentDetail> OutgoingShipmentDetails { get; set; }
        public virtual ICollection<ProductFlavourQuantity> ProductFlavourQuantity { get; set; }
    }
}
