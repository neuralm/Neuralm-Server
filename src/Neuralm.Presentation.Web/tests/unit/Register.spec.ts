import { Wrapper, mount } from '@vue/test-utils';
import { IMessage } from '@/interfaces/IMessage';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import UserModule, { IUserModule } from '@/modules/User.module';
import RegisterView from '@/views/Register.vue';
import { createMockedNeuralmMQClient, flush, registerPlugins, getLastSnotifyNotification } from '../utilities/TestUtility';
import UserService from '@/services/UserService';
import IUserService from '@/interfaces/IUserService';
import Vuex from '@/plugins/vuex';
import { IRootState } from '@/interfaces/IRootState';
import NeuralmRouter from '@/plugins/vuerouter';
import RegisterRequest from '@/messages/requests/RegisterRequest';
import RegisterResponse from '@/messages/responses/RegisterResponse';
import Guid from '@/helpers/Guid';
import MessageWrapper from '@/messaging/MessageWrapper';
import messageTypeCache from '@/messaging/MessageTypeCache';

jest.useFakeTimers();
registerPlugins();

describe('Register.vue', () => {
  let wrapper: Wrapper<RegisterView>;
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
    wrapper = mount(RegisterView,
    {
      store,
      router,
      propsData: {
        userService
      },
      sync: false
    });
    wrapper.vm.$router.push('/register');
    wrapper.vm.$snotify.clear();
    await flush();
  });

  it('register button should not be enabled because username and password are not set.', () => {
    expect(wrapper.find('button').attributes('disabled')).toBe('disabled');
  });

  it('register button click should initiate a RegisterRequest.', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
    expect(messages[0]).toBeInstanceOf(RegisterRequest);
  });

  it('register button click should initiate a successful register response and redirect to the login page', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
    const response: RegisterResponse = new RegisterResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'empty message',
      true
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('RegisterResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flush();
    expect(wrapper.vm.$route.name).toBe('login');
  });

  it('after a successful register response the user should receive a successful registration notification', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
    const response: RegisterResponse = new RegisterResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'Registration successful!',
      true
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('RegisterResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flush();
    expect(getLastSnotifyNotification(wrapper.vm).body).toBe('Registration successful!');
  });

  it('after an unsuccessful register response the user should receive a failed registration notification', async () => {
    wrapper.vm.$data.username = 'mario';
    wrapper.vm.$data.password = 'password';
    await flush();
    wrapper.find('button').trigger('submit');
    await flush();
    const response: RegisterResponse = new RegisterResponse(
      Guid.newGuid().toString(),
      messages[0].id,
      new Date(),
      'Registration failed!',
      false
    );
    const messageWrapper: MessageWrapper = {
      name: messageTypeCache.getMessageType('RegisterResponse'),
      message: response
    };
    messageProcessor.processMessage(messageWrapper);
    await flush();
    expect(getLastSnotifyNotification(wrapper.vm).body).toBe('Registration failed!');
  });
});
