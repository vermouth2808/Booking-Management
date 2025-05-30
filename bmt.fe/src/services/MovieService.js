import api from "../api/api"
import API_ENDPOINTS, { API_BASE_URL } from "../config/Config"

const MovieService = {
    async searchMovies(params = {}) {
        try {
            const now = new Date();
    
            // Ngày đầu tiên của tháng hiện tại
            const firstDayOfMonth = new Date(now.getFullYear(), now.getMonth(), 1).toISOString();
            
            // Ngày cuối cùng của tháng hiện tại
            const lastDayOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0).toISOString();
    
            // Giá trị mặc định nếu params không có
            const payload = {
                keySearch: params.keySearch || '',
                pageSize: params.pageSize || 10,
                pageIndex: params.pageIndex || 1,
                fromDate: params.fromDate || firstDayOfMonth,
                toDate: params.toDate || lastDayOfMonth,
            };
    
            const res = await api.post(API_ENDPOINTS.SEARCH_MOVIES, payload);
            return Array.isArray(res.data.movies) ? res.data.movies : [];
        } catch (error) {
            console.error("Failed to fetch movies", error);
            return [];
        }
    }
    ,
    async movieDetail(id) {  
        if (!id) {
            console.error("Movie ID is undefined!");
            return null;
        }

        try {
            const res = await api.get(`${API_ENDPOINTS.GET_BY_ID_MOVIE}/${id}`);
            console.log("data",res.data);
            return res.data;
        } catch (error) {
            console.error("Failed to fetch movie", error);
            return null;
        }
    }
    
}

export default MovieService;