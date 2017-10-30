/// <reference path='../common/interfaces.ts'/>

import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

@Component
export default class AccountsComponent extends Vue {
    accounts: IAccount[] = [];

    mounted() {
        fetch(Globals.API_URL + '/accounts')
            .then(response => response.json() as Promise<IAccount[]>)
            .then(data => {
                this.accounts = data;
            })
            .catch(error => {
                console.log(error);
            });
    }
}