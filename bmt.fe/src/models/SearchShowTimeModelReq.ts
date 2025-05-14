import {SearchReq } from "./SearchReq";

export interface SearchShowTimeModelReq extends SearchReq {
  fromDate: Date;
  toDate: Date;
  movieId: number;
  roomId?: number;
}