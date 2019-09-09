using IoTPlatform.Data.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IoTPlatform.Data.Device
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
        public virtual WebProfileUser Owner { get; set; }

        [Required]
        [Display(Name = "Device Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Is Public")]
        public bool IsPublic { get; set; }

        public virtual IEnumerable<Node> Nodes { get; set; }
        public virtual IEnumerable<Unit> Units { get; set; }
    }
}
