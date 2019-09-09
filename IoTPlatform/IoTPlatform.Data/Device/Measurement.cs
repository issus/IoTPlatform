using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTPlatform.Data.Device
{
    public class Measurement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UnitsId { get; set; }
        public virtual Unit Units { get; set; }

        public int NodeId { get; set; }
        public virtual Node Node { get; set; }

        public DateTime Received { get; set; }
        public DateTime Recorded { get; set; }

        public decimal Value { get; set; }
    }
}
