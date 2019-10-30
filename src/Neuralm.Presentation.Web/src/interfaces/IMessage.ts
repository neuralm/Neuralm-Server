export interface IMessage {
  id: string;
}

export interface IResponse extends IMessage {
  success: boolean;
}
