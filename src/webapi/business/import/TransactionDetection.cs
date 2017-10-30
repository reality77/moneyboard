using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using dto.import;
using System.IO;
using dal;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace business.import
{
    public class TransactionDetection
    {
        const string USERDATE_TAG = "user_date_";

        IQueryable<dal.models.ImportRegex> _importRegexes;

        public TransactionDetection(MoneyboardContext db)
        {
            _importRegexes = db.ImportRegexes.Include(ir => ir.ImportPayeeSelections);
        }

        public void DetectTransaction(ImportedTransaction transaction)
        {
            foreach(var importRegEx in _importRegexes)
            {
                var match = importRegEx.Regex.Match(transaction.CaptionOrPayee);

                if(!match.Success)
                    continue;

                transaction.DetectionSucceded = true;

                foreach(Group group in match.Groups)
                {
                    if(!group.Success)
                        continue;

                    if(group.Name.StartsWith(USERDATE_TAG))
                    {
                        DateTime date;
                        if(DateTime.TryParseExact(group.Value, group.Name.Remove(0, USERDATE_TAG.Length), DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out date))
                            transaction.DetectedUserDate = date;
                        else
                        {
                            //TODO : errors
                        }
                    }
                    else
                    {
                        switch(group.Name)
                        {
                            case "mode":
                                transaction.DetectedMode = group.Value;
                                break;
                            case "payee":
                            {
                                if (importRegEx.ImportPayeeSelections != null && importRegEx.ImportPayeeSelections.Count() > 0)
                                {
                                    var payeeSelection = importRegEx.ImportPayeeSelections.SingleOrDefault(p => p.ImportedCaption.ToLower() == group.Value.ToLower());

                                    if (payeeSelection != null)
                                    {
                                        transaction.DetectedPayeeId = payeeSelection.PayeeId;
                                        transaction.DetectedCategoryId = payeeSelection.CategoryId;
                                        transaction.DetectedCaption = payeeSelection.TransactionCaption;
                                    }
                                }
                                break;
                            }
                            case "comment":
                                transaction.DetectedComment = group.Value;
                                break;
                        }
                    }
                }
            }
        }
    }
}