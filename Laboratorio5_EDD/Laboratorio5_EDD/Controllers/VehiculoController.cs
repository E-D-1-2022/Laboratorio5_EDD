using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Laboratorio5_EDD.DTO;
using Laboratorio5_EDD.Entidad;
using System.IO;
using System.Drawing;
using System;

namespace Laboratorio5_EDD.Controllers
{
    public class Vehiculo : Controller
    {
        [Route("SubirArchivo")]
        public IActionResult SubirArchivos()
        {
            return View();
        }

        [HttpPost("SubirArchivo")]
       public IActionResult SubirArchivos(IFormFile file)
       {
            if(file !=null)
            {
                try
                {
                    //Sube archivo a carpeta temporal
                    string ruta = Path.Combine(Path.GetTempPath(), file.Name);
                    using(var stream = new FileStream(ruta, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    //Lee el archivo
                    string allFileData = System.IO.File.ReadAllText(ruta);

                    //Recorre archivo
                    foreach(string lineaActual in allFileData.Split('\n'))
                    {
                        if(!string.IsNullOrEmpty(lineaActual))
                        {
                            string[] info = lineaActual.Split(',');

                            new AutosDTO
                            {
                                Placa = Convert.ToInt32(info[0]),
                                color = Color.FromName(info[1]),
                                Propietario = info[2],
                                Latitud = Convert.ToInt32(info[3]),
                                Longitud = Convert.ToInt32(info[4]),
                            };
                        }
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View();
       }
        
    }
}
