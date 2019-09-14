import Vue from 'vue';
import App from './App.vue';

// Plugins
import vuetify from '@/plugins/vuetify';
import snotify from '@/plugins/vuesnotify';
import NeuralmRouter from '@/plugins/vuerouter';
import Vuex from '@/plugins/vuex';

// Modules
import UserModule, { IUserModule } from './modules/User.module';
import AppModule, { IAppModule } from './modules/App.module';

// Services
import ITrainingRoomService from './interfaces/ITrainingRoomService';
import UserService from './services/UserService';
import TrainingRoomService from './services/TrainingRoomService';
import IUserService from './interfaces/IUserService';

// Styling
import 'typeface-nunito';

Vue.config.productionTip = false;

const trainingRoomService: ITrainingRoomService = new TrainingRoomService();
const userService: IUserService = new UserService();
const userModule: IUserModule = new UserModule();
const appModule: IAppModule = new AppModule();

const store = new Vuex.Store({
  modules: {
    user: userModule,
    app: appModule
  }
});

const router = new NeuralmRouter();

router.setRoutes([
  { name: 'home', props: { trainingRoomService } },
  { name: 'dashboard' },
  { name: 'login', props: { userService } },
  { name: 'register', props: { userService } },
  { name: 'about' }
], true);

router.beforeEach((to, _, next) => {
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
  vuetify,
  snotify,
  router,
  store,
  render: (h: any) => h(App)
} as any).$mount('#app');
