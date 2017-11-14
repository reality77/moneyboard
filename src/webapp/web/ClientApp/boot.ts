import './css/site.css';
import 'bootstrap';
import Vue from 'vue';
import VueRouter from 'vue-router';
Vue.use(VueRouter);

const routes = [
    { path: '/', component: require('./components/home/home.vue.html') },
    { path: '/counter', component: require('./components/counter/counter.vue.html') },
    { path: '/fetchdata', component: require('./components/fetchdata/fetchdata.vue.html') },
    { path: '/accounts', component: require('./components/accounts/accounts.vue.html') },
    { path: '/accounts/create', component: require('./components/account-edit/account-edit.vue.html') },
    { path: '/accounts/edit/:id', component: require('./components/account-edit/account-edit.vue.html') },
    { path: '/account_import', component: require('./components/account_import/account_import.vue.html') },
    { path: '/account_transactions/:id', component: require('./components/account_transactions/account_transactions.vue.html') },
    { path: '/payees', component: require('./components/payees/payees.vue.html') },
    { path: '/payees/:id', component: require('./components/payee/payee.vue.html') },
    { path: '/categories', component: require('./components/categories/categories.vue.html') },
    { path: '/categories/:id', component: require('./components/category/category.vue.html') },
];

new Vue({
    el: '#app-root',
    router: new VueRouter({ mode: 'history', routes: routes }),
    render: h => h(require('./components/app/app.vue.html'))
});
