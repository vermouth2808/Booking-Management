import axios from "axios";
import { API_BASE_URL } from "../config/Config"
import AuthService from "../services/AuthService";
import RefreshTokenService from '../services/AuthService';



const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    }
});

//auto add access token 
api.interceptors.request.use(
    (config) => {
        const token = AuthService.getAccessToken();
        if (token) {
            config.headers["Authorization"] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

//auto refresh token 
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {
                const newAccessToken = await RefreshTokenService.refreshAccessToken();
                api.defaults.headers["Authorization"] = `Bearer ${newAccessToken}`;
                return api(originalRequest);
            } catch (err) {
                return Promise.reject(err);
            }
        }
        return Promise.reject(error);
    }
);

export default api;