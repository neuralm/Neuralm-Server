import Response from './Response';
import TrainingRoom from '@/models/TrainingRoom';

/**
 * Represents paginate training room response class.
 */
export default class PaginateTrainingRoomResponse extends Response {
  public items: TrainingRoom[];
  public pageNumber: number;
  public pageSize: number;
  public totalRecords: number;
  public pageCount: number;

  /**
   * Initializes a new instance of the PaginateTrainingRoomResponse class.
   */
  constructor(id: string, requestId: string, dateTime: Date, message: string, success: boolean, items: TrainingRoom[], pageNumber: number, pageSize: number, totalRecords: number, pageCount: number) {
    super(id, requestId, dateTime, message, success);
    this.items = items;
    this.pageCount = pageCount;
    this.totalRecords = totalRecords;
    this.pageSize = pageSize;
    this.pageNumber = pageNumber;
  }
}
