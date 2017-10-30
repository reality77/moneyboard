import Vue from 'vue';
import { Component } from 'vue-property-decorator';

interface Currency {
    currency: string;
    value: number;
}

interface Account {
    id: number;
    name: string;
    currency: string;
    initialBalance: Currency;
    balance: Currency;
}

@Component
export default class AccountsComponent extends Vue {
    accounts: Account[] = [];

    mounted() {
        fetch('http://localhost:49871/accounts')
            .then(response => response.json() as Promise<Account[]>)
            .then(data => {
                this.accounts = data;
            })
            .catch(error => {
                console.log(error);
            });
    }
}