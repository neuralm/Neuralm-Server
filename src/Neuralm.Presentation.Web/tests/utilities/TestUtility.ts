import flushPromises from 'flush-promises';
import Vuetify from 'vuetify';
import Snotify, { SnotifyToast } from 'vue-snotify';
import Router from 'vue-router';
import Vue from 'vue';
import MessageProcessor from '@/messaging/MessageProcessor';
import { IMessageProcessor } from '@/interfaces/IMessageProcessor';
import INeuralmMQClient from '@/interfaces/INeuralmMQClient';
import NeuralmMQClient from '@/messaging/NeuralmMQClient';
import { IMessage } from '@/interfaces/IMessage';
import { INetworkConnector } from '@/interfaces/INetworkConnector';
import ErrorResponseHandler from '@/handlers/ErrorResponseHandler';
import '@/plugins/vee-validate';

/**
 * Flushes the promises and makes sures that all times are ran.
 * @returns Returns an empty promise.
 */
async function flush(): Promise<void> {
  await flushPromises();
  jest.runAllTimers();
}

/**
 * Registers the default plugins used with neuralm front-end.
 */
function registerPlugins(): void {
  Vue.use(Vuetify);
  Vue.use(Snotify);
  Vue.use(Router);
}

/**
 * Creates a mocked neuralm mq client with
 * a backing array to store outgoing messages in.
 * @param messages The array to store outgoing messages in.
 * @returns Returns the message processor and the mocked neuralm mq client.
 */
function createMockedNeuralmMQClient(messages: IMessage[]): {
  processor: IMessageProcessor,
  mq: INeuralmMQClient
} {
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
  const messageProcessor: IMessageProcessor = new MessageProcessor();
  const neuralmMQClient: INeuralmMQClient = new NeuralmMQClient(messageProcessor, networkConnectorMock);
  const errorResponseHandler: ErrorResponseHandler = new ErrorResponseHandler(messageProcessor);
  neuralmMQClient.addHandler(errorResponseHandler);
  return { processor: messageProcessor, mq: neuralmMQClient };
}

/**
 * Gets the last notification in the Snotify notifications array.
 * @param vm The vue instance.
 * @returns Returns the last notification.
 */
function getLastSnotifyNotification(vm: Vue): SnotifyToast {
  const lastNotification = vm.$snotify.notifications.length;
  if (lastNotification === 0) {
    throw new Error('Snotify does not have any notifications.');
  }
  return vm.$snotify.notifications[lastNotification - 1];
}

/**
 * Checks if the notifications contains a certain body.
 * @param vm The vue instance.
 * @param body The body of the notification.
 * @returns Returns whether the notifications contains a certain body.
 */
function snotifyNotificationsContains(vm: Vue, body: string): boolean {
  return vm.$snotify.notifications.filter((not) => not.body === body).length > 0;
}

export { flush, registerPlugins, createMockedNeuralmMQClient, getLastSnotifyNotification, snotifyNotificationsContains };
