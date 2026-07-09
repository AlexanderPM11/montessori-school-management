export interface ReportDataResponse {
    vmPrinc: VmPrinc[];
    evaluationsDetails: EvaluationsDetails[];
    studentViewModel: null;
    educationalInstitutionViewModel: null;
    nameTeacher: string | null;
    nameProvince: string | null;
    nameMunicipality: string | null;
    nextGradeDes: string | null;
    comment1: string | null;
    comment2: string | null;
    base64Preview: string | null;
    period: string | null;
    idStudent: number;
    subjectName: string | null;
    observationCommentEvaluationViewModel: null;
}
export class ReportDataResponseConvert {
    public static toRequestReport(json: string | object): ReportDataResponse {
        const data = typeof json === "string" ? JSON.parse(json) : json;

        return data as ReportDataResponse;
    }
}

export interface VmPrinc {
    indicator: Indicator;
    existsInEvaluationsPeriod: boolean;
    estado: string;
    periodo: string;
    calification: number;
    recuperaPedg: number;
    periodosEstados: null;
}
// NUEVA INTERFACE AGRUPADA
export interface EvaluationsDetails {
    id: string;
    title: string;
    subTitle: string;
    indicators: VmPrinc[];
}

export interface Indicator {
    description: string;
    associatedGrades: string;
    codeSubject: string | null;
    id: number;
    createdBy: string;
    created: string; // ISO date string
    lastModifiedBy: string;
    lastModified: string; // ISO date string
}
