using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using dto.transactions_view;
using dto;
using dal.Model;

namespace business
{
    public static class AccountTransactionsViewsCreator
    {
        public static TransactionsView GenerateAccountView(int accountId, int pageId, int itemsPerPage, MoneyboardContext db)
        {
            var tview = new TransactionsView()
            {
                PageId = pageId,
            };

            var account = db.GetAccount(accountId);

            var trx = db.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Include(t => t.Payee)
                .Where(t => t.AccountId == accountId);

            tview.PageCount = (1 + trx.Count()) / itemsPerPage;

            tview.Transactions = trx.OrderBy(t => t.Date).ThenBy(t => t.Id)
                .Skip(pageId * itemsPerPage)
                .Take(itemsPerPage)
                .ConvertToAccountTransactionRow(db);

            return tview;
        }

        public static CurrencyNumber GetBalanceAt(int accountId, DateTime date, int? beforeTransactionId, MoneyboardContext db)
        {
            var account = db.GetAccount(accountId);
            //TODO : Assert account != null

            var trxOnDate = db.Transactions
                .Where(t => t.Date.Date == date.Date)
                .Select(t => t.Id);

            decimal sum;

            if(beforeTransactionId != null)
            {
                if(!trxOnDate.Contains(beforeTransactionId.Value))
                    throw new Exception($"La transaction {beforeTransactionId} n'est pas à la date {date.Date}");

                sum = db.Transactions
                    .Where(t => t.Date.Date <= date.Date && t.Id < beforeTransactionId)
                    .Sum(t => t.Amount);
            }
            else
            {
                sum = db.Transactions
                    .Where(t => t.Date.Date <= date.Date)
                    .Sum(t => t.Amount);                    
            }

            return new CurrencyNumber { Currency = account.Currency, Value = sum + account.InitialBalance };
        }

        // Extension of Transaction
        public static IEnumerable<TransactionRow> ConvertToAccountTransactionRow(this IEnumerable<dal.Model.Transaction> transactions, MoneyboardContext db)
        {
            var enumerator = transactions.GetEnumerator();

            // Parcours for..each
            int rowId = 0;
            decimal? balance = null;

            while (enumerator.MoveNext())
            {
                // Calcul de la balance initiale
                if(balance == null)
                {
                    // on prend tous les transactions à compter de la date suivante de la première transaction de la liste
                    var firstTransaction = enumerator.Current;
                    balance = GetBalanceAt(firstTransaction.AccountId, firstTransaction.Date, firstTransaction.Id, db);
                }

                yield return new AccountTransactionRow 
                { 
                    RowId = rowId,
                    Transaction = enumerator.Current.CreateDto<dto.Transaction>(db),
                    Balance = new CurrencyNumber { Currency = enumerator.Current.Account.Currency, Value = balance.Value + enumerator.Current.Amount }
                };

                rowId++;
                balance += enumerator.Current.Amount;
            }
        }
    }
}