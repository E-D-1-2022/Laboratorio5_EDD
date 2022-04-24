
using System.Drawing;
public class Objects
{
    public int placa { get; set; }

    public Color color { get; set; }

    public string propietario { get; set; }

    public decimal latitud { get; }

    public decimal longitud {get; }


    public Objects(int _placa, Color _color, string _propietario, decimal _latitud, decimal _longitud)
    {
        placa = _placa;
        color = _color;
        propietario = _propietario;
        latitud = _latitud;
        longitud = _longitud;
    }
}
