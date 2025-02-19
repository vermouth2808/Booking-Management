export const API_BASE_URL = "https://localhost:44331/api";
const API_ENDPOINTS = {
    LOGIN: `${API_BASE_URL}/Auth/login`,
    REGISTER: `${API_BASE_URL}/Auth/registerClient`,
    REFRESH_TOKEN: `${API_BASE_URL}/Auth/refresh-token`,
    IMAGE_URL: `${API_BASE_URL}/Banners/GetById/`
};

export default API_ENDPOINTS;