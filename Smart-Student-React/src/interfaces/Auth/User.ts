

export interface User {
    id: string;
    userName: string;
    email: string;
    roles: string[];
    isVerified: boolean;
    firstName: string;
    lastName: string;
    addres: string;
    dateBirth: string;
    gender: number;
    password: string;
    confirmPassword: string;
    phoneNumber: string;
    estado: boolean;
    idUserCreator: string;
    institutionId: string;
    institutionIdPrincipal: string;
    urlImage: string;
    statu: boolean;
    tel: string;
    profession: string;
    occupation: string;
    job: string;
    placeWork: string;
    identificationId: string;
    idNivelEducativo: string;
    titleAchieved: string;
    studiesCurrentlyPursuing: string;
    civilStatus: string;
    nationality: string;
    yearsServiceEducationalSystem: string;
    yearServiceGrade: string;
    areaSpecialization: string;
    worksAnActivityDiferentThanteaching: boolean;
    specify: string;
    relationshipId: string;
    noBook: string;
    noFolio: string;
    token: string;
    refreshToken: string;
    file?: File;
}

// Converts JSON strings to/from your types
export class UserConvert {
    public static toUser(json: string | object): User {
        if (typeof json === "string") {
            return JSON.parse(json);
        }
        return json as User;
    }

    public static userToJson(value: User): string {
        return JSON.stringify(value);
    }
}
