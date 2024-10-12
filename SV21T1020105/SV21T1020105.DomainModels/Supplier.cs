﻿namespace SV21T1020105.DomainModels
{
    public class Supplier
    {
        public int SupplierID {  get; set; }
        public string SupplierName { get; set; } = String.Empty;
        public string ContactName { get; set;} = String.Empty;
        public string Province { get; set; } = String.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}