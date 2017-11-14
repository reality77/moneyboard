import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { IAccount } from '../common/interfaces';

@Component
export default class AccountsComponent extends Vue {
    accounts: IAccount[]|null = null;

    mounted() {
        fetch(Globals.API_URL + '/accounts')
            .then(response => response.json() as Promise<IAccount[]>)
            .then(data => {
                this.accounts = data;
                console.log(data);
            })
            .catch(error => {
                console.log(error);
            });
    }
    
    getEditLink(accountId:number) : string {
        return "/accounts/edit/" + accountId;
    }

    getTransactionsLink(accountId:number) : string {
        return "/account_transactions/" + accountId;
    }
}