/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class TransactionsViewComponent extends Vue {
    routeAccountId: string = "";
    account: IAccount = { id: 0, name: "", currency: "Unknown", initialBalance: { currency: "Unknown", value: 0 }, balance: { currency: "Unknown", value: 0 } }
    transactionsview: ITransactionsView = { pageId: 0, pageCount: 1, transactions: [] };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        var routeAccountId =  this.$route.params.id;

        fetch(Globals.API_URL + '/accounts/' + routeAccountId)
            .then(response => response.json() as Promise<IAccount>)
            .then(data => {
                this.account = data;
            })
            .catch(error => {
                console.log(error);
            });


        this.paginate(0);
    }

    paginate(pageId: number) {

        if (pageId < 0 || pageId >= this.transactionsview.pageCount)
            return;

        fetch(Globals.API_URL + '/transactions_view/account/' + this.$route.params.id + '?pageId=' + pageId + '&itemsPerPage=' + this.itemsPerPage)
            .then(response => response.json() as Promise<ITransactionsView>)
            .then(data => {
                this.transactionsview = data;

                // --- Javascript deserialization issue : Dates are deserialized as strings
                // Workaround : We rebuild the dates manually
                this.transactionsview.transactions.forEach(trx => {
                    trx.transaction.date = new Date(trx.transaction.date);

                    if(trx.transaction.userDate != null)
                        trx.transaction.userDate = new Date(trx.transaction.userDate);
                });
                // ---

                var min = pageId - 2;

                if (min < 0)
                    min = 0;

                var max = min + 4;

                if (pageId > this.transactionsview.pageCount - 3) {

                    max = this.transactionsview.pageCount - 1;
                    min = max - 4;

                    if (min < 0)
                        min = 0;
                }

                this.pagerIndexes = [];

                for (var i = min; i <= max; i++) {
                    this.pagerIndexes.push(i);
                }
            })
            .catch(error => {
                console.log(error);
            });
    }

    getFormattedDate(date:Date) : string {
        if(date == null)
            return "";
        return date.toLocaleDateString();
    }
}