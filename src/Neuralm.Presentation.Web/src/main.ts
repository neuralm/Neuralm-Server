import Vue from 'vue';
import App from './App.vue';
import router from './router';
import Vuex from 'vuex';
import UserModule, { IUserModule } from '@/store/User.module';
import UserService from './services/UserService';

Vue.config.productionTip = false;
Vue.use(Vuex);

const userModule: IUserModule = new UserModule(new UserService());

const store = new Vuex.Store({
  modules: {
    userModule
  }
});

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
