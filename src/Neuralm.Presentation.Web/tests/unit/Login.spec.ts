import NeuralmRouter from '@/plugins/vuerouter';
import Vuex from '@/plugins/vuex';
import LoginView from '@/views/Login.vue';
import { shallowMount, Wrapper } from '@vue/test-utils';
import IUserService from '@/interfaces/IUserService';
import UserService from '@/services/UserService';
import { IRootState } from '@/interfaces/IRootState';
import UserModule, { IUserModule } from '@/modules/User.module';
import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import MessageProcessor from '@/messaging/MessageProcessor';
import NeuralmMQClient from '@/messaging/NeuralmMQClient';
import { INetworkConnector } from '@/interfaces/INetworkConnector';
import { IMessage } from '@/interfaces/IMessage';
import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import MessageWrapper from '@/messaging/MessageWrapper';
import messageTypeCache from '@/messaging/MessageTypeCache';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import Guid from '@/helpers/Guid';
import ErrorResponseHandler from '@/handlers/ErrorResponseHandler';
import Vuetify from 'vuetify';
import Snotify from 'vue-snotify';
import Vue from 'vue';
import flushPromises from 'flush-promises';

describe('Login.vue', () => {
  let wrapper: Wrapper<LoginView>;
  let messages: IMessage[];
  let messageProcessor: IMessageProcessor;
  let userModule: IUserModule;
  const networkConnectorMock: INetworkConnector = {
    isConnected: true,
    isRunning: true,
    connectAsync(): Promise<boolean> {
      this.isConnected = true;
      return Promise.resolve(true);
    },
    sendMessage<TMessage extends IMessage>(message: TMessage): void {
      messages.push(message);
    },
    start() {
      this.isRunning = true;
    },
    stop() {
      this.isConnected = false;
      this.isRunning = false;
    }
  };

  beforeEach(() => {
    localStorage.clear();
    messages = [];
    messageProcessor = new MessageProcessor();
    userModule = new UserModule();

    const neuralmMQClient: INeuralmMQClient = new NeuralmMQClient(messageProcessor, networkConnectorMock);
    const errorResponseHandler: ErrorResponseHandler = new ErrorResponseHandler(messageProcessor);
    neuralmMQClient.addHandler(errorResponseHandler);
    const userService: IUserService = new UserService(neuralmMQClient);
    const store = new Vuex.Store<IRootState>({
      modules: {
        user: userModule
      }
    });
    const router = new NeuralmRouter();
    router.setRoutes([
      { name: 'home' },
      { name: 'login', props: { userService } },
      { name: 'register', props: { userService } }
    ], true);

    Vue.use(Vuetify);
    Vue.use(Snotify);
    wrapper = shallowMount(LoginView,
    {
      store,
      router,
      propsData: {
        userService
      }
    });
  });

  it('login button should not be enabled because username and password are not set.', () => {
    expect(wrapper.find('button').attributes('disabled')).toBe('disabled');
  });

  it('login button should be enabled because username and password are set.', () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    expect(wrapper.find('button').attributes('disabled')).toBeUndefined();
  });

  it('login button click should initiate an AuthenticateRequest.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    wrapper.find('button').trigger('submit');
    await flushPromises();
    expect(messages[0]).toBeInstanceOf(AuthenticateRequest);
  });

  it('login button click should initiate a successful authenticate response setting status.loggedIn to true.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    wrapper.find('button').trigger('submit');
    const response: AuthenticateResponse = new AuthenticateResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'empty message',
      true,
      Guid.newGuid().toString(),
      'access token'
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('AuthenticateResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flushPromises();
    expect(userModule.state!.status.loggedIn).toBe(true);
  });

  it('after a successful login the user should be redirected to home', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    wrapper.find('button').trigger('submit');
    const response: AuthenticateResponse = new AuthenticateResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'empty message',
      true,
      Guid.newGuid().toString(),
      'access token'
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('AuthenticateResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flushPromises();
    expect(wrapper.vm.$route.name).toBe('home');
  });

  it('is able to navigate to register page', async () => {
    wrapper.vm.$router.push('/register');
    await flushPromises();
    expect(wrapper.vm.$route.name).toBe('register');
  });
});
