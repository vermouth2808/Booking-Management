import axios from "axios";
import { API_BASE_URL } from "../config/Config";
import AuthService from "../services/AuthService"; 

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        "Content-Type": "application/json",
    }
});

// Tự động thêm access token vào request
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

// Tự động refresh token khi bị lỗi 401
// api.interceptors.response.use(
//     (response) => response,
//     async (error) => {
//         const originalRequest = error.config;

//         if (error.response?.status === 401 && !originalRequest._retry) {
//             originalRequest._retry = true;
//             try {
//                 const newAccessToken = await AuthService.refreshAccessToken();

//                 if (newAccessToken?.accessToken) {
//                     api.defaults.headers["Authorization"] = `Bearer ${newAccessToken.accessToken}`;
//                     return api(originalRequest);
//                 }
//             } catch (err) {
//                 return Promise.reject(err);
//             }
//         }
//         return Promise.reject(error);
//     }
// );

export default api;
