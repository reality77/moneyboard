import Vue from 'vue';
//import VueResource from 'vue-resource';
import { Component } from 'vue-property-decorator';

@Component
export default class AccountsComponent extends Vue {
    accounts: Account[] = [];

    onFileChange(e: any) {

        /*this.$http({ url: '/someUrl', method: 'GET' }).then(function (response:any) {
            // success callback
        }, function (response:any) {
            // error callback
        });*/
    };
}