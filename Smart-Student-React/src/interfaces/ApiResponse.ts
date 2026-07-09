export interface ApiResponse<T> {
    data: T;
    result: boolean;
    message?: string;
}