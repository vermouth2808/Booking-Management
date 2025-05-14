import { SearchReq } from "./SearchReq";

export interface searchMovie extends SearchReq {
  fromDate: Date;
  toDate: Date;
}
