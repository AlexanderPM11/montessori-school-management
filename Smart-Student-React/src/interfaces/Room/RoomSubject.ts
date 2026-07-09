export interface RoomSubject {
    id: number;
    idTeacher: string;
    idSuject: number;
    idRoom: number | null;
    nameTeacher: string;
    nameGrade: string;
}
// Converts JSON strings to/from your types
export class RoomSubjectConvert {


    public static toRoomSubject(json: string | object): RoomSubject {
        const roomSubject = typeof json === "string" ? JSON.parse(json) : json;

        return roomSubject;
    }


    public static RoomSubjectToJson(value: RoomSubject[]): string {
        return JSON.stringify(value);
    }
}