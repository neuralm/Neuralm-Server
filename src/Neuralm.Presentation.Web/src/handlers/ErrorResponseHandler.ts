import { MessageHandler } from '@/messaging/MessageHandler';
import ErrorResponse from '../messages/responses/ErrorResponse';
import { IMessageProcessor } from '../interfaces/IMessageProcessor';
import MessageWrapper from '../messaging/MessageWrapper';

/**
 * Represents the ErrorResponseHandler class.
 */
export default class ErrorResponseHandler extends MessageHandler {
  /**
   * Initializes a new instance of the ErrorResponseHandler class.
   */
  constructor(messageProcessor: IMessageProcessor) {
    const handleError = (data: ErrorResponse): void => {
      const message: MessageWrapper = { name: data.responseName, message: { success: false, id: data.requestId }};
      console.log(message);
      messageProcessor.processMessage(message);
    };
    super('Neuralm.Services.Common.Messages.ErrorResponse', handleError, handleError);
  }
}
