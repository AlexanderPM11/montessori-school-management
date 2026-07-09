
import { ApiResponse } from "../../interfaces/ApiResponse";
import { ChangePasswordInterface } from "../../interfaces/Auth/ChangePasswordInterface";
import { GenerateTokenRequest } from "../../interfaces/Auth/GenerateTokenRequest";
import { LoginInterface } from "../../interfaces/Auth/Login";
import { RefreshToken, RefreshTokenConvert } from "../../interfaces/Auth/RefreshToken";
import { User, UserConvert } from '../../interfaces/Auth/User';
import BaseApi from "../BaseApi";


class AuthService {

    static async RefreshTokenAsync(generateTokenRequest: GenerateTokenRequest): Promise<ApiResponse<RefreshToken>> {

        try {

            const response = await BaseApi.postAsync("/Account/RefreshToken", generateTokenRequest);
            const dataResponse = response.data;

            const refreshTokenData = RefreshTokenConvert.toRefreshToken(dataResponse.data);

            return {
                data: refreshTokenData,
                result: dataResponse.result,
                message: dataResponse.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as RefreshToken,
                result: false,
                message: "Error al refrescar el token."
            }
        }
    }

    static async LoginAsync(credentials: LoginInterface): Promise<ApiResponse<User>> {

        try {

            const response = await BaseApi.postAsync("/Account/Login", credentials);
            const dataResponse = response.data;

            const userData = UserConvert.toUser(dataResponse.data);

            return {
                data: userData,
                result: dataResponse.result,
                message: dataResponse.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: {} as User,
                result: false,
                message: "Error al iniciar sesión."
            }
        }
    }

    static async ForgotPasswordAsync(email: string): Promise<ApiResponse<string>> {

        try {

            const response = await BaseApi.postAsync("/Account/ForgotPassword", { email });
            const dataResponse = response.data;

            return {
                data: dataResponse.data,
                result: dataResponse.result,
                message: dataResponse.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: '',
                result: false,
                message: "Error al enviar el correo de recuperación."
            }
        }
    }

    static async ChangePasswordAsync(values: ChangePasswordInterface): Promise<ApiResponse<string>> {

        try {

            const response = await BaseApi.postAsync("/Account/ChangePassword", values);
            const dataResponse = response.data;

            return {
                data: dataResponse.data,
                result: dataResponse.result,
                message: dataResponse.messages[0]
            }

        } catch (error) {
            console.log(error)
            return {
                data: '',
                result: false,
                message: "Error al cambiar la contraseña."
            }
        }
    }

}

export default AuthService;



