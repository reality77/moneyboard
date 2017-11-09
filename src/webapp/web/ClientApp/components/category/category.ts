/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { ICategory, IDateStatistics } from '../common/interfaces';

@Component
export default class CategoryDetailViewComponent extends Vue {
    categoryId: number = 0;
    category: ICategory|null = null;
    statistics: IDateStatistics = { data: {} };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        this.categoryId = parseInt(this.$route.params.id);

        fetch(Globals.API_URL + '/categories/' + this.categoryId)
            .then(response => response.json() as Promise<ICategory>)
            .then(data => {
                this.category = data;
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/statistics/monthly?accountId=1&idOrigin=' + this.$route.params.id + '&origin=Category')
            .then(response => response.json() as Promise<IDateStatistics>)
            .then(data => {
                this.statistics = data;
            })
            .catch(error => {
                console.log(error);
            });
    }

    getDate(statdate: string): string {
        console.log(statdate);
        return Globals.formatDateMonthYear(new Date(statdate));
    }
}