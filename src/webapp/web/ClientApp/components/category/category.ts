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

        fetch(Globals.API_URL + '/categories/statisticsbycategory?accountId=1&categoryId=' + this.$route.params.id)
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