/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

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

    getTransactionsLink(accountId:number) : string {
        return "/account_transactions/" + accountId;
    }
}