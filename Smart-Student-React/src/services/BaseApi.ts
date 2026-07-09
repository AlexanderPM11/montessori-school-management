import { AxiosResponse, AxiosRequestConfig } from "axios";
import apiClient from "./ConfigGeneralAxios";


class BaseApi {

    // Método GET
    static async getAsync(url: string, params?: Record<string, unknown>): Promise<AxiosResponse> {
        const response = await apiClient.get(url, { params });
        return response;
    }

    // Método POST
    static async postAsync(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<AxiosResponse> {
        const response = await apiClient.post(url, data, config);
        return response;
    }

    // Método PUT
    static async putAsync(url: string, data?: unknown): Promise<AxiosResponse> {
        const response = await apiClient.put(url, data);
        return response.data;
    }

    // Método DELETE
    static async deleteAsync(url: string, params?: Record<string, unknown>): Promise<AxiosResponse> {
        const response = await apiClient.delete(url, { params });
        return response;
    }
}

export default BaseApi;



