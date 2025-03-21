import api from "../api/api"
import API_ENDPOINTS from "../config/Config"
import { MovieModelRes } from "../models/MovieModelRes";
import { searchMovie } from "../models/searchMovie";

const MovieService = {
  async searchMovies(
    params: searchMovie | undefined
  ): Promise<MovieModelRes | undefined> {
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
      };

      const res = await api.post(API_ENDPOINTS.SEARCH_MOVIES, payload);
      return res.data;
    } catch (error) {
      console.error("Failed to fetch movies", error);
      return undefined;
    }
  },
  async movieDetail(id: number) {
    if (!id) {
      console.error("Movie ID is undefined!");
      return null;
    }

    try {
      const res = await api.get(`${API_ENDPOINTS.GET_BY_ID_MOVIE}/${id}`);
      console.log("data", res.data);
      return res.data;
    } catch (error) {
      console.error("Failed to fetch movie", error);
      return null;
    }
  },
};

export default MovieService;