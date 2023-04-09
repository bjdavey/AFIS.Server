using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SourceAFIS.Simple;
using System;
using System.Drawing;
using Wsq2Bmp;

namespace AFIS.Server.Controllers
{
    //[Authorize]
    [Route("/AFIS/[action]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        static AfisEngine Afis;
        static WsqDecoder wsqDecoder;
        public VerificationController()
        {
            Afis = new AfisEngine();
            wsqDecoder = new WsqDecoder();
        }

        private static Bitmap WSQtoBitmap(string wsq)
        {
            var data = Convert.FromBase64String(wsq);
            return wsqDecoder.Decode(data);
        }

        [HttpPost]
        public IActionResult Verify([FromForm] string wsq1, [FromForm] string wsq2)
        {
            try
            {
                Fingerprint fp1 = new Fingerprint();
                fp1.AsBitmap = WSQtoBitmap(wsq1);
                Person person1 = new Person();
                person1.Fingerprints.Add(fp1);
                Afis.Extract(person1);


                Fingerprint fp2 = new Fingerprint();
                fp2.AsBitmap = WSQtoBitmap(wsq2);
                Person person2 = new Person();
                person2.Fingerprints.Add(fp2);
                Afis.Extract(person2);

                float score = Afis.Verify(person1, person2);
                return Ok(new
                {
                    Status = true,
                    VerificationResult = (score > 0),
                    Score = score
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = false,
                    Error = ex.Message,
                    InnerException = (ex.InnerException != null) ? ex.InnerException.Message : null
                });
            }
        }

    }
}
