namespace Angel.Webservice.Controllers
{
    using Angel.Webservice.Enumns;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    [AllowAnonymous]
    public class TokenController : Controller
    {
        private readonly IHttpContextAccessor accessor;
        private readonly ILogger<TokenController> logger;
        private readonly string username;
		private readonly string password;
        private readonly string key;
        private readonly int expiryInMinutes;

        public TokenController(IConfiguration configuration, ILogger<TokenController> logger, IHttpContextAccessor accessor)
        {
            key = configuration.GetSection("Token").GetValue<string>("Key");
            expiryInMinutes = configuration.GetSection("Token").GetValue<int>("ExpiryInMinutes");
			username = configuration.GetSection("Credenziali").GetValue<string>("Username");
			password = configuration.GetSection("Credenziali").GetValue<string>("Password");
            this.logger = logger;
            this.accessor = accessor;
        }
        
        [Route("CreateBearerToken")]
        public async Task<IActionResult> CreateBearerToken()
        {
            string richiestaJson = await Request.GetRawBodyStringAsync();
            if (richiestaJson == null)
            {
                logger.LogError("CreateBearerToken: Contenuto Nullo - Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                return ActionResultGenerator.GeneraRisposta("Contenuto Nullo", TipoDiRisposta.Nok);
            }

            BearerTokenLoginModel bearerTokenLoginModel = JsonConvert.DeserializeObject<BearerTokenLoginModel>(richiestaJson);

            if (bearerTokenLoginModel != new BearerTokenLoginModel(username, password))
            {
                logger.LogError("CreateBearerToken: Credenziali Errate - Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                return Unauthorized();
            }

            try
            {
                JwtToken token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create(key))
                    .AddSubject("Angel")
                    .AddIssuer("Angel")
                    .AddAudience("Angel")
                    .AddClaim("AngelId", "1000")
                    .AddExpiry(expiryInMinutes)
                    .Build();

                logger.LogInformation("CreateBearerToken: Richiesta Token da Ip: " + accessor.HttpContext.Connection.RemoteIpAddress);
                return ActionResultGenerator.GeneraRisposta(token.Value, TipoDiRisposta.Ok);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
    }
}
