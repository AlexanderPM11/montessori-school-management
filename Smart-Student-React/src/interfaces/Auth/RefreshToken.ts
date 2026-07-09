export interface RefreshToken {
    token: string;
    refreshToken: string
}

export class RefreshTokenConvert {
    public static toRefreshToken(json: string): RefreshToken {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as RefreshToken;
    }
}