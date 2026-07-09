import React, { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import {
  showCustomLoading,
  closeCustomLoading,
} from "../../../util/showCustomLoading";
import { useReportDataStore } from "../../../hooks/store/Reports/Reports.store";
import { useGeneralStore } from "../../../Formik/Users/user";
import { previewBase64Pdf } from "../../../util/reports/previewPdfBase64";
import { downloadBase64Pdf } from "../../../util/reports/downloadBase64Pdf";
import { PeriodoAcciones } from "./PeriodoAcciones";
import { EvaluationList } from "./EvaluationList";
import { useAuthStore } from "../../../hooks/store/Auth.store";
import { EvaluacionHeader } from "../EvaluacionHeader";

export const Evaluacion: React.FC = () => {
  const [searchParams] = useSearchParams();
  const idStudent = Number(searchParams.get("idStudent"));
  const idRoom = Number(searchParams.get("idRoom"));
  const studentname = searchParams.get("studentname");
  const studentlastname = searchParams.get("studentlastname");

  const [periodSelect, setPeriodo] = useState("Agosto - Octubre");
  const GetDataReport = useReportDataStore((state) => state.GetDataReport);
  const GetReport = useReportDataStore((state) => state.GetReport);
  const GetSubjectsByTeacher = useReportDataStore(
    (state) => state.GetSubjectsByTeacher
  );
  const setSubjectSelect = useReportDataStore(
    (state) => state.setSubjectSelect
  );
  const getPeriods = useGeneralStore((state) => state.getPeriods);

  const periods = useGeneralStore((state) => state.periods);
  const isLoading = useReportDataStore((state) => state.loading);
  const reportData = useReportDataStore((state) => state.reportData);
  const subjectsByTeacher = useReportDataStore(
    (state) => state.subjectsByTeacher
  );

  const subjectSelect = useReportDataStore((state) => state.subjectSelect);

  const userLoggued = useAuthStore((state) => state.UserLoggued);

  const fetchData = async (
    forceRefres = false,
    getSubjet = true,
    period?: string,
    subject?: number
  ) => {
    try {
      if (getSubjet) {
        const requestSubjectByTeacher = {
          idTeacher: userLoggued.id,
          idRoom: idRoom,
        };
        await GetSubjectsByTeacher(requestSubjectByTeacher);
      }

      // Obtener el valor actual de los subjects y el subject seleccionado
      const state = useReportDataStore.getState();
      const selectedSubject = subject ?? state.subjectSelect;

      const requestReport = {
        idStudent,
        idAchievementIndicator: 0,
        period: period ?? periodSelect,
        estado: "",
        idSubject: selectedSubject,
      };
      await getPeriods(true);
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
      idSubject: subjectSelect,
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
      idSubject: subjectSelect,
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

  const displayContent = !isLoading && reportData?.vmPrinc?.length > 0;

  if (isLoading) {
    return <div></div>;
  }
  return (
    <div className="max-w-[1200px] mb-16">
      <EvaluacionHeader idRoom={idRoom} />
      {!displayContent && (
        <div className="mt-10 p-6 bg-yellow-100 border-l-4 border-yellow-500 text-yellow-800 rounded-md shadow-md max-w-[800px] mx-auto text-center">
          <h2 className="text-lg font-semibold mb-2">
            Información no disponible
          </h2>
          <p>
            No se ha encontrado información de evaluaciones para este estudiante
            en el periodo seleccionado. Por favor, verifique que el grado y la
            asignatura estén correctamente asignados. Si el problema persiste,
            comuníquese con el administrador del sistema.
          </p>
        </div>
      )}

      {displayContent && (
        <div className=" mx-auto mt-6 border rounded-lg p-4 shadow-lg bg-white">
          <h2 className="text-xl font-semibold mb-2">
            {studentname} ({studentlastname})
          </h2>

          <PeriodoAcciones
            periodSelect={periodSelect}
            periods={periods}
            subjectSelect={subjectSelect}
            subjects={subjectsByTeacher}
            onPeriodChange={(next) => {
              setPeriodo(next);
              fetchData(true, false, next, subjectSelect);
            }}
            onSubjectChange={(next) => {
              setSubjectSelect(next);
              fetchData(true, false, periodSelect, next);
            }}
            onPreview={verPdfBase64}
            onDownload={downloadPdfBase64}
          />

          <EvaluationList
            period={periodSelect}
            idStudent={idStudent}
            subject={subjectSelect}
          />

          {/* <CommentSection period={periodSelect} idStudent={idStudent} /> */}
        </div>
      )}
    </div>
  );
};
