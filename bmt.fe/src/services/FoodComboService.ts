import api from "../api/api"
import API_ENDPOINTS from "../config/Config"

const FoodComboService = {
    async getAllFoodCombo() {
        try {
            const res = await api.get(API_ENDPOINTS.READ_ALL_FOODCOMBO);
            return Array.isArray(res.data) ? res.data : [];
        } catch (error) {
            console.error("Failed to fetch categories", error);
            return [];
        }
    }
}

export default FoodComboService;