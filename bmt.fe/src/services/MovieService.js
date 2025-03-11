import api from "../api/api"
import API_ENDPOINTS, { API_BASE_URL } from "../config/Config"

const MovieService = {
    async searchMovies() {
        try {
            const payload = {
                keySearch: "",
                pageSize: 10,
                pageIndex: 0
            }
            const res = await api.post(API_ENDPOINTS.SEARCH_MOVIES, payload);
            return Array.isArray(res.data.movies) ? res.data.movies : [];
        } catch (error) {
            console.error("Failed to fetch movies", error);
            return [];
        }
    },
    async movieDetail(id) {  
        if (!id) {
            console.error("Movie ID is undefined!");
            return null;
        }

        try {
            const res = await api.get(`${API_ENDPOINTS.GET_BY_ID_MOVIE}/${id}`);
            return res.data;
        } catch (error) {
            console.error("Failed to fetch movie", error);
            return null;
        }
    }
    
}

export default MovieService;