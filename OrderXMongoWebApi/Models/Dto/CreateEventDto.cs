using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Models.Dto
{
    public class CreateEventDto
    {
        public string eventname { get; set; }
        public string username { get; set; }
        public string userid { get; set; }
    
        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public string eventaddress { get; set; }
   

      
    }

    public class EventDto
    {
        public string? _id { get; set; }
        public string eventname { get; set; }
        public string username { get; set; }
        public string userid { get; set; }

        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public string eventaddress { get; set; }

    }
    public class StockDto
    {
        public string? _id { get; set; }
        public string eventname { get; set; }
      
        public Stock[] Stock { get; set; }
    }
    public class WaiterDto
    {
        public string? _id { get; set; }
        public string eventname { get; set; }

        public Waiter[] Waiter { get; set; }
    }
    public class TableDto
    {
        public string? _id { get; set; }
        public string eventname { get; set; }

        public Table[] Table { get; set; }
    }
    public class LinkedEventDto
    {
        public string? _id { get; set; }
        public string eventname { get; set; }

        public LinkedEvent[] LinkedEvent { get; set; }
    }
    public class AssingWaiterDto
    {
        public string UserName { get; set; }
        public string[] Tables { get; set; }
        public string EventId { get; set; }
    }
    public class ScanTableDto
    {
        public string Eventid { get; set; }
        public string eventname { get; set; }
        public string? Tableid { get; set; }
        public string Tablename { get; set; }
        public string WaiterUserName { get; set; }
    }
    public class CreateOrderDto
    {
        public string EventId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string[] Addons { get; set; }
        public OrderItem[] OrderItems { get; set; }
        public string WaiterUserName { get; set; }
        public string TableId { get; set; }
        public string TableName { get; set; }
    }
    public class OrderDto
    {
        public string? _id { get; set; }
        public string UserName { get; set; }
        public string[] Addons { get; set; }
        public OrderItem[] OrderItems { get; set; }
    
        public string WaiterUserName { get; set; }
        public string TableName { get; set; }
    }
}
