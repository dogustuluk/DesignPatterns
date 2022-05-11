using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.StrategyDesignPattern.Models
{
    public class Product
    {//hem Sql Server için hem de mongoDb için ortak olarak oluşturuyoruz.
        
        [BsonId] //bu kod ile mongoDb için bir id olduğunu belirtiyoruz.
        [Key] //EF Core ile çalışıyorsak id olduğunu belirtmemize gerek yoktur, çünkü alt satırda property ismini "Id" şeklinde vermiş bulunduk
        [BsonRepresentation(BsonType.ObjectId)] //id'yi string'e çeviriyor.
        public string Id { get; set; }
        public string Name { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedDate { get; set; }


        //hem entity için hem de mongoDb'de temsil edilecek olan class hazır.
    }
}
