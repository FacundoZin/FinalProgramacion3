
import { createRouter, createWebHistory } from 'vue-router'
import ClientsView from '../views/ClientsView.vue'
import TransactionsView from '../views/TransactionsView.vue'
import HistoryView from '../views/HistoryView.vue'
import AnalysisView from '../views/AnalysisView.vue'
import BestExchangeView from '../views/BestExchangeView.vue'

const routes = [
  { path: '/', redirect: '/clients' },
  { path: '/clients', component: ClientsView },
  { path: '/transactions', component: TransactionsView },
  { path: '/history', component: HistoryView },
  { path: '/analysis', component: AnalysisView },
  { path: '/best-exchange', component: BestExchangeView }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
