export interface SaveCommentsRequest {
  idStudent: number;
  comment1: string;
  comment2: string;
  period: string;
}

// Converts JSON strings to/from your types
export class SaveCommentsRequestConvert {
  public static toSaveCommentsRequest(
    json: string | object
  ): SaveCommentsRequest {
    const data = typeof json === "string" ? JSON.parse(json) : json;
    return data;
  }
}
