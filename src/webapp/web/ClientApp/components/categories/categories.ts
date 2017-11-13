
import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { ICategory, ICurrencyNumberStatistics, ICurrency } from '../common/interfaces';

@Component
export default class PayeeViewComponent extends Vue {
    categories: ICategory[] = [];
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    charts: any[] = [];

    mounted() {

        fetch(Globals.API_URL + '/categories')
            .then(response => response.json() as Promise<ICategory[]>)
            .then(data => {
                this.categories = Globals.sortDataList(data);
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/categories/monthlystatistics?accountId=1')
            .then(response => response.json() as Promise<ICurrencyNumberStatistics>)
            .then(data => {

                this.charts = [];

                var chartX = [ 'Date' ];

                for(var xval in data.xValues) {
                    chartX.push(xval);
                }

                this.charts.push(chartX);
                
                data.seriesNames.forEach((serie, i) => {
                    var currentY:any[] = [serie];

                    data.dataPoints[i].forEach((point:ICurrency|null, j:number) => {
                        if(point == null) {
                            currentY.push(null);
                        } else {
                            currentY.push(point!.value);
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
                        xFormat: '%M',
                        columns: this.charts,
                        type: 'pie'
                    },
                    axis: {
                        x: {
                            type: 'timeseries',
                            format: '%Y-%m-%d'
                        }
                    }
                });
    }

    getCategoryUrl(categoryId: number): string {
        return "/categories/" + categoryId;
    }
}