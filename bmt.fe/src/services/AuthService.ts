const AuthService = {
  getAccessToken() {
    return sessionStorage.getItem("accessToken");
  },
  setAccessToken(token: string) {
    sessionStorage.setItem("accessToken", token);
  },
  getRefreshToken() {
    return sessionStorage.getItem("refreshToken");
  },
  setRefreshToken(token: string) {
    sessionStorage.setItem("refreshToken", token);
  },
  cleanToken() {
    sessionStorage.removeItem("accessToken");
    sessionStorage.removeItem("refreshToken");
  },
  refreshAccessToken() {
    sessionStorage.get("refreshToken");
  },
};

export default AuthService;