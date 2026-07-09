export interface RequestTeacherSubject {
    idTeacher: string;
    idRoom: number;
}

// Converts JSON strings to/from your types
export class RequestTeacherSubjectConvert {
    public static toRequestTeacherSubject(json: string | object): RequestTeacherSubject {
        const data = typeof json === "string" ? JSON.parse(json) : json;
        return data;
    }
}
