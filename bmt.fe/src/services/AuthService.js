class AuthService {
    static getAccessToken() {
        return sessionStorage.getItem("accessToken");
    }

    static setAccessToken(token) {
        sessionStorage.setItem("accessToken", token);
    }

    static getRefreshToken() {
        return sessionStorage.getItem("refreshToken");
    }

    static setRefreshToken(token) {
        sessionStorage.setItem("refreshToken", token);
    }

    static cleanToken() {
        sessionStorage.removeItem("accessToken");
        sessionStorage.removeItem("refreshToken");
    }
}

export default AuthService