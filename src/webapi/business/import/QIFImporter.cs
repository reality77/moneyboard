using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using dto.import;


namespace business.import
{
    public class QIFImporter : ImporterBase
	{
		SHA1 _sha1;

		public QIFImporter()
		{
			_sha1 = SHA1.Create();
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
							transaction.TransactionDate = DateTime.ParseExact(data, "dd/MM/yyyy", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None);
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
								transaction.GenerateHash(_sha1);
								var duplicates = base.RaiseOnFindDuplicates(transaction.ImportTransactionHash);

								if(duplicates == null || duplicates.Count() == 0)
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
			{
				transaction.GenerateHash(_sha1);
				var duplicates = base.RaiseOnFindDuplicates(transaction.ImportTransactionHash);

				if(duplicates == null || duplicates.Count() == 0)
					account.Transactions.Add(transaction);
			}

			return account;
		}
	}
}
