import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { ICategory, ICurrencyNumberStatistics, ICurrency } from '../common/interfaces';

@Component
export default class CategoryDetailViewComponent extends Vue {
    categoryId: number = 0;
    category: ICategory|null = null;
    statistics: ICurrencyNumberStatistics = { xValues: [], seriesNames:[], dataPoints:[] };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    charts: any[] = [];

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

        fetch(Globals.API_URL + '/categories/' + this.$route.params.id + '/monthlystatistics?accountId=1')
            .then(response => response.json() as Promise<ICurrencyNumberStatistics>)
            .then(data => {

                this.statistics = data;
                this.charts = [];

                var chartX = [ 'Date' ];

                data.xValues.forEach((sdate, i) => {
                    console.log(sdate);
                    var date = new Date(sdate);
                    chartX.push(date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate());
                    this.charts.push(chartX);
                });

                data.seriesNames.forEach((serie, i) => {
                    var currentY:any[] = [serie];

                    data.dataPoints[i].forEach((point:ICurrency|null, j:number) => {
                        if(point == null) {
                            currentY.push(null);
                        } else {
                            currentY.push(Math.abs(point!.value));
                        }
                    });

                    this.charts.push(currentY);
                });

                console.log(this.charts);

                this.generateChart();
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
                        xFormat: '%Y-%m-%d',
                        columns: this.charts,
                        type: 'bar'
                    },
                    bar: {
                        width: {
                            ratio: 0.2 // this makes bar width n% of length between ticks
                        }
                    },
                    axis: {
                        x: {
                            type: 'timeseries',
                            format: '%m/%Y'
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