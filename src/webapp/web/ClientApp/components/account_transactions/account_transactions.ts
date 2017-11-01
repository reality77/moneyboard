/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class TransactionsViewComponent extends Vue {
    transactionsview: ITransactionsView = { pageId: 0, pageCount: 1, transactions: [] };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {
        this.paginate(0);
    }

    paginate(pageId: number) {

        if (pageId < 0 || pageId >= this.transactionsview.pageCount)
            return;

        fetch(Globals.API_URL + '/transactions_view/account/1?pageId=' + pageId + '&itemsPerPage=' + this.itemsPerPage)
            .then(response => response.json() as Promise<ITransactionsView>)
            .then(data => {
                this.transactionsview = data;
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
}