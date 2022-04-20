using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio5_EDD.DTO;
namespace Laboratorio5_EDD.Entidad
{
    public class BaseDeDatoscs
    {
        private static BaseDeDatoscs _Instance = null;
        private BaseDeDatoscs() { }

        public static BaseDeDatoscs Instance
        {
            get {
                if (_Instance == null) {
                    _Instance = new BaseDeDatoscs();
                }
                return _Instance;
            }
        }

        public List<AutosDTO> ListaAutos = new List<AutosDTO>();
    }
}
