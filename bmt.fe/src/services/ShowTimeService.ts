import api from "../api/api"
import API_ENDPOINTS, { API_BASE_URL } from "../config/Config"
import { SearchShowTimeModelReq } from "../models/SearchShowTimeModelReq";
import { ShowTime, ShowTimeSearchModelRes } from "../models/ShowTimeSearchModelRes";
const ShowTimeService = {
    async SearchShowTime(params: SearchShowTimeModelReq | undefined ) : Promise<ShowTime[]|undefined> {
        try {
          const now = new Date();
          // Ngày đầu tiên của tháng hiện tại
          const firstDayOfMonth = new Date(
            now.getFullYear(),
            now.getMonth(),
            1
          ).toISOString();

          // Ngày cuối cùng của tháng hiện tại
          const lastDayOfMonth = new Date(
            now.getFullYear(),
            now.getMonth() + 1,
            0
          ).toISOString();

          const payload = {
            keySearch: params?.keySearch || "",
            pageSize: params?.pageSize || 10,
            pageIndex: params?.pageIndex || 1,
            fromDate: params?.fromDate || firstDayOfMonth,
            toDate: params?.toDate || lastDayOfMonth,
            movieId: params?.movieId || null,
            roomId: null,
          };
          const res = await api.post(API_ENDPOINTS.SEARCH_SHOWTIME, payload);
          return res.data.showTimes;
        } catch (error) {
            console.log("Failed to fetch showtimes", error);
        }
    },
    async GetDetailShowTime(id:number) {
        try {
            const res = await api.get(`${API_ENDPOINTS.GET_BY_ID_SHOWTIME}/${id}`);
            console.log("data show time :" ,res.data);
            return res.data ? res.data : null;
        } catch (error) {
            console.log("Failed to fetch showtimes", error);
        }
    }
};
export default ShowTimeService;