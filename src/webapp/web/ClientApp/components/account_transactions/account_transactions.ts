import Vue from 'vue';
import { Component } from 'vue-property-decorator';
import { ECurrency, ETransactionType } from '../common/enums';
import { Globals } from '../common/globals';
import { IAccount, ITransactionsView, ITransactionRow, ICategory, IPayee } from '../common/interfaces';

interface IEditableTransactionsViewModel extends ITransactionsView {
    transactions: IEditableTransactionRowModel[]; 
}

export interface IEditableTransactionRowModel extends ITransactionRow {
    isEditable: boolean;
}

@Component
export default class TransactionsViewComponent extends Vue {
    routeAccountId: string = "";
    
    // *** Server data
    account: IAccount = { id: 0, name: "", currency: ECurrency.Unknown, initialBalance: { currency: ECurrency.Unknown, value: 0 }, balance: { currency: ECurrency.Unknown, value: 0 } }
    categories: ICategory[] = [];
    payees: IPayee[] = [];
    transactionsview: IEditableTransactionsViewModel = { pageId: 0, pageCount: 1, transactions: [] };
    
    // *** Pagination data
    itemsPerPage: number = 25;
    pagerIndexes: number[] = [];
    pagerMaxPages: number = 5;

    mounted() {

        var routeAccountId =  this.$route.params.id;

        fetch(Globals.API_URL + '/accounts/' + routeAccountId)
            .then(response => response.json() as Promise<IAccount>)
            .then(data => {
                this.account = data;
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/categories')
            .then(response => response.json() as Promise<ICategory[]>)
            .then(data => {
                this.categories = Globals.sortDataList(data);
            })
            .catch(error => {
                console.log(error);
            });

        fetch(Globals.API_URL + '/payees')
            .then(response => response.json() as Promise<IPayee[]>)
            .then(data => {
                this.payees = Globals.sortDataList(data);
            })
            .catch(error => {
                console.log(error);
            });

        this.paginate(0);
    }

    paginate(pageId: number) {

        if (pageId < 0 || pageId >= this.transactionsview.pageCount)
            return;

        fetch(Globals.API_URL + '/transactions_view/account/' + this.$route.params.id + '?pageId=' + pageId + '&itemsPerPage=' + this.itemsPerPage)
            .then(response => response.json() as Promise<IEditableTransactionsViewModel>)
            .then(data => {

                // --- Javascript deserialization issue : Dates are deserialized as strings
                // Workaround : We rebuild the dates manually
                data.transactions.forEach(trx => {
                    trx.transaction.date = new Date(trx.transaction.date);

                    if(trx.transaction.userDate != null)
                        trx.transaction.userDate = new Date(trx.transaction.userDate);

                    trx.isEditable = false;;
                });

                this.transactionsview = data;
                
                // --- Pager

                var min = pageId - 2;

                if (min < 0)
                    min = 0;

                var max = min + 4;

                if (pageId > this.transactionsview.pageCount - 3) {

                    max = this.transactionsview.pageCount - 1;
                    min = max - 4;

                    if (min < 0)
                        min = 0;
                }

                this.pagerIndexes = [];

                for (var i = min; i <= max; i++) {
                    this.pagerIndexes.push(i);
                }
            })
            .catch(error => {
                console.log(error);
            });
    }

    getFormattedDate(date:Date) : string {
        if(date == null)
            return "";
        return date.toLocaleDateString();
    }

    getFormattedType(type:ETransactionType) : string {
        switch(type) {
            case 0:
                return "?";
            case 1:
                return "Paiement";
            case 2:
                return "Virement";
            case 3:
                return "Retrait";
            case 4:
                return "Prélèvement";
            default:
                return type.toString();        
        }
    }
}