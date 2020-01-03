import Vue from 'vue';
import App from './App.vue';

// Plugins
import vuetify from '@/plugins/vuetify';
import snotify from '@/plugins/vuesnotify';
import NeuralmRouter from '@/plugins/vuerouter';
import Vuex from '@/plugins/vuex';
import '@/plugins/vee-validate';

// Modules
import UserModule, { IUserModule } from './modules/User.module';
import AppModule, { IAppModule } from './modules/App.module';
import TrainingRoomModule, { ITrainingRoomModule } from './modules/TrainingRoom.module';
import DashboardModule, { IDashboardModule } from './modules/Dashboard.module';
import TrainingSessionModule, { ITrainingSessionModule } from './modules/TrainingSession.module';

// Services
import ITrainingRoomService from './interfaces/ITrainingRoomService';
import TrainingRoomService from './services/TrainingRoomService';
import IUserService from './interfaces/IUserService';
import UserService from './services/UserService';
import ITrainingSessionService from './interfaces/ITrainingSessionService';
import TrainingSessionService from './services/TrainingSessionService';

// Styling
import 'typeface-nunito';

// Messaging
import WSNetworkConnector from './messaging/WSNetworkConnector';
import JsonMessageSerializer from './messaging/JsonMessageSerializer';
import { IMessageSerializer } from './interfaces/IMessageSerializer';
import { IMessageProcessor } from './interfaces/IMessageProcessor';
import MessageProcessor from './messaging/MessageProcessor';
import { INetworkConnector } from './interfaces/INetworkConnector';
import INeuralmMQClient from './interfaces/INeuralmMQClient';
import NeuralmMQClient from './messaging/NeuralmMQClient';
import ErrorResponseHandler from './handlers/ErrorResponseHandler';
import { IRootState } from './interfaces/IRootState';

Vue.config.productionTip = false;
const messageSerializer: IMessageSerializer = new JsonMessageSerializer();
const messageProcessor: IMessageProcessor = new MessageProcessor();
const url: string = 'ws://localhost:5000/neuralm';
const wsNetworkConnector: INetworkConnector = new WSNetworkConnector(messageSerializer, messageProcessor, url);
wsNetworkConnector.connectAsync().then((_) => {
  wsNetworkConnector.start();
});

const neuralmMQClient: INeuralmMQClient = new NeuralmMQClient(messageProcessor, wsNetworkConnector);
const errorResponseHandler: ErrorResponseHandler = new ErrorResponseHandler(messageProcessor);
neuralmMQClient.addHandler(errorResponseHandler);
const trainingRoomService: ITrainingRoomService = new TrainingRoomService(neuralmMQClient);
const trainingSessionService: ITrainingSessionService = new TrainingSessionService(neuralmMQClient);
const userService: IUserService = new UserService(neuralmMQClient);
const userModule: IUserModule = new UserModule();
const appModule: IAppModule = new AppModule();
const trainingRoomModule: ITrainingRoomModule = new TrainingRoomModule();
const dashboardModule: IDashboardModule = new DashboardModule();
const trainingSessionModule: ITrainingSessionModule = new TrainingSessionModule();

const store = new Vuex.Store<IRootState>({
  modules: {
    user: userModule,
    app: appModule,
    trainingRoom: trainingRoomModule,
    dashboard: dashboardModule,
    trainingSession: trainingSessionModule
  }
});

const router = new NeuralmRouter();

router.setRoutes([
  { name: 'home' },
  { name: 'dashboard', props: { trainingRoomService } },
  { name: 'trainingroom', props: { trainingSessionService } },
  { name: 'trainingsession', props: { trainingSessionService } },
  { name: 'login', props: { userService } },
  { name: 'register', props: { userService } },
  { name: 'createtrainingroom', props: { trainingRoomService } },
  { name: 'about' }
], true);

new Vue({
  vuetify,
  snotify,
  router,
  store,
  render: (h: any) => h(App)
} as any).$mount('#app');
