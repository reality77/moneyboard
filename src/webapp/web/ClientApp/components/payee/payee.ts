/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class TransactionsViewComponent extends Vue {
    payeeId: number = 1;
    payee: IPayee = { id: 0, name: "" };
    statistics: IDateStatistics = { data: {} };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        fetch(Globals.API_URL + '/payees/' + this.$route.params.id)
            .then(response => response.json() as Promise<IPayee>)
            .then(data => {
                this.payee = data;
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/payees/statisticsbypayee?accountId=1&payeeId=' + this.$route.params.id)
            .then(response => response.json() as Promise<IDateStatistics>)
            .then(data => {
                this.statistics = data;
            })
            .catch(error => {
                console.log(error);
            });

    }

    getDate(statdate: number): string {
        console.log(statdate);
        var year = Math.floor(statdate / 100);
        console.log(year);
        var month = statdate - year * 100;
        console.log(month);
        return (new Date(year, month - 1)).toLocaleDateString();
    }
}