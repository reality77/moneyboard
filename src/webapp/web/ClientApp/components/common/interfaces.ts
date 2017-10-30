
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