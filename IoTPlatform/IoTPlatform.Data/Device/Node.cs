using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTPlatform.Data.Device
{
    public class Node
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }

        [Display(Name = "Access Key")]
        public Guid Key { get; set; }

        public string Description { get; set; }
        public string Location { get; set; }

        public DateTime Added { get; set; }

        [Display(Name = "Last Seen")]
        public DateTime LastSeen { get; set; }

        public virtual IEnumerable<Measurement> Measurements { get; set; }
    }
}
