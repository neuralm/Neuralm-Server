import { IMessage, IResponse } from '@/interfaces/IMessage';

export default interface MessageWrapper {
  name: string;
  message: IResponse;
}
