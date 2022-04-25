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

namespace Laboratorio5_EDD.Controllers
{
    public class AutosController : Controller
    {
        [BindProperty]
        public UploadFile Subir_Archivos { get; set; }

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
            if (ComprobarNumeros(Latitud, -90, 90) != -1)
            {
                AutosDto.Latitud = Convert.ToDecimal(Request.Form["NudLatitud"].ToString());
            }
            else {
                ViewBag.Message = "La latitud se sale del rango de -90 y 90";
                return View();
            }

            if (ComprobarNumeros(Longitud, -180, 180) != -1)
            {
                AutosDto.Longitud = Convert.ToDecimal(Request.Form["NudLongitud"].ToString());
            }
            else {
                ViewBag.Message = "La logitud se sale del rango de -180 y 180";
                return View();
            }
            BaseDeDatos.ListaAutos.Add(AutosDto);
            ViewData["Mensaje"] = "Auto con placa: " + AutosDto.Placa + " Del Propietario: " + AutosDto.Propietario;
            return View();
        }
        public IActionResult ListAutos() {
            List<AutosDTO> Lista = BaseDeDatos.ListaAutos.inOrder();
            Lista.RemoveAll(x=> x.Placa==0);
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
                    //Sube archivo a carpeta temporal
                    string ruta = Path.Combine(Path.GetTempPath(), file.Name);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    //Lee el archivo
                    string allFileData = System.IO.File.ReadAllText(ruta);

                    //Recorre archivo
                    foreach (string lineaActual in allFileData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(lineaActual))
                        {
                            string[] info = lineaActual.Split(',');

                            BaseDeDatos.ListaAutos.Add(new AutosDTO
                            {
                                Placa = Convert.ToInt32(info[0]),
                                color = Color.FromName(info[1]),
                                Propietario = info[2],
                                Latitud = Convert.ToInt32(info[3]),
                                Longitud = Convert.ToInt32(info[4]),
                            });

                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View();
        }
    }
    public class UploadFile { 
        public IFormFile file { get; set; }
    }
}
