import { IMessage } from '@/interfaces/IMessage';

export abstract class MessageHandler {
  public callback: MessageCallBack;
  public messageName: string;

  constructor(messageName: string, resolve: any, reject: any) {
    this.messageName = messageName;
    this.callback = (message: IMessage) => {
      if (!message) {
        reject(message);
      }
      resolve(message);
    };
  }
}

export type MessageCallBack = (data: IMessage) => void;
export type MessageHandlerDestructor = (data: MessageHandler) => void;
