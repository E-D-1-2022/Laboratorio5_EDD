using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio5_EDD.DTO;
using System.Drawing;
namespace Laboratorio5_EDD.Controllers
{
    public class AutosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private decimal ComprobarNumeros(string n, int ri, int rf)
        {
            int cont = 0;
            string Val = "";
            for (cont = 0; cont < n.Length; cont++) {
                if (n[cont] != '.')
                {
                    Val += n[cont].ToString();
                }
                else {
                    break;
                }
            }
            int num = int.Parse(Val);
            List<int> rango = Enumerable.Range(ri, (rf+1)*2).ToList();
            rango.Remove(rango[rango.Count-1]);
            if (rango.IndexOf(num) != -1)
            {
                return Convert.ToDecimal(n);
            }
            else {
                return -1;
            }
        }

        public IActionResult GuardarCarro() {
            var AutosDto = new AutosDTO();
            int Placa = Convert.ToInt32(Request.Form["txtPlaca"].ToString());
            string Latitud = Request.Form["NudLatitud"].ToString();
            string Longitud = Request.Form["NudLongitud"].ToString();
            if (Placa.ToString().Length == 6)
            {
                AutosDto.Placa = Placa;
            }
            else {
               ViewBag.Message = "La Placa no puede tener más de 6 digitos";
                return View();
            }
            AutosDto.color = Color.FromName(Request.Form["txtColor"].ToString());
            AutosDto.Propietario = Request.Form["txtPropietario"].ToString();
            if (ComprobarNumeros(Request.Form["NudLatitud"].ToString(), -90, 90) != -1)
            {
                AutosDto.Latitud = Convert.ToDecimal(Request.Form["NudLatitud"].ToString());
            }
            else {
                ViewBag.Message = "La latitud se sale del rango de -90 y 90";
                return View();
            }

            if (ComprobarNumeros(Request.Form["NudLongitud"].ToString(), -180, 180) != -1)
            {
                AutosDto.Longitud = Convert.ToDecimal(Request.Form["NudLongitud"].ToString());
            }
            else {
                ViewBag.Message = "La logitud se sale del rango de -180 y 180";
                return View();
            }

            ViewData["Mensaje"] = "Auto con placa: " + AutosDto.Placa + " Del Propietario: " + AutosDto.Propietario;
            return View();
        }
    }
}
