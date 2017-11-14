
import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { ICategory, ICurrencyNumberStatistics, ICurrency } from '../common/interfaces';

@Component
export default class CategoriesViewComponent extends Vue {
    categories: ICategory[] = [];
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    chart_revenues: any[] = [];
    chart_expenses: any[] = [];

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

                this.chart_revenues = [];
                this.chart_expenses = [];

                var chartX = [ 'Date' ];

                for(var xval in data.xValues) {
                    chartX.push(xval);
                }

                this.chart_revenues.push(chartX);
                this.chart_expenses.push(chartX);
                
                data.seriesNames.forEach((serie, i) => {
                    var currentY:any[] = [serie];
                    var isRevenue;
                    
                    data.dataPoints[i].forEach((point:ICurrency|null, j:number) => {


                        if(point == null) {
                            currentY.push(null);
                        } else {
                            if(point!.value > 0)
                                isRevenue = true;
                            else if(point!.value < 0)
                                isRevenue = false;
                            currentY.push(Math.abs(point!.value));
                        }
                    });
                
                    if(isRevenue)
                        this.chart_revenues.push(currentY);
                    else
                        this.chart_expenses.push(currentY);
                });

                this.generateCharts();
            })
            .catch(error => {
                console.log(error);
            });
    }

    generateCharts() {
        var c3:any = require('c3');
        
        var chart = c3.generate({
            bindto: '#chart_expenses',
            data: {
                x : 'Date',
                xFormat: '%M',
                columns: this.chart_expenses,
                type: 'pie'
            },
            axis: {
                x: {
                    type: 'timeseries',
                    format: '%Y-%m-%d'
                }
            }
        });

        var chart = c3.generate({
            bindto: '#chart_revenues',
            data: {
                x : 'Date',
                xFormat: '%M',
                columns: this.chart_revenues,
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