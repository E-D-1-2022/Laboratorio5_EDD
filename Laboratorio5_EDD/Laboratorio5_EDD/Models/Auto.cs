using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace Laboratorio5_EDD.Models
{
    public class Auto 
    {
        public int Placa { get; set; }
        public Color color { get; set; }

        public string Propietario { get; set; }

        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}
