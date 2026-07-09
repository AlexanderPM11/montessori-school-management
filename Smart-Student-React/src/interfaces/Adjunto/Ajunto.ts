

export interface Adjunto {
    id: number;
    name: string;
    description: string;
    path: string;
    base64: string | null;
    bytesAdjunto: number;
    idStudent: number;
    idTipoAdjunto: number;
    file: File;
}

// Converts JSON strings to/from your types
export class AdjuntoConvert {

    public static toAjunto(json: string | object): Adjunto {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as Adjunto;
    }
    public static ajuntoToJson(value: Adjunto[]): string {
        return JSON.stringify(value);
    }
}
