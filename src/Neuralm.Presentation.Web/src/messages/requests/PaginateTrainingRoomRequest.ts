import Request from './Request';

/**
 * Represents the PaginateTrainingRoomRequest class.
 */
export default class PaginateTrainingRoomRequest extends Request {
  public pageNumber: number;
  public pageSize: number;

  /**
   * Initializes the PaginateTrainingRoomRequest class.
   * @param pageNumber The page number.
   * @param pageSize The page size.
   */
  constructor(pageNumber: number, pageSize: number) {
    super();
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
  }
}
