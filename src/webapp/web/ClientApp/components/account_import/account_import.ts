import Vue from 'vue';
import Axios from 'axios';

import { Component } from 'vue-property-decorator';
import { Globals, ImportPartData } from '../common/globals';

interface IImportedAccount {
    name: string;
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
    detectedRegexId: number;
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
    payeeData:ImportPartData;
    categoryData:ImportPartData;
    captionData:ImportPartData;
}

@Component({
    components: {
        ImportPart: require('../parts/import-part/import-part.vue.html')
    },
})
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
                this.$emit('payees_loaded');
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

                    unsaved.payeeData = new ImportPartData();
                    unsaved.payeeData.id = -1;
                    unsaved.payeeData.name = trx.detectedPayee;
                    unsaved.payeeData.setdefault = true;

                    unsaved.categoryData = new ImportPartData();
                    unsaved.categoryData.id = -1;
                    unsaved.categoryData.name = "";
                    unsaved.categoryData.setdefault = true;

                    unsaved.captionData = new ImportPartData();
                    unsaved.captionData.id = -1;
                    unsaved.captionData.name = "";
                    unsaved.captionData.setdefault = true;

                    this.unsavedTransactionChanges[trx.importTransactionHash] = unsaved;
                }
            }

             this.accounts = data; 
        })
        .catch(error => console.log(error));
    };

    // --- Methods for temporary Payee/Category/Caption selection

    /*onUnsavedPayeeChanged(trx:IImportedTransaction) {
        var unsaved = this.getUnsavedTransactionChanges(trx);
        console.log("CHANGED PAYEE : " + unsaved.payeeData.id);
    }

    onUnsavedCategoryChanged(trx:IImportedTransaction) {
        var unsaved = this.getUnsavedTransactionChanges(trx);
        console.log("CHANGED CATEGORY : " + unsaved.categoryData.id);
    }

    onUnsavedCaptionChanged(trx:IImportedTransaction) {
        var unsaved = this.getUnsavedTransactionChanges(trx);
        console.log("CHANGED CAPTION : " + unsaved.captionData.name);
    }*/

    onValidateTemporaryChanges(trx:IImportedTransaction) {
        var unsaved = this.getUnsavedTransactionChanges(trx);

        if(unsaved == null) {
            alert("No changes");
            return;
        }

        trx.detectedCaption = unsaved.captionData.name;
        
        if(unsaved.payeeData.id != null && unsaved.payeeData.id >= 0) {

            trx.detectedPayeeId = unsaved.payeeData.id;
            this.validateTemporaryCategory(trx, unsaved);
        }
        else {
            // Create payee
            Axios.post(Globals.API_URL + '/payees', JSON.stringify(unsaved.payeeData.name), {
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                }
            })
            .then(response => response.data as Promise<IPayee>)
            .then(data => 
            {
                //TODO : add payee in payees variable
                this.payees = this.payees.concat([data]);
                trx.detectedPayeeId = data.id;
                unsaved.payeeData.id = data.id;

                console.log(data);

                this.validateTemporaryCategory(trx, unsaved);
            })
            .catch(error => alert(error));
        }
    }

    private validateTemporaryCategory(trx:IImportedTransaction, unsaved:UnsavedTransactionChanges) {

        if(unsaved.categoryData.id != null && unsaved.categoryData.id >= 0) {
            trx.detectedCategoryId = unsaved.categoryData.id;
            this.registerPayeeRule(trx, unsaved);
        }
        else if (unsaved.categoryData.name != null && unsaved.categoryData.name.trim().length > 0) {
            // Create category
            Axios.post(Globals.API_URL + '/categories', JSON.stringify(unsaved.categoryData.name), {
                headers: {
                    "Content-Type": "application/json;charset=utf-8",
                }
            })
            .then(response => response.data as Promise<ICategory>)
            .then(data => 
            {
                //TODO : add category in categories variable
                this.categories = this.categories.concat([data]);
                trx.detectedCategoryId = data.id;
                unsaved.categoryData.id = data.id;
                
                this.registerPayeeRule(trx, unsaved);
            })
            .catch(error => alert(error));
        }
        else {
            this.registerPayeeRule(trx, unsaved);
        }
    }

    private registerPayeeRule(trx:IImportedTransaction, unsaved:UnsavedTransactionChanges) {
        
        if(!unsaved.payeeData.setdefault && !unsaved.categoryData.setdefault && !unsaved.captionData.setdefault)
            return;

        console.log(unsaved);

        // Create payee rule
        var payeeRule = {
            regexId: trx.detectedRegexId,
            importedCaption: trx.detectedPayee,
            payeeId: trx.detectedPayeeId,
            categoryId: (unsaved.categoryData.setdefault ? trx.detectedCategoryId : null),
            transactionCaption: (unsaved.captionData.setdefault ? trx.detectedCaption : null)
        };

        console.log(payeeRule);

        Axios.post(Globals.API_URL + '/import/registerpayeerule', JSON.stringify(payeeRule), {
            headers: {
                "Content-Type": "application/json;charset=utf-8",
            }
        })
        .then(response => 
        {
            this.refreshTransactions(trx.detectedPayee, payeeRule.payeeId, payeeRule.categoryId, payeeRule.transactionCaption);
        })
        .catch(error => alert(error));
    }

    private refreshTransactions(detectedPayee:string, payeeId:number, categoryId:number|null, caption:string|null) {
        
        for(var i=0; i < this.accounts[0].transactions.length; i++) {
            var trx = this.accounts[0].transactions[i];

            if(trx.detectedPayee === detectedPayee) {
                trx.detectedPayeeId = payeeId;
                if(categoryId != null)
                    trx.detectedCategoryId = categoryId;
                
                if(caption != null)
                    trx.detectedCaption = caption;
            }
        }
    }

    private getUnsavedTransactionChanges(trx:IImportedTransaction) : UnsavedTransactionChanges {
        var hash = trx.importTransactionHash;
        var unsaved = this.unsavedTransactionChanges[hash];
        return unsaved;
    }

    // --- Upload transactions
    uploadTransactions(acc: IAccount) {

        Axios.post(Globals.API_URL + '/import/uploadtoaccount', JSON.stringify(acc), {
            headers: {
                "Content-Type": "application/json;charset=utf-8",
            }
        })
            .then(response => {
                alert("Transactions uploaded");
            })
            .catch(error => alert(error));
    }

    // --- Display method helpers

    isTransactionDisplayed(trx: IImportedTransaction) {
        return !trx.detectionSucceded || (trx.detectedPayee != null && trx.detectedPayeeId == null);
    }

    selectPayee(trx: IImportedTransaction, payeeId: number) {
        var unsaved = this.getUnsavedTransactionChanges(trx);
        unsaved.payeeData.id = payeeId;
    }

    selectCategory(trx: IImportedTransaction, categoryId: number) {
        var unsaved = this.getUnsavedTransactionChanges(trx);
        unsaved.categoryData.id = categoryId;
    }

    displayPayeeName(payeeId: number) : string {
        var array = this.payees.filter(p => p.id == payeeId);
        if (array.length > 0)
            return array[0].name;
        else
            return "";
    }

    displayCategoryName(categoryId: number): string {
        var array = this.categories.filter(p => p.id == categoryId);
        if (array.length > 0)
            return array[0].name;
        else
            return "";
    }
}