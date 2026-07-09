export interface EducationalInstitutionViewModel {
    id?: string;
    name: string;
    address: string;
    nameMunicipality: string;
    phone?: string;
    mobile: string;
    idRector: string;
    idCordinator: string;
    idProvinceDom?: number;
    idDepartment?: number;
    idSecretary?: string;
    academicResolution?: string;
    educationalRegistry?: string;
    website?: string;
    urlLogo?: string;
    isMainSchool: boolean;
    idUser?: string;
    userAssignmentId: string;
    session: string;
    regional: string;
    district: string;

    nameRector?: string;
    nameCordinator?: string;
    nameSecretary?: string;
    nameAdmin?: string;
    base64Img?: string;

    file?: File;

    // Propiedades auditables heredadas de AuditableBaseModel
    createdAt?: string;
    updatedAt?: string;
    createdBy?: string;
    updatedBy?: string;
}
export class EducationalInstituConvert {


    public static toEducationalInstitu(json: string | object): EducationalInstitutionViewModel {
        const educationalInstitu = typeof json === "string" ? JSON.parse(json) : json;

        return educationalInstitu;
    }
}


