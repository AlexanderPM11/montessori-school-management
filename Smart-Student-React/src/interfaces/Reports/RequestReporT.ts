export interface RequestReport {
    idStudent: number;
    idAchievementIndicator: number;
    score?: number;
    idSubject: number;
    period: string;
    estado: string;
    preview?: boolean;
    isRp?: boolean;
    rp?: boolean;
}

// Converts JSON strings to/from your types
export class RequestReportConvert {
    public static toRequestReport(json: string | object): RequestReport {
        const data = typeof json === "string" ? JSON.parse(json) : json;
        return data;
    }
}
