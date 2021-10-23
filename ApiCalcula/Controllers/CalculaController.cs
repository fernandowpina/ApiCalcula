using ApiCalcula.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ApiCalcula.Controllers
{
    [RoutePrefix("api/ws")]
    public class CalculaController : ApiController
    {
        Taxa tObj;

        [HttpGet]
        [Route("calculajuros")]
        public async Task<object> CalcularJurosAsync(decimal valorInicial, int meses)
        {

            await PegarTaxa();

            var valorFinal = decimal.Multiply(valorInicial, new decimal(Math.Pow(1 + tObj.valorTaxa, meses)));

            return new { ValorFinal = valorFinal.ToString("F") , ValorCedula = valorFinal.ToString("C") };

        }

        [HttpGet]
        [Route("showmethecode")]
        public object MostrarCodigo()
        {
            return new { urlApi1 = "https://github.com/fernandowpina/ApiTaxa.git", urlApi2 = "https://github.com/fernandowpina/ApiCalcula.git" };
        }


        private async Task PegarTaxa()
        {
            using (HttpClient cliente = new HttpClient())
            {
                try
                {
                    var resposta = await cliente.GetAsync("http://localhost/ApiTaxa/api/ws/taxaJuros");

                    string dados = await resposta.Content.ReadAsStringAsync();

                    tObj = new JavaScriptSerializer().Deserialize<Taxa>(dados);
                }
                catch(Exception ex)
                {
                    string ms = ex.Message;
                }


            }
        }

    }
}