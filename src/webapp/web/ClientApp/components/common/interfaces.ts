
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