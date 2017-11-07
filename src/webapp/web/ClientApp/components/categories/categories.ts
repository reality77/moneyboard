
import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { ICategory } from '../common/interfaces';

@Component
export default class PayeeViewComponent extends Vue {
    categories: ICategory[] = [];
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