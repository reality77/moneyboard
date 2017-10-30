import Vue from 'vue';
import Axios from 'axios';

import { Component } from 'vue-property-decorator';
import { Globals } from '../common/globals';

interface IImportedAccount {
    transactions: IImportedTransaction[];
}

interface IImportedTransaction {
    transactionDate: Date;
    captionOrPayee: string;
    category: string;
    transferTarget: string;
    amount: number;
    memo: string;
    number: string;
    detectionSucceded: boolean;
    detectedUserDate: Date;
    detectedMode: string;
    detectedComment: string;
    detectedCaption: string;
    detectedPayeeId: number;
    detectedCategoryId: number;
    importTransactionHash: string;
}

@Component
export default class AccountTransactionsImportComponent extends Vue {
    accounts: IImportedAccount[] = [];

    onFileChange(e: any) {

        //var files = e.target.files || e.dataTransfer.files;
        var formData = new FormData();

        formData.append("file", e.target.files[0]);

        Axios.post(Globals.API_URL + '/import/prepare', formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
        .then(response => response.data as Promise<IImportedAccount[]>)
        .then(data => { this.accounts = data; })
        .catch(error => console.log(error));
    };
}