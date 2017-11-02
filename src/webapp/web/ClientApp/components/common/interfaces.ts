enum ETransactionType {
    Unknown = 0,
    Payment = 1,
    Transfer = 2,
    Withdrawal = 3,
    Debit = 4
}

interface ICurrency {
    currency: string;
    value: number;
}

interface IAccount {
    id: number;
    name: string;
    currency: string;
    initialBalance: ICurrency;
    balance: ICurrency;
}

interface IPayee {
    id: number;
    name: string;
}

interface ICategory {
    id: number;
    name: string;
}

interface ITransactionsView {
    pageId: number,
    pageCount: number;
    transactions: ITransactionRow[]; 
}

interface ITransactionRow {
    rowId: number;
    balance: ICurrency;
    transaction: ITransaction;
}

interface ITransaction {
    id: number;
    accountId: number;
    accountName: string;
    caption: string;
    amount: ICurrency;
    type: ETransactionType;
    date: Date;
    userDate: Date;
    categoryId: number;
    categoryName: string;
    payeeId: number;
    payeeName: string;
    comment: string;
    importedTransactionCaption: string;
    importedTransactionHash: string;
}

interface IDateStatistics {
    data: { [key: number]: ICurrency; }; 
}