namespace Angel.Webservice.Helpers
{
    using Angel.Webservice.Enumns;
    using Microsoft.AspNetCore.Mvc;

    public static class ActionResultGenerator
    {
        public static IActionResult GeneraRisposta(object message, TipoDiRisposta tipoRisposta)
        {
            switch (tipoRisposta)
            {
                case TipoDiRisposta.Ok:
                    return new JsonResult(message);
                case TipoDiRisposta.Nok:
                    return new BadRequestObjectResult(message);
                default:
                    return new JsonResult(message);
            }
        }
    }
}
