import { ECurrency, ETransactionType } from '../common/enums'

export interface ICurrency {
    currency: ECurrency;
    value: number;
}

export interface IAccount {
    id: number;
    name: string;
    currency: ECurrency;
    initialBalance: ICurrency;
    balance: ICurrency;
}

export interface IDataIdName {
    id: number;
    name: string;
}

export interface IPayee extends IDataIdName {
    id: number;
    name: string;
}

export interface ICategory extends IDataIdName {
    id: number;
    name: string;
}

export interface ITransactionsView {
    pageId: number,
    pageCount: number;
    transactions: ITransactionRow[]; 
}

export interface ITransactionRow {
    rowId: number;
    balance: ICurrency;
    transaction: ITransaction;
}

export interface ITransaction {
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

export interface IDateStatistics {
    data: { [key: string]: ICurrency; }; 
}