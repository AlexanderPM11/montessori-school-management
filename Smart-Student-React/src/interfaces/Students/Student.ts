
export interface Student {
    code: null;
    id: number;
    name: string;
    lastname: string;
    sexo: string;
    age: number;
    direction?: string;
    telFather: string;
    tel?: string;
    telMother: string;
    estado: string;
    bornDate: string;
    idInstitu: number;
    idRoom?: string;
    idTypeRegister: number;
    idGrade: number;
    book?: string;
    folio?: string;
    relationPersonLiveWith: string;
    carriedPreprimary: boolean;
    neae?: string;
    diseasesAllergic?: string;
    medicinesUse?: string;
    emergencyPerson?: string;
    emergencyTel?: string;
    idFather: string;
    idMother: string;
    idNacionality: number;
    numberSiblings?: string;
    placeBetweenSiblings?: string;
    doctorPediatrician?: string;
    urlImg?: string;
    relationPersonLiveWithDesc: string;
    idFatherDesc: string;
    idMotherDesc: string;
    sexDes: string;
    gradeDes: string;
    nacionality: string;
    agesSiblings?: string;
    level: string;
    numberList: number;
    adjuntos: unknown[];
    file?: File;
}

// Converts JSON strings to/from your types
export class StudentConvert {


    public static toStudent(json: string | object): Student {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as Student;
    }

    public static studentToJson(value: Student[]): string {
        return JSON.stringify(value);
    }
}
