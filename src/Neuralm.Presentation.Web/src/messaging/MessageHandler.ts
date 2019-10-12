import { IMessage } from '@/interfaces/IMessage';

export abstract class MessageHandler {
  public abstract callback: MessageCallBack;
  public messageName!: string;

  constructor(messageName: string) {
    this.messageName = messageName;
  }
}

export type MessageCallBack = (data: IMessage) => void;
export type MessageHandlerDestructor = (data: MessageHandler) => void;