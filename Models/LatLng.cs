using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBootCloneApp.Models
{
    [ComplexType]
    public class LatLng
    {
        public string Lat { get; set; } = null!;
        public string Lng { get; set; } = null!;

        public override string ToString()
        {
            return $"Lat: {(Lat ?? "null")}, " +
                   $"Lng: {(Lng ?? "null")}\n";
        }
    }
}
