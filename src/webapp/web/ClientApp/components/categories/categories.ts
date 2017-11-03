/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class PayeeViewComponent extends Vue {
    categories: ICategory[] = [];
    statistics: IDateStatistics = { data: {} };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        fetch(Globals.API_URL + '/categories')
            .then(response => response.json() as Promise<ICategory[]>)
            .then(data => {
                this.categories = Globals.sortDataList(data);
            })
            .catch(error => {
                console.log(error);
            });
    }

    getCategoryUrl(categoryId: number): string {
        return "/categories/" + categoryId;
    }
}