using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PlayGermany.Server.DataAccessLayer.Models.Base;

namespace PlayGermany.Server.DataAccessLayer.Models
{
    public class Vehicle
        : PositionRotationEntityBase, ILockableEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Model { get; set; }

        public float Mileage { get; set; }

        public float Fuel { get; set; }

        public float FuelMax { get; set; }

        public string NumberPlate { get; set; }

        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        public bool Locked { get; set; }

        public Guid KeyDataId { get; set; }
        public KeyData KeyData { get; set; }
    }
}