import api from "../api/api"
import API_ENDPOINTS from "../config/Config"

const MovieService = {
    async getAllCategory() {
        try {
            const res = await api.get(API_ENDPOINTS.READ_ALL_CATEGORY);
            return Array.isArray(res.data) ? res.data : [];
        } catch (error) {
            console.error("Failed to fetch categories", error);
            return [];
        }
    }
}

export default MovieService;