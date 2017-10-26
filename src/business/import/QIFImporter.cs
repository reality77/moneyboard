using System;
using System.Globalization;
using System.IO;
using System.Text;
using dto.import;

namespace business.import
{
    public class QIFImporter : ImporterBase
	{
		public QIFImporter()
		{
		}

		public override ImportedAccount Import(Stream stream)
		{
            ImportedTransaction transaction = null;
            var account = new ImportedAccount();

			using (StreamReader reader
				= new StreamReader(stream, Encoding.UTF8, true, 1024, true))
			{
				while(!reader.EndOfStream)
				{
					string line = reader.ReadLine().TrimEnd(' ');

					if(transaction == null)
						transaction = new ImportedTransaction();

					if (line.StartsWith("!"))
					{
						if (line != "!Type:Bank")
							return null;
						else
							continue;
					}

					if (line.Length == 0)
						continue;

					char tag = line[0];
					string data = line.Substring(1);

					switch (tag)
					{
						case 'D':
							data = data.Replace('\'', '/');
							transaction.TransactionDate = DateTime.ParseExact(data, "dd/MM/yyyy", DateTimeFormatInfo.CurrentInfo);
							break;
						case 'T':
							transaction.Amount = decimal.Parse(data, NumberFormatInfo.InvariantInfo);
							break;
						case '$':
							transaction.Amount = decimal.Parse(data, NumberFormatInfo.InvariantInfo);
							break;
						case 'P':
							transaction.CaptionOrPayee = data;
							break;
						case 'N':
							transaction.Number = data;
							break;
						case 'L':
							if (data.StartsWith("["))
							{
								string targetaccountname = data.Trim('[', ']');
								transaction.TransferTarget = targetaccountname;
							}
							else
							{
								transaction.Category = data;
							}
							break;
						case 'S':
							{
								/*operationdetail = new ImportTransactionDetail();
								operationdetail.OwnerTransaction = operation;
								operation.Details.Add(operationdetail);

								if (data.StartsWith("["))
								{
									string targetaccountname = data.Trim('[', ']');
									operationdetail.ImportTargetTransferAccount = targetaccountname;
								}
								else
									operationdetail.ImportCategory = data;*/
								transaction.Error = "Splits not supported";
							}
							break;
						case '^':
							{
								account.Transactions.Add(transaction);
								transaction = null;
							}
							break;
						case 'M':
							{
								//champ memo
								transaction.Memo = data;
							}
							break;
						case 'E':
							{
								//champ memo (mode split)
								//operationdetail.ImportMemo = data;
								transaction.Error = "Splits not supported";
							}
							break;
						case 'C':
							{
								//champ à ignorer
							}
							break;
						default:
							{
								Console.WriteLine("UNKNOWN TAG [" + tag + "] : " + line);
							}
							break;
					}
				}
			}

			if(transaction != null)
				account.Transactions.Add(transaction);

			return account;
		}
	}
}
