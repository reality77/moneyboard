using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using api.Importer;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("Import")]
    public class ImportController : MoneyboardController
    {
        [HttpPost("")]
        public IActionResult Import(string fileName)
        {
            var listImportedAccount = new List<ImportAccount>();

            var stream = Request.Body;

            using (StreamReader reader
                  = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                string fileContent = reader.ReadToEnd();

                // --- Import des données du fichier

                ImporterBase importer = null;

                if (fileName.ToLower().EndsWith(".ofx"))
                {
                    importer = new OFXImporter();
                }
                else if (fileName.ToLower().EndsWith(".qif"))
                {
                    importer = new QIFImporter();
                }
                else
                {
                    return BadRequest("[IMPORT] File extension not supported");
                }

                ImportAccount importedAccount = null;

                try
                {
                    importedAccount = importer.Import(fileContent);
                    listImportedAccount.Add(importedAccount);
                }
                catch (Exception ex)
                {
                    return BadRequest($"[IMPORT] {ex.Message}");
                }
            }

            return new OkObjectResult(listImportedAccount);
        }
    }
}