export interface Room {
    id: number;
    name: string;
    capacity: number;
    description: string;
    location: string;
    imageUrl: string;
    idEducationalInsti: number;
    idTeacherLead: string;
    idTypeRegisters: string;
    idTypeRegistersBack: string;
    teacherFullName: string;
    level: string;
    levelBack: string;
    createdBy: string | null;
    created: string;
    lastModifiedBy: string | null;
    lastModified: string;

    file?: File
    idTypeRegistersList?: string[];
}
// Converts JSON strings to/from your types
export class RoomConvert {


    public static toRoom(json: string | object): Room {
        const data = typeof json === "string" ? JSON.parse(json) : json;

        // Clonar y convertir propiedad idTypeRegisters
        const room: Room = {
            ...data,
            levelBack: data.idTypeRegisters.toString(),
            idTypeRegistersList: typeof data.idTypeRegisters === "string"
                ? data.idTypeRegisters.split(",").map((x: string) => x.trim())
                : [],
        };

        return room;
    }


    public static roomToJson(value: Room[]): string {
        return JSON.stringify(value);
    }
}