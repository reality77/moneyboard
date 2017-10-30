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
    detectedPayee: string;
    detectedPayeeId: number;
    detectedCategoryId: number;
    importTransactionHash: string;
}

class UnsavedTransactionChanges {
    unsavedNewPayeeName: string;
    unsavedNewCategoryName: string;
    unsavedNewCaption: string;
    unsavedExistingPayeeId: number;
    unsavedExistingCategoryId: number;
}

@Component
export default class AccountTransactionsImportComponent extends Vue {
    accounts: IImportedAccount[] = [];
    payees: IPayee[] = [];
    categories: ICategory[] = [];
    unsavedTransactionChanges: { [key:string]:UnsavedTransactionChanges; } = {}

    mounted() {
        fetch(Globals.API_URL + '/payees')
            .then(response => response.json() as Promise<IPayee[]>)
            .then(data => {
                this.payees = data;
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/categories')
            .then(response => response.json() as Promise<ICategory[]>)
            .then(data => {
                this.categories = data;
            })
            .catch(error => {
                console.log(error);
            });
    }

    // --- Events

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
        .then(data => 
        {
            var acc = data[0];
            for(var i=0; i < acc.transactions.length; i++) {
                var trx = acc.transactions[i];

                if(trx.detectedPayeeId == null && trx.detectedPayee != null) {
                    var unsaved = new UnsavedTransactionChanges();
                    unsaved.unsavedNewPayeeName = trx.detectedPayee;
                    unsaved.unsavedNewCaption = "";
                    unsaved.unsavedNewCategoryName = "";
                    this.unsavedTransactionChanges[trx.importTransactionHash] = unsaved;
                }
            }

             this.accounts = data; 
        })
        .catch(error => console.log(error));
    };

    // --- Methods for temporary Payee/Category/Caption selection

    onValidateTemporaryChanges(trx:IImportedTransaction) {
        var unsaved = this.getUnsavedTransactionChanges(trx);

        if(unsaved == null) {
            alert("No changes");
            return;
        }

        trx.detectedCaption = unsaved.unsavedNewCaption;
        
        if(unsaved.unsavedExistingPayeeId != null) {

            trx.detectedPayeeId = unsaved.unsavedExistingPayeeId;
            this.validateTemporaryCategory(trx, unsaved);
        }
        else {
            // Create payee
            Axios.post(Globals.API_URL + '/payees', unsaved.unsavedNewPayeeName)
            .then(response => response.data as Promise<IPayee>)
            .then(data => 
            {
                //TODO : add payee in payees variable
                trx.detectedPayeeId = data.id;

                this.validateTemporaryCategory(trx, unsaved);
            })
            .catch(error => alert(error));
        }
    }

    private validateTemporaryCategory(trx:IImportedTransaction, unsaved:UnsavedTransactionChanges) {

        if(unsaved.unsavedExistingCategoryId != null) {
            trx.detectedCategoryId = unsaved.unsavedExistingCategoryId;
            this.refreshTransactions(trx.detectedPayee, trx.detectedPayeeId, trx.detectedCategoryId, trx.detectedCaption);
        }
        else {
            // Create category
            Axios.post(Globals.API_URL + '/categories', unsaved.unsavedNewCategoryName)
            .then(response => response.data as Promise<ICategory>)
            .then(data => 
            {
                //TODO : add category in categories variable
                trx.detectedCategoryId = data.id;
                
                this.refreshTransactions(trx.detectedPayee, trx.detectedPayeeId, trx.detectedCategoryId, trx.detectedCaption);
            })
            .catch(error => alert(error));
        }    
    }

    private refreshTransactions(detectedPayee:string, payeeId:number, categoryId:number, caption:string) {
        
        for(var i=0; i < this.accounts[0].transactions.length; i++) {
            var trx = this.accounts[0].transactions[i];

            if(trx.detectedPayee === detectedPayee) {
                trx.detectedPayeeId = payeeId;
                trx.detectedCategoryId = categoryId;
                trx.detectedCaption = caption;
            }
        }
    }

    private getUnsavedTransactionChanges(trx:IImportedTransaction) : UnsavedTransactionChanges {
        var hash = trx.importTransactionHash;
        var unsaved = this.unsavedTransactionChanges[hash];
        return unsaved;
    }
    
    // --- Display method helpers

    isTransactionDisplayed(trx: IImportedTransaction) {
        return !trx.detectionSucceded || (trx.detectedPayee != null && trx.detectedPayeeId == null);
    }

}