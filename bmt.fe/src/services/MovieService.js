import api from "../api/api"
import API_ENDPOINTS from "../config/Config"

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
    }
}

export default MovieService;