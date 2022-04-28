using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio5_EDD.DTO;
using Laboratorio5_EDD.Entidad;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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
            List<int> rango = Enumerable.Range(ri, (rf + 1) * 2).ToList();
            rango.Remove(rango[rango.Count - 1]);
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
            if (ComprobarNumeros(Latitud, -90, 90) != -1)
            {
                AutosDto.Latitud = Convert.ToDecimal(Latitud);
            }
            else {
                ViewBag.Message = "La latitud se sale del rango de -90 y 90";
                return View();
            }

            if (ComprobarNumeros(Longitud, -180, 180) != -1)
            {
                AutosDto.Longitud = Convert.ToDecimal(Longitud);
            }
            else {
                ViewBag.Message = "La logitud se sale del rango de -180 y 180";
                return View();
            }
            BaseDeDatos.ListaAutos.Add(AutosDto);
            ViewData["Mensaje"] = "Auto con placa: " + AutosDto.Placa + " Del Propietario: " + AutosDto.Propietario;
            return View();
        }

        [HttpGet]
        public IActionResult EditarVehiculo(int Placa = 0) {
            AutosDTO Busqueda = null;
            if (Placa != 0)
            {
                Busqueda = BaseDeDatos.ListaAutos.Find(new AutosDTO { Placa = Placa });
            }
            else {
                Busqueda = BaseDeDatos.ListaAutos.Find(new AutosDTO { Placa = Convert.ToInt32(Request.Form["txtBusqueda"].ToString()) });
            }
            ViewData["DataAuto"] = Busqueda;
            if (Busqueda == null) {
                ViewBag.Message = "No existe un auto con la placa enviada";
            }
            return View();
        }
        public IActionResult ActualizarVehiculo() {
            var AutosDto = new AutosDTO();
            int Placa = Convert.ToInt32(Request.Form["txtPlaca"].ToString());
            string Latitud = Request.Form["NudLatitud"].ToString();
            string Longitud = Request.Form["NudLongitud"].ToString();
            if (Placa.ToString().Length == 6)
            {
                AutosDto.Placa = Placa;
            }
            else
            {
                ViewBag.Message = "La Placa no puede tener más de 6 digitos";
                return View();
            }
            AutosDto.color = Color.FromName(Request.Form["txtColor"].ToString());
            AutosDto.Propietario = Request.Form["txtPropietario"].ToString();
            if (ComprobarNumeros(Latitud, -90, 90) != -1)
            {
                AutosDto.Latitud = Convert.ToDecimal(Latitud);
            }
            else
            {
                ViewBag.Message = "La latitud se sale del rango de -90 y 90";
                return View();
            }

            if (ComprobarNumeros(Longitud, -180, 180) != -1)
            {
                AutosDto.Longitud = Convert.ToDecimal(Longitud);
            }
            else
            {
                ViewBag.Message = "La logitud se sale del rango de -180 y 180";
                return View();
            }
            AutosDTO OldValue = BaseDeDatos.ListaAutos.Find(AutosDto);
            BaseDeDatos.ListaAutos.modify(OldValue,AutosDto);
            ViewData["Mensaje"] = "Auto con placa: " + AutosDto.Placa + " Del Propietario: " + AutosDto.Propietario;
            return View();
        }
        public IActionResult ListAutos() {
            List<AutosDTO> Lista = new List<AutosDTO>();
            if (BaseDeDatos.ListaAutos.inOrder()!=null)
                Lista = BaseDeDatos.ListaAutos.inOrder();
                Lista.RemoveAll(x => x.Placa == 0);
            ViewData["ListaAutos"] = Lista;
            return View();
        }
        [HttpGet]
        public IActionResult EliminarAuto(int placa) {
            AutosDTO auto = BaseDeDatos.ListaAutos.Find(new AutosDTO { Placa = placa });
            BaseDeDatos.ListaAutos.Remove(auto);
            return View();
        }
        public IActionResult SubirArchivos() {
            return View();
        }
        [HttpPost]
        public IActionResult CargarData(IFormFile file) {
            if (file != null)
            {
                
                try
                {
                    string FilePath = Path.Combine("C:\\Users\\pcpis\\Documents\\" + file.FileName);
                    //Sube archivo a carpeta temporal
                    //string ruta = Path.Combine(Path.GetTempPath(), file.Name);

                    StreamReader sr = new StreamReader(FilePath);

                    //Recorre archivo
                    string lineaActual="";
                    while ((lineaActual = sr.ReadLine()) != null) {
                        //Separa 
                        string[] info = lineaActual.Split(',');

                        int Placa = Convert.ToInt32(info[0]);
                               //Valida información

                        var AutosDto = new AutosDTO();
                        if (info[0].ToString().Length == 6)
                        {
                            AutosDto.Placa = Placa;
                        }
                        else
                        {
                            ViewBag.Message = "La Placa no puede tener más de 6 digitos";

                        }
                        AutosDto.color = Color.FromName(info[1]);
                        AutosDto.Propietario = info[2];
                        if (ComprobarNumeros(info[3], -90, 90) != -1)
                        {
                            AutosDto.Latitud = Convert.ToDecimal(info[3].ToString());
                        }
                        else
                        {
                            ViewBag.Message = "La latitud se sale del rango de -90 y 90";

                        }

                        if (ComprobarNumeros(info[4], -180, 180) != -1)
                        {
                            AutosDto.Longitud = Convert.ToDecimal(info[4]);
                        }
                        else
                        {
                            ViewBag.Message = "La logitud se sale del rango de -180 y 180";
                            return View();
                        }
                        BaseDeDatos.ListaAutos.Add(AutosDto);
                        ViewData["Mensaje"] = "Auto con placa: " + AutosDto.Placa + " Del Propietario: " + AutosDto.Propietario;


                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            return View();
        }
    }


}
