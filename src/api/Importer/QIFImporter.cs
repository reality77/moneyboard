using System;
using System.Globalization;

namespace api.Importer
{
    public class QIFImporter : ImporterBase
	{
		public QIFImporter()
		{
		}

		public override ImportAccount Import(string filecontent)
		{
            string[] contents = filecontent.Split('\n');

            ImportTransaction operation = new ImportTransaction();
            ImportTransactionDetail operationdetail = null;
            ImportAccount account = new ImportAccount();

			decimal? accountInitialBalance = null;
            string accountname = null;
			string checknumber = null;

			foreach (string sline in contents)
			{
				string line = sline.TrimEnd(' ', '\r');

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
						if (operation != null)
							operation.UserDate = DateTime.Parse(data, DateTimeFormatInfo.CurrentInfo);
						break;
					case 'T':
						if (operation != null)
						{
							operation.Amount = decimal.Parse(data, NumberFormatInfo.InvariantInfo);
						}
						else
							accountInitialBalance = decimal.Parse(data, NumberFormatInfo.InvariantInfo);
						break;
					case '$':
						operationdetail.Amount = decimal.Parse(data, NumberFormatInfo.InvariantInfo);
						break;
					case 'P':
						if (operation != null)
						{
                            // --- version avec creation tiers
                            /* 
							Payee tiers = this.DataManager.FindPayee(data);

							if(tiers == null && _dicCreatedTiers.ContainsKey(data.ToLower()))
								tiers = _dicCreatedTiers[data.ToLower()];

							if (tiers == null)
							{
								tiers = new Payee() { PayeeName = data };
								this.DataManager.CurrentContext.AddToTiers(tiers);
								_dicCreatedTiers.Add(data.ToLower(), tiers);
							}
                            
							operation.Payee = tiers;*/

                            // --- version avec description
                            operation.ImportName = data;
						}
						break;
					case 'N':
						checknumber = data;
						break;
					case 'L':
						if (operation != null)
						{
							if (data.StartsWith("["))
							{
                                string targetaccountname = data.Trim('[', ']');
                                operation.ImportTargetTransferAccount = targetaccountname;
							}
							else
							{
                                operation.ImportCategory = data;
							}
						}
                        else if (accountname == null || accountname.Length == 0)
							accountname = data.Trim('[', ']');
						break;
					case 'S':
						{
                            operationdetail = new ImportTransactionDetail();
                            operationdetail.OwnerTransaction = operation;
                            operation.Details.Add(operationdetail);

							if (data.StartsWith("["))
							{
                                string targetaccountname = data.Trim('[', ']');
                                operationdetail.ImportTargetTransferAccount = targetaccountname;
							}
							else
                                operationdetail.ImportCategory = data;
						}
						break;
					case '^':
						{
							if (operation == null)
							{
								account.AccountName = account.OriginalAccountName = accountname;
								if (accountInitialBalance != null)
                                    account.ImportInitialBalance = accountInitialBalance.Value;
                            }
                            else
                                account.Transactions.Add(operation);

                            operation = new ImportTransaction();
                            operation.OwnerAccount = account;
						}
						break;
					case 'M':
						{
							//champ memo
                            operation.ImportMemo = data;
						}
						break;
					case 'E':
						{
							//champ memo (mode split)
                            operationdetail.ImportMemo = data;
						}
						break;
					case 'C':
						{
							//champ ?
						}
						break;
					default:
						{
							Console.WriteLine("UNKNOWN TAG [" + tag + "] : " + line);
						}
						break;
				}
			}

			return account;
		}
	}
}
