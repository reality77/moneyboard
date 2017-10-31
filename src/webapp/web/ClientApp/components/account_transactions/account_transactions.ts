/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class TransactionsViewComponent extends Vue {
    transactionsview: ITransactionsView = { transactions: [] };

    mounted() {
        fetch(Globals.API_URL + '/transactions_view/account/1')
            .then(response => response.json() as Promise<ITransactionsView>)
            .then(data => {
                this.transactionsview = data;
            })
            .catch(error => {
                console.log(error);
            });
    }
}