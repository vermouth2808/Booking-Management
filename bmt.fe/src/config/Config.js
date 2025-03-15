export const API_BASE_URL = "https://localhost:44331/api";
const API_ENDPOINTS = {
    LOGIN: `${API_BASE_URL}/Auth/login`,
    REGISTER: `${API_BASE_URL}/Auth/registerClient`,
    REFRESH_TOKEN: `${API_BASE_URL}/Auth/refresh-token`,
    IMAGE_URL: `${API_BASE_URL}/Banners/GetById/`,
    //////////MOVIE//////////
    SEARCH_MOVIES: `${API_BASE_URL}/Movies/SearchMovie`,
    GET_BY_ID_MOVIE: `${API_BASE_URL}/Movies/GetById`,
    /////////CATEGORIES//////
    READ_ALL_CATEGORY: `${API_BASE_URL}/Categorys/ReadAll`,
    CREATE_CATEGORY: `${API_BASE_URL}/Categorys/Create`,
    UPDATE_CATEGORY: `${API_BASE_URL}/Categorys/Update`,
    GET_BY_ID_CATEGORY: `${API_BASE_URL}/Categorys/GetById/`,
    DELETE_CATEGORY: `${API_BASE_URL}/Categorys/Delete/`,
    ////////SHOWTIME////////////////
    SEARCH_SHOWTIME : `${API_BASE_URL}/ShowTimes/SearchShowTime`,
    GET_BY_ID_SHOWTIME : `${API_BASE_URL}/ShowTimes/GetById`,

    ///////ROOM///////////
    GET_BY_ID_ROOM : `${API_BASE_URL}/Rooms/GetById`,
};

export default API_ENDPOINTS;