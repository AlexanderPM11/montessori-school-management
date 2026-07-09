

export interface Subject {
    id: number;
    name: string;
    description: string;
    code: string;
    imageUrl: string | null;

    file: File;
}

// Converts JSON strings to/from your types
export class SubjectConvert {

    public static toAjunto(json: string | object): Subject {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as Subject;
    }
    public static ajuntoToJson(value: Subject[]): string {
        return JSON.stringify(value);
    }
}
