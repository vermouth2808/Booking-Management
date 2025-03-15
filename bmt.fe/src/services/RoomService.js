import api from "../api/api";
import API_ENDPOINTS from "../config/Config";

const RoomService = {
    async getDetailRoom(RoomId) {
        try {
            const res = await api.get(`${API_ENDPOINTS.GET_BY_ID_ROOM}/${RoomId}`);
            return res.data ? res.data : null;
        } catch (error) {
            console.log("failed read by id room", error);
            return null;
        }
    }
};

export default RoomService;

