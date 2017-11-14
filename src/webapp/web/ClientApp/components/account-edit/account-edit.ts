import Vue from 'vue';
import Axios from 'axios';

import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';
import { IAccount } from '../common/interfaces';

@Component
export default class AccountEditComponent extends Vue {
    account: IAccount|null = null;
    editmode: boolean = false;

    message: string|null = null;
    isError: boolean = false;

    mounted() {

        var id = parseInt(this.$route.params.id);
        
        if(!isNaN(id)) {
            fetch(Globals.API_URL + '/accounts/' + id)
                .then(response => response.json() as Promise<IAccount>)
                .then(data => {
                    this.account = data;
                    console.log(data);
                })
                .catch(error => {
                    console.log(error);
                });

            this.editmode = true;
        }
        else {
            this.editmode = false;
            this.account = {
                id: 0,
                name: "",
                currency: "EUR",
                initialBalance:  { value: 0.00, currency: "EUR" }
            } as IAccount;
        }
    }

    validate() {
        if(this.editmode) {

            Axios.put(Globals.API_URL + '/accounts/' + this.$route.params.id, JSON.stringify(this.account), {
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                }
            })
            .then(response => {
                this.isError = false;
                this.message = "Les changement ont été sauvegardés";
            })
            .catch(error => {
                this.isError = true;
                this.message = error;
            });
            
        }
        else {

            Axios.post(Globals.API_URL + '/accounts', JSON.stringify(this.account), {
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                }
            })
            .then(response => {
                this.isError = false;
                this.message = "La création a été effectuée";
            })
            .catch(error => {
                this.isError = true;
                this.message = error;
            });

        }
    }
}