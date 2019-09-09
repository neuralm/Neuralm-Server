import Vue from 'vue';
import App from './App.vue';
import Vuex from 'vuex';
import Router, { Route } from 'vue-router';
import UserModule, { IUserModule } from '@/modules/User.module';
import UserService from './services/UserService';
import IUserService from './interfaces/IUserService';
import ITrainingRoomService from './interfaces/ITrainingRoomService';
import TrainingRoomService from './services/TrainingRoomService';

Vue.config.productionTip = false;
Vue.use(Vuex);
Vue.use(Router);

const trainingRoomService: ITrainingRoomService = new TrainingRoomService();
const userService: IUserService = new UserService();
const userModule: IUserModule = new UserModule();

const store = new Vuex.Store({
  modules: {
    user: userModule
  }
});

/* tslint:disable */
const loadView = (name: string): any => import(`./views/${name}.vue`);
/* tslint:enable */

const router: Router = new Router({
  mode: 'history',
  routes: [
    { path: '/', name: 'home', component: () => loadView('Home'), props: { trainingRoomService } },
    { path: '/about', name: 'about', component: () => loadView('About') },
    { path: '/login', name: 'login', component: () => loadView('Login'), props: { userService } },
    { path: '/register', name: 'register', component: () => loadView('Register'), props: { userService } },

    // otherwise redirect to home.
    { path: '*', redirect: '/' }
  ]
});

router.beforeEach((to: Route, from: Route, next) => {
  // redirect to login page if not logged in and trying to access a restricted page.
  const publicPages = ['/login', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('user');
  if (authRequired && !loggedIn) {
    console.log('redirected to login!');
    return next('/login');
  }
  console.log(`Routed to ${to.name}`);
  next();
});

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
