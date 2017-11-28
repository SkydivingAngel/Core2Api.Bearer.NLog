namespace Angel.Webservice.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Angel.Webservice.Enumns;
    using Angel.Webservice.Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class MainController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<MainController> logger;
        private readonly IHttpContextAccessor accessor;

        public MainController(IConfiguration configuration, ILogger<MainController> logger, IHttpContextAccessor accessor)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.accessor = accessor;
        }

        [Authorize]
        [HttpPost("RichiestaWs")]
        public async Task<IActionResult> RichiestaWs()
        {
            try
            {
                string richiestaJson = await Request.GetRawBodyStringAsync();
                if (richiestaJson == null)
                {
                    logger.LogError("RichiestaWs: Contenuto Body Nullo - Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                    return ActionResultGenerator.GeneraRisposta("Contenuto Nullo", TipoDiRisposta.Nok);
                }

                //Passa il json al metodo dell'assembly referenziato (vb) e riceve la risposta
                return ActionResultGenerator.GeneraRisposta(GestisciRichiesta(richiestaJson), TipoDiRisposta.Ok);
            }
            catch (Exception ex)
            {
                logger.LogError("RichiestaWs: " + ex + " - Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                return ActionResultGenerator.GeneraRisposta(ex.ToString(), TipoDiRisposta.Nok);
            }
        }

        //Test
        private object GestisciRichiesta(string richiestaJson)
        {
            Richiesta richiestaRisposta = new Richiesta();

            try
            {
                Richiesta richiesta = JsonConvert.DeserializeObject<Richiesta>(richiestaJson);

                switch (richiesta.Tipo)
                {
                    case TipoDiRichiesta.ScaricaFile:
                        
                        logger.LogInformation("RichiestaWs: ScaricaFile");
                        richiestaRisposta.FileBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"c:\temp\TestDownload.txt"));
                        richiestaRisposta.FileName = "TestDownload.txt";
                        richiestaRisposta.Esito = "ok download";
                        break;

                    case TipoDiRichiesta.UploadFile:
                        logger.LogInformation("RichiestaWs: UploadFile");
                        Byte[] bytes = Convert.FromBase64String(richiesta.FileBase64);
                        System.IO.File.WriteAllBytes(@"c:\temp\" + richiesta.FileName, bytes);
                        richiestaRisposta.Esito = "ok upload";
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("RichiestaWs: " + ex + " - Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                richiestaRisposta.Esito = "nok";
                richiestaRisposta.Errore = ex.ToString();
            }

            return richiestaRisposta;
        }

        [Authorize]
        [HttpGet, Route("TestAuthorize")]
        public JsonResult TestAuthorize()
        {
            return Json("Ok TestAuthorize");
        }

        [HttpGet, Route("TestNoAuthorize")]
        public JsonResult TestNoAuthorize()
        {
            return Json("Ok TestNoAuthorize");
        }
    }

    //Test
    public class Richiesta
    {
        public TipoDiRichiesta Tipo { get; set; }
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
        public string Esito { get; set; }
        public string Errore { get; set; }
    }

    //Test
    public enum TipoDiRichiesta
    {
        ScaricaFile,
        UploadFile
    }
}
