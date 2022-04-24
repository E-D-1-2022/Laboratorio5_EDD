using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;


namespace Laboratorio5_EDD.Models
{

    public class Vehiculo
    {
        public Vehiculo(int Placa, Color Colorr, string Propietario, decimal Latitud, decimal Longitud)
        {
            this.Placa = Placa;
            this.Color = Colorr;
            this.Propietario = Propietario;
            this.Latitud = Latitud;
            this.Longitud = Longitud;
        }

        public int Placa { get; set; }
        public Color Color { get; set; }
        public string Propietario { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

}
