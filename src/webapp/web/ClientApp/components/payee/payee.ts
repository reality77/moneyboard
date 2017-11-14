import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { IPayee, ICurrencyNumberStatistics } from '../common/interfaces';

@Component
export default class PayeeDetailViewComponent extends Vue {
    payeeId: number = 0;
    payee: IPayee|null = null;
    statistics: ICurrencyNumberStatistics = { xValues: [], seriesNames:[], dataPoints:[] };
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        this.payeeId = parseInt(this.$route.params.id);

        fetch(Globals.API_URL + '/payees/' + this.payeeId)
            .then(response => response.json() as Promise<IPayee>)
            .then(data => {
                this.payee = data;
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/payees/statisticsbypayee?accountId=1&payeeId=' + this.$route.params.id)
            .then(response => response.json() as Promise<ICurrencyNumberStatistics>)
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