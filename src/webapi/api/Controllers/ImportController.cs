using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using business.import;
using dto.import;
using api.filters;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("Import")]
    public class ImportController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public ImportController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        [HttpPost("prepare")]
        //[RequestFormSizeLimit(valueLengthLimit:200000)]
        public IActionResult PrepareImport()
        {
            var listImportedAccount = new List<ImportedAccount>();

            /*using(var reader = new StreamReader(this.Request.Body))
            {
                string body = reader.ReadToEnd();
            }*/

            foreach (var file in  Request.Form.Files)
            {
                var stream = file.OpenReadStream();

                // --- Import des données du fichier
                ImporterBase importer = null;

                if (file.FileName.ToLower().EndsWith(".ofx"))
                {
                    importer = new OFXImporter();
                }
                else if (file.FileName.ToLower().EndsWith(".qif"))
                {
                    importer = new QIFImporter();
                }
                else
                {
                    return BadRequest("[IMPORT] File extension not supported");
                }

                ImportedAccount importedAccount = null;

                try
                {
                    importedAccount = importer.Import(stream);


                    var detector = new TransactionDetection(_db);
                    foreach(var transaction in importedAccount.Transactions)
                        detector.DetectTransaction(transaction);

                    listImportedAccount.Add(importedAccount);
                }
                catch (Exception ex)
                {
                    return BadRequest($"[IMPORT] {ex.Message}");
                }
            }

            return new OkObjectResult(listImportedAccount);
        }

        [HttpPost("registerpayeerule")]
        //[RequestFormSizeLimit(valueLengthLimit:200000)]
        public IActionResult RegisterPayeeRule([FromBody] dto.import.PayeeRuleRegistration payeeRegistration)
        {
            var payeeSelection = _db.ImportPayeeSelections.Where(ps => ps.ImportRegexId == payeeRegistration.RegexId && ps.TransactionCaption.Trim().ToLower() == payeeRegistration.TransactionCaption.Trim().ToLower()).SingleOrDefault();

            if(payeeSelection != null)
                return BadRequest($"There is already a ruleA on regex {payeeRegistration.RegexId} for the caption '{payeeRegistration.TransactionCaption}'");

            payeeSelection = new dal.models.ImportPayeeSelection
            {
                ImportRegexId = payeeRegistration.RegexId,
                ImportedCaption = payeeRegistration.ImportedCaption,
                PayeeId = payeeRegistration.PayeeId,
                CategoryId = payeeRegistration.CategoryId,
                TransactionCaption = payeeRegistration.TransactionCaption,
            };

            _db.ImportPayeeSelections.Add(payeeSelection);
            _db.SaveChanges();

            return NoContent();
        }
    }
}