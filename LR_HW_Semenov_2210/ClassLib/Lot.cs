using System;
using System.ComponentModel.DataAnnotations;

namespace ClassLib
{
    public class Lot
    {
        public string MessageId { get; set; }
        public string PopReceipt { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Seller { get; set; }
    }
}
