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

    chartX: any[] = [];
    chartY: any[] = [];

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

                this.chartX = [ 'Date' ];
                this.chartY = [ 'Montant' ];

                for(var date in this.statistics.data) {
                    this.chartX.push(this.getDate(parseInt(date)));
                    this.chartY.push(Math.abs(this.statistics.data[date].value));

                    this.generateChart();
                }
            })
            .catch(error => {
                console.log(error);
            });
    }

    generateChart() {
        var c3:any = require('c3');
        
                var chart = c3.generate({
                    bindto: '#chart',
                    data: {
                        x : 'Date',
                        xFormat: '%M',
                        columns: [
                            this.chartX,
                            this.chartY,
                        ],
                        type: 'bar'
                    },
                    bar: {
                        width: {
                            ratio: 0.75 // this makes bar width n% of length between ticks
                        }
                    },
                    axis: {
                        x: {
                            type: 'timeseries',
                            format: '%Y-%m-%d'
                        }
                    }
                });
    }

    getDate(statdate: number): Date {
        //console.log(statdate);
        var year = Math.floor(statdate / 100);
        //console.log(year);
        var month = statdate - year * 100;
        //console.log(month);
        return (new Date(year, month - 1, 1));
    }

    getFormattedDate(statdate: number): string {
        return this.getDate(statdate).toLocaleDateString();
    }
}