using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio5_EDD.DTO;
using CustomStructure.Tree_2_3;
namespace Laboratorio5_EDD.Entidad
{
    public static class BaseDeDatos
    {
    
        public static int Comparer(AutosDTO newValue, AutosDTO Oldvalue) {
            if (Oldvalue == null) { Oldvalue = new AutosDTO(); }
            if (newValue == null) { newValue = new AutosDTO(); }
            if (newValue.Placa > Oldvalue.Placa)
            {
                return 1;
            }
            else if (newValue.Placa < Oldvalue.Placa)
            {
                return -1;
            }
            else {
                return 0;
            }
        }
        public static bool Compar(AutosDTO a1, AutosDTO a2)
        {

            return a1.Placa == a2.Placa;
        }
        public static Tree23<AutosDTO> ListaAutos = new Tree23<AutosDTO>(Comparer,Compar);
        
    }
}
