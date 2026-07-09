import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import {
  showCustomLoading,
  closeCustomLoading,
} from "../../../util/showCustomLoading";
import { useReportDataStore } from "../../../hooks/store/Reports/Reports.store";
import { useGeneralStore } from "../../../Formik/Users/user";
import { EvaluacionHeader } from "../EvaluacionHeader";
import { EvaluationList } from "./EvaluationList";
import { previewBase64Pdf } from "../../../util/reports/previewPdfBase64";
import { downloadBase64Pdf } from "../../../util/reports/downloadBase64Pdf";
import { PeriodoAcciones } from "./PeriodoAcciones";
import { CommentSection } from "./CommentSection";

export const Evaluacion: React.FC = () => {
  const [searchParams] = useSearchParams();
  const idStudent = Number(searchParams.get("idStudent"));
  const idRoom = Number(searchParams.get("idRoom"));
  const studentname = searchParams.get("studentname");
  const studentlastname = searchParams.get("studentlastname");

  const [periodSelect, setPeriodo] = useState("1ro. Agosto - Diciembre");
  const GetDataReport = useReportDataStore((state) => state.GetDataReport);
  const GetReport = useReportDataStore((state) => state.GetReport);
  const getPeriods = useGeneralStore((state) => state.getPeriods);
  const periods = useGeneralStore((state) => state.periods);
  const isLoading = useReportDataStore((state) => state.loading);
  const reportData = useReportDataStore((state) => state.reportData);

  const fetchData = async (forceRefres = false, period?: string) => {
    try {
      const requestReport = {
        idStudent,
        idAchievementIndicator: 0,
        period: period ?? periodSelect,
        estado: "",
        idSubject: 0,
      };
      await getPeriods(false);
      await GetDataReport(requestReport, forceRefres);
    } catch (error) {
      console.error("Error fetching report data:", error);
    }
  };
  // GetReport
  const verPdfBase64 = async () => {
    showCustomLoading();
    const requestReport = {
      idStudent,
      idAchievementIndicator: 0,
      period: periodSelect,
      estado: "",
      idSubject: 0,
    };
    const dataResponse = await GetReport(requestReport);
    closeCustomLoading();

    previewBase64Pdf(dataResponse.data.base64Preview ?? "");
  };
  const downloadPdfBase64 = async () => {
    showCustomLoading();
    const requestReport = {
      idStudent,
      idAchievementIndicator: 0,
      period: periodSelect,
      estado: "",
      idSubject: 0,
      preview: false,
    };

    const dataResponse = await GetReport(requestReport);
    closeCustomLoading();

    const base64 = dataResponse.data.base64Preview;
    downloadBase64Pdf(base64 ?? "", `${studentname}_${periodSelect}.pdf`);
  };
  //

  useEffect(() => {
    fetchData();
    // eslint-disable-next-line
  }, []);

  if (isLoading) showCustomLoading();
  else closeCustomLoading();

  const displayContent =
    !isLoading && reportData?.evaluationsDetails?.length > 0;

  return (
    <div className="max-w-[1200px] mb-16">
      <EvaluacionHeader idRoom={idRoom} />
      {displayContent && (
        <div className=" mx-auto mt-6 border rounded-lg p-4 shadow-lg bg-white">
          <h2 className="text-xl font-semibold mb-2">
            {studentname} ({studentlastname})
          </h2>

          <PeriodoAcciones
            periodSelect={periodSelect}
            periods={periods}
            onPeriodChange={(next) => {
              setPeriodo(next);
              fetchData(true, next);
            }}
            onPreview={verPdfBase64}
            onDownload={downloadPdfBase64}
          />

          <EvaluationList period={periodSelect} idStudent={idStudent} />

          <CommentSection period={periodSelect} idStudent={idStudent} />
        </div>
      )}
    </div>
  );
};
