import NeuralmRouter from '@/plugins/vuerouter';
import Vuex from '@/plugins/vuex';
import LoginView from '@/views/Login.vue';
import { Wrapper, mount } from '@vue/test-utils';
import IUserService from '@/interfaces/IUserService';
import UserService from '@/services/UserService';
import { IRootState } from '@/interfaces/IRootState';
import UserModule, { IUserModule } from '@/modules/User.module';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import { IMessage } from '@/interfaces/IMessage';
import AuthenticateRequest from '@/messages/requests/AuthenticateRequest';
import MessageWrapper from '@/messaging/MessageWrapper';
import messageTypeCache from '@/messaging/MessageTypeCache';
import AuthenticateResponse from '@/messages/responses/AuthenticateResponse';
import Guid from '@/helpers/Guid';
import { flush, registerPlugins, createMockedNeuralmMQClient } from '../../tests/TestUtility';

jest.useFakeTimers();
registerPlugins();

describe('Login.vue', () => {
  let wrapper: Wrapper<LoginView>;
  let messages: IMessage[];
  let messageProcessor: IMessageProcessor;
  let userModule: IUserModule;

  beforeEach(async () => {
    localStorage.clear();
    messages = [];
    const { processor, mq } = createMockedNeuralmMQClient(messages);
    messageProcessor = processor;
    const userService: IUserService = new UserService(mq);
    userModule = new UserModule();
    const store = new Vuex.Store<IRootState>({
      modules: {
        user: userModule
      }
    });
    const router = new NeuralmRouter([
      { name: 'home' },
      { name: 'login', props: { userService } },
      { name: 'register', props: { userService } }
    ], true);
    wrapper = mount(LoginView,
    {
      store,
      router,
      propsData: {
        userService
      },
      sync: false
    });
    await flush();
  });

  it('login button should not be enabled because username and password are not set.', () => {
    expect(wrapper.find('button').attributes('disabled')).toBe('disabled');
  });

  it('login button should be enabled because username and password are set.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    expect(wrapper.find('button').attributes('disabled')).toBeUndefined();
  });

  it('login button click should initiate an AuthenticateRequest.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
    expect(messages[0]).toBeInstanceOf(AuthenticateRequest);
  });

  it('login button click should initiate a successful authenticate response setting status.loggedIn to true.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
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
    await flush();
    expect(userModule.state!.status.loggedIn).toBe(true);
  });

  it('after a successful login the user should be redirected to home', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
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
    await flush();
    expect(wrapper.vm.$route.name).toBe('home');
  });

  it('is able to navigate to register page', async () => {
    wrapper.vm.$router.push('/register');
    await flush();
    expect(wrapper.vm.$route.name).toBe('register');
  });

  it('is not able to navigate to dashboard page', async () => {
    wrapper.vm.$router.push('/dashboard');
    await flush();
    expect(wrapper.vm.$route.name).toBe('login');
  });
});