import axios from "axios";
import AuthService from "./AuthService";
import  API_ENDPOINTS  from "../config/Config";

class RefreshTokenService{
    static async refreshAccessToken(){
        try
        {
          const refreshToken = AuthService.getAccessToken();
          if (!refreshToken) throw new Error("Refresh không tồn tại!");

          const { data } = await axios.post(API_ENDPOINTS.REFRESH_TOKEN, {
            refreshToken,
          });

          AuthService.setAccessToken(data.accessToken);
          AuthService.setRefreshToken(data.refreshToken);

          return data.accessToken;
        } 
        catch(error)
        {
            console.log("refresh token lỗi!",error);
            AuthService.cleanToken();
            window.location.href="/login";
            throw error;
        }
    }
}

export default RefreshTokenService