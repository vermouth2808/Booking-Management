import api from "../api/api"
import API_ENDPOINTS from "../config/Config"

const ImageService ={
async getBackgroundImage(){
    try {
        const res = await api.get(API_ENDPOINTS.IMAGE_URL+"1")
        return res.data;
    } catch (error) {
        console.error("Failed to fetch image URL", error);
            return "";
    }
}
}

export default ImageService;