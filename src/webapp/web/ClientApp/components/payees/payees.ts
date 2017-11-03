/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class PayeeViewComponent extends Vue {
    payees: IPayee[] = [];
    statistics: IDateStatistics = { data: {} };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        fetch(Globals.API_URL + '/payees')
            .then(response => response.json() as Promise<IPayee[]>)
            .then(data => {
                this.payees = Globals.sortDataList(data);
            })
            .catch(error => {
                console.log(error);
            });
    }

    getPayeeUrl(payeeId: number): string {
        return "/payees/" + payeeId;
    }
}