using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CostosObras.Models
{
    public partial class ProyectosActivos
    {
        public ProyectosActivos()
        {
            this.ingresos = new HashSet<ctacte>();
            this.ingresos = new HashSet<ctacte>();
        }
        public virtual proyecto proyecto{ get; set; }
        public virtual ICollection<ctacte> ingresos { get; set; }
        public virtual ICollection<ctacte> egresos { get; set; }
        
    }
}